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

    public class Graphics : MonoBehaviour
    {
        public static Graphics Instance { get; private set; }

        private GameSet gameSet;
        #region Redirected events

        public event EventHandler<StartTileSelectionEventArgs> TileSelectionStarted
        {
            add
            {
                this.gameSet.TileSelectionStarted += value;
            }

            remove
            {
                this.gameSet.TileSelectionStarted -= value;
            }
        }
        public event EventHandler<UpdateTileSelectionEventArgs> TileSelectionUpdated
        {
            add
            {
                this.gameSet.TileSelectionUpdated += value;
            }

            remove
            {
                this.gameSet.TileSelectionUpdated -= value;
            }
        }
        public event EventHandler<FinishTileSelectionEventArgs> TileSelectionFinished
        {
            add
            {
                this.gameSet.TileSelectionFinished += value;
            }

            remove
            {
                this.gameSet.TileSelectionFinished -= value;
            }
        }

        #endregion



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

    }
}