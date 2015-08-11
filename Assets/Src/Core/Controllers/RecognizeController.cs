using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Src.Core.Game.Tile;
using Shkoda.RecognizeMe.Core;
using Shkoda.RecognizeMe.Core.Game.Achievements;
using Shkoda.RecognizeMe.Core.Game.Cell;
using Shkoda.RecognizeMe.Core.Graphics;
using Shkoda.RecognizeMe.Core.Graphics.Events;
using Shkoda.RecognizeMe.Core.Mechanics;
using Shkoda.RecognizeMe.Core.Mechanics.Actions;
using Shkoda.RecognizeMe.Core.Mechanics.Actions.Handlers;
using UnityEngine;
using Graphics = Shkoda.RecognizeMe.Core.Graphics.Graphics;

namespace Shkoda.Rec.Core.Controllers
{
    public class RecognizeController
    {
        private readonly List<IAction> actions = new List<IAction>();
        protected readonly Graphics Graphics;
        private Mechanics mechanics = new SimpleMechanics();
        public event Action<GameFinishedEventArgs> GameFinished = delegate { };
        public event Action TilesInitialized = delegate { };

        public Mechanics Mechanics
        {
            get { return mechanics; }
        }

//        protected Mechanics Mechanics { get; private set; }
        public GameActionHandler ActionsHandler { get; set; }
        public RecognizeController(int seed)
        {
            Graphics = Graphics.Instance;
            ActionsHandler = new  SimpleActionHandler(seed);
            mechanics.Seed = seed;
        }


        /// <summary>
        ///     Called when game is finished
        /// </summary>
        public void CleanUp()
        {
            Graphics.GatherAndClear();
            UnSubscribe();
        }

        public void StartGame()
        {
            AppController.StartRoutine(StartGameRoutine(false));
        }

        public void Subscribe()
        {
            Graphics.TileSelectionStarted += OnTileSelectionStarted;
            Graphics.TileSelectionUpdated += OnTileSelectionUpdated;
            Graphics.TileSelectionFinished += OnTileSelectionFinished;
        }

        public void UnSubscribe()
        {
            Graphics.TileSelectionStarted -= OnTileSelectionStarted;
            Graphics.TileSelectionUpdated -= OnTileSelectionUpdated;
            Graphics.TileSelectionFinished -= OnTileSelectionFinished;
        }

        public void FinishGame()
        {
            GameFinished(ActionsHandler.CreateGameFinishedEventArgs());
        }

        protected virtual void OnTileSelectionStarted(object obj, StartTileSelectionEventArgs args)
        {
        }

        protected virtual void OnTileSelectionUpdated(object obj, UpdateTileSelectionEventArgs args)
        {
        }

        protected virtual void OnTileSelectionFinished(object obj, FinishTileSelectionEventArgs args)
        {
        }

        protected IEnumerator StartGameRoutine(bool isTutorial)
        {
            // Should wait till the end of init
            yield return AppController.StartRoutine(Graphics.Init());

            this.Mechanics.Reset();

            // Init all cards
            if (isTutorial)
            {
                Mechanics.DealTutorial();
            }
            else
            {
                Mechanics.Deal(Graphics.Instance.GameProperties);
            }

            float duration = 0.03f;
            // Deal closed cards
            var delay = 0;
            const float Duration = 0.06f;
            var n = 0;

            n += this.DealTile(this.Mechanics.Cells, duration, 0);

            yield return new WaitForSeconds(n*duration);

            this.OnTilesInitialized();
        }


        protected virtual void OnTilesInitialized()
        {
            TilesInitialized();
        }

        protected int DealTile(CellModel[][] cells,
            float animationDuration,
            float delayBeforeFirstAnimation)
        {
            int dealed = 0;

            for (int row = 0, n = cells.Length; row < n; row++)
            {
                for (int column = 0; column < cells[0].Length; column++)
                {
                    dealed += this.DealTile(
                        cells[row][column],
                        animationDuration,
                        delayBeforeFirstAnimation + dealed*animationDuration);
                }
            }

            return dealed;
        }



        protected int DealTile(CellModel cell, float animationDuration, float delayBeforeFirstAnimation)
        {
            int dealed = 0;

            var cellValue = cell.Tile.TileValue;
          


            float currentDelay = delayBeforeFirstAnimation + animationDuration*dealed;
            var dstId = cell.Tile;
       
            this.Graphics.MoveTopCard(new CellId(), cell.CellId, currentDelay);
            this.Graphics.SetTileValue(cell.CellId, cellValue);
            dealed++;


            return 1;
        }


        public void OnApplicationQuit()
        {
//            throw new NotImplementedException();
        }
    }
}