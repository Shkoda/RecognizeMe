using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private GameProperties gameProperties;
        private readonly List<IAction> actions = new List<IAction>();
        protected readonly Graphics Graphics;
        private Mechanics mechanics = new SimpleMechanics();
        public event Action<GameFinishedEventArgs> GameFinished = delegate { };
        public event Action TilesInitialized = delegate { };

        private TileGenerator tileGenerator = new TileGenerator(); //todo make edaitor assigned?

        private List<Tile> HighlightedTiles = new List<Tile>();

        public Mechanics Mechanics
        {
            get { return mechanics; }
        }

//        protected Mechanics Mechanics { get; private set; }
        public GameActionHandler ActionsHandler { get; set; }

        public RecognizeController(int seed)
        {
            Graphics = Graphics.Instance;
            gameProperties = Graphics.GameProperties;
            ActionsHandler = new SimpleActionHandler(seed);
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

        #region subscription

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

        #endregion

        public void FinishGame()
        {
            GameFinished(ActionsHandler.CreateGameFinishedEventArgs());
        }

        protected virtual void OnTileSelectionStarted(object obj, StartTileSelectionEventArgs args)
        {
            HighlightedTiles.Add(args.Tile);
            args.Tile.ToggleHighlight(true);
        }

        protected virtual void OnTileSelectionUpdated(object obj, UpdateTileSelectionEventArgs args)
        {
            if (!IsAlreadyHighlighted(args.Tile))
            {
                HighlightedTiles.Add(args.Tile);
                args.Tile.ToggleHighlight(true);
            }
         
        }

        private bool IsAlreadyHighlighted(Tile tile)
        {
            return HighlightedTiles.Any(highlighted => highlighted.Id.Equals(tile.Id));
        }

        protected virtual void OnTileSelectionFinished(object obj, FinishTileSelectionEventArgs args)
        {
            HighlightedTiles.ForEach(tile=>tile.ToggleHighlight(false));
            HighlightedTiles.Clear();
        }


        protected IEnumerator StartGameRoutine(bool isTutorial)
        {
            // Should wait till the end of init
//          `1``````````````````  yield return AppController.StartRoutine(Graphics.Init());

            this.Mechanics.Reset();
            // Init all cards
            if (isTutorial)
            {
                Mechanics.GenerateTutorialTileSet();
            }
            else
            {
                Mechanics.GenerateRandomTileSet(Graphics.Instance.GameProperties);
            }

            Graphics.InitTiles(Mechanics.Tiles);

            var dealedTilesNumber = VisualDealTiles();
            float duration = 0.03f;
            yield return new WaitForSeconds(dealedTilesNumber*duration);

            this.OnTilesInitialized();
        }


        protected int VisualDealTiles()
        {
            float duration = 0.03f;
            var delay = 0;
            for (int row = 0; row < gameProperties.RowNumber; row++)
            {
                for (int column = 0; column < gameProperties.ColumnNumber; column++)
                {
                    this.DealTile(this.Mechanics.Cells[row][column], duration, 0);
                }
            }

            return gameProperties.RowNumber*gameProperties.ColumnNumber;
        }


        protected virtual void OnTilesInitialized()
        {
            TilesInitialized();
        }


        protected void DealTile(CellModel cell, float animationDuration, float delayBeforeFirstAnimation)
        {
//            Debug.Log(string.Format("try to deal to cell {0}", cell));

            var tileValue = cell.Tile.TileValue;


//            float currentDelay = delayBeforeFirstAnimation + animationDuration*dealed;
            float currentDelay = delayBeforeFirstAnimation;
            var dstId = cell.Tile;

//            this.Graphics.MoveTopCard(new CellId(), cell.CellId, currentDelay);
            this.Graphics.SetTileValue(cell.CellId, tileValue);
//            Debug.Log("recognizeController.DealTile is done");
        }


        public void OnApplicationQuit()
        {
//            throw new NotImplementedException();
        }
    }
}