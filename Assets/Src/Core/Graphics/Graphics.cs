#region imports

using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Src.Core.Game.Cell;
using JetBrains.Annotations;
using Shkoda.RecognizeMe.Core.Game.Tile;
using Shkoda.RecognizeMe.Core.Graphics.Events;
using UnityEngine;

#endregion

namespace Shkoda.RecognizeMe.Core.Graphics
{
    public class Graphics : MonoBehaviour
    {
        #region fields & properties

        [EditorAssigned] public GameProperties GameProperties;
        private GameSet gameSet;
        private bool isGamePaused;
        private bool shouldStartTutorial;
        public static Graphics Instance { get; private set; }
        public bool IsUiWindowOpened { get; set; }
        public event Action<Game.Game> GameChosen = delegate { };
        public event Action GameClosed = delegate { };

        #endregion

        #region Awake() Start() 

        [UsedImplicitly]
        private void Awake()
        {
            Instance = this;
            gameSet = GetComponent<GameSet>();
        }

        // Use this for initialization
        private void Start()
        {
            StartGame();
        }

        #endregion

        // Update is called once per frame
        private void Update()
        {
            Pointer.Update();
            if (!isGamePaused)
            {
                gameSet.ProcessPointerEvents();
            }
        }

        public void StartGame()
        {
            StartCoroutine(Init());
        }

        public IEnumerator Init()
        {
//            Debug.Log("Graphics.Init()");
            yield return AppController.StartRoutine(gameSet.Init());
        }

        public void InitTiles(List<TileModel> Set)
        {
            gameSet.InitTiles(Set);
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

        public void SetTileValue(CellId cellId, TileValue tileValue)
        {
            gameSet.SetTileValue(cellId, tileValue);
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
            NewGame(Game.Game.RandomSeed());
        }

        public void NewGame(int seed)
        {
            shouldStartTutorial = false;
            Play(false, seed);
        }

        private void Play(bool tutorial, int seed)
        {
            var game = new Game.Game(seed);
            IsUiWindowOpened = true;
            isGamePaused = false;
            GameChosen(game);
        }

        #region Redirected events

        public event EventHandler<StartTileSelectionEventArgs> TileSelectionStarted
        {
            add { gameSet.TileSelectionStarted += value; }

            remove { gameSet.TileSelectionStarted -= value; }
        }

        public event EventHandler<UpdateTileSelectionEventArgs> TileSelectionUpdated
        {
            add { gameSet.TileSelectionUpdated += value; }

            remove { gameSet.TileSelectionUpdated -= value; }
        }

        public event EventHandler<FinishTileSelectionEventArgs> TileSelectionFinished
        {
            add { gameSet.TileSelectionFinished += value; }

            remove { gameSet.TileSelectionFinished -= value; }
        }

        #endregion
    }
}