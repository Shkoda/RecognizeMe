using Assets.Src.Core.Game.Tile;
using Shkoda.RecognizeMe.Core.Graphics.Events;

namespace Shkoda.RecognizeMe.Core.Graphics
{
    using UnityEngine;
    using System.Collections;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using UnityEngine;
    using Object = UnityEngine.Object;

    using Shkoda.RecognizeMe.Core.Game;
    public class Graphics : MonoBehaviour
    {
        public static Graphics Instance { get; private set; }

        [EditorAssigned] public GameProperties GameProperties;

        private GameSet gameSet;
        private bool shouldStartTutorial;
        private bool isGamePaused;

        public event Action<Game> GameChosen = delegate { };
        public event Action GameClosed = delegate { };
        #region Redirected events

        public event EventHandler<StartTileSelectionEventArgs> TileSelectionStarted
        {
            add { this.gameSet.TileSelectionStarted += value; }

            remove { this.gameSet.TileSelectionStarted -= value; }
        }

        public event EventHandler<UpdateTileSelectionEventArgs> TileSelectionUpdated
        {
            add { this.gameSet.TileSelectionUpdated += value; }

            remove { this.gameSet.TileSelectionUpdated -= value; }
        }

        public event EventHandler<FinishTileSelectionEventArgs> TileSelectionFinished
        {
            add { this.gameSet.TileSelectionFinished += value; }

            remove { this.gameSet.TileSelectionFinished -= value; }
        }

        #endregion

        [UsedImplicitly]
        private void Awake()
        {
            Instance = this;
            this.gameSet = this.GetComponent<GameSet>();
        }

        // Use this for initialization
        private void Start()
        {
            this.StartGame();
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void StartGame()
        {
            this.StartCoroutine(this.Init());
        }

        public IEnumerator Init()
        {
            yield return AppController.StartRoutine(this.gameSet.Init());
        }

        public void UpdateTimer(long time)
        {
//            this.MainGui.GameGui.UpdateTimer(TimeSpan.FromMilliseconds(time));
        }

        public void GatherAndClear()
        {
//            throw new NotImplementedException();
        }

        public void CleanUpGameHud()
        {
//            if (this.MainGui && this.MainGui.GameGui)
//            {
//                this.MainGui.GameGui.UpdateGameHUD(TimeSpan.FromSeconds(0), 0, 0);
//            }
        }


        public void MoveTopCard(CellId source, CellId dest, float delay = 0f)
        {
            this.gameSet.MoveTopCard(source, dest, delay);
        }

        public void SetTileValue(CellId cellId, TileValue tileValue)
        {
            this.gameSet.SetTileValue(cellId, tileValue);
        }


        public void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 50, 50), "Start"))
            {
                NewGame();
            }
        }

        public void NewGame()
        {
            this.NewGame(Game.RandomSeed());
        }

        public void NewGame(int seed)
        {
            this.shouldStartTutorial = false;
//            this.MainGui.GameGui.DisableTutorial();
//            this.HideLooseGui();
//            this.GameClosed();

//            this.MainGui.SwitchGuiToGame();

//                KlondikeDeal.RemoveSavedGame();

            this.Play( false, seed);
        }

        private void Play(bool tutorial, int seed)
        {
            Game game = new Game(seed);

           
            this.IsUiWindowOpened = true;
            this.isGamePaused = false;
            this.gameSet.InitTiles();
//            this.MainGui.GameGui.SetButtonsEnabled(false);
            this.GameChosen(game);
        }

        public bool IsUiWindowOpened { get; set; }
    }
}