using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Src.Core.Game.Tile;
using Assets.Src.Core.Graphics.Field;
using GlobalPlay.Tools;
using JetBrains.Annotations;
using Shkoda.RecognizeMe.Core.Game.Tile;
using Shkoda.RecognizeMe.Core.Graphics.Events;
using UnityEngine;

namespace Shkoda.RecognizeMe.Core.Graphics
{
    public class GameSet : MonoBehaviour
    {
        public bool NoModeEnabled { get; private set; }

        /// <summary>
        /// Is set to true when cards were moved between decks on last frame
        /// </summary>
        private bool cardsMovedDirtyFlag;

        public bool IsSelectingTiles { get; private set; }

        #region fields

        public event EventHandler<StartTileSelectionEventArgs> TileSelectionStarted = delegate { };
        public event EventHandler<UpdateTileSelectionEventArgs> TileSelectionUpdated = delegate { };
        public event EventHandler<FinishTileSelectionEventArgs> TileSelectionFinished = delegate { };

        [EditorAssigned] public SimpleCellFieldGenerator CellGenerator;
        [EditorAssigned] public TileGenerator TileGenerator;

        private List<Tile> allTiles;
        private Dictionary<CellId, Cell> allCells;

        #endregion

        public IEnumerator Init()
        {
            // Need to wait one frame for all shit to be cleaned on scene.
            // Remove this line and you're fucking dead.
            yield return null;

            CellGenerator.GenerateCells();

            this.allCells =
                GameObject.FindGameObjectsWithTag("Cell")
                    .Select(o => o.GetComponent<Cell>())
                    .Where(cell => cell != null)
                    .ToDictionary(cell => cell.CellId, cell => cell);
        }

        public void InitTiles(List<TileModel> tileModels)
        {
            TileGenerator.GeneratePhysicalTiles(tileModels, allCells);

            this.allTiles =
                GameObject.FindGameObjectsWithTag("Tile")
                    .Select(o => o.GetComponent<Tile>())
                    .Where(tile => tile != null)
                    .ToList();

            foreach (var tile in this.allTiles)
            {
                tile.Init();
            }
        }


        public void MoveTopCard(CellId source, CellId dest, float delay)
        {
            var sourceCell = this.CellFromId(source);
            if (sourceCell == null)
            {
                Debug.Log("all cells :: " + allCells.Keys.ToList().AsString());
                Debug.LogError(String.Format("Cell {0} not found", source));
            }

            var tile = sourceCell.PopTile();
            this.FlyCardTo(CellFromId(dest), delay, tile);
        }

        private float FlyCardTo(Cell dest, float delay, Tile tile)
        {
            dest.PushTile(tile);
            float time = 0;


            tile.FlyTo(dest, delay);
            time = delay + .7f;


//           this.OnCardsMovedBetweenDecks();
            return time;
        }


        public Cell CellFromId(CellId id)
        {
            return this.allCells[id];
        }

        public void SetTileValue(CellId cellId, TileValue tileValue)
        {
            var cell = this.CellFromId(cellId);
//            Debug.Log("GameSet.SetTileValue() " + cell);
            cell.Tile.SetTileValue(tileValue);
        }

        //-------------------------------------------


        private void HandleSwipe()
        {
            Debug.Log("swiping pointer");
            int distance = 100;
            RaycastHit hit;
            if (Physics.Raycast(Pointer.PointerRayInWorldspace,
                out hit,
                distance,
                LayerMask.GetMask("Tiles")))
            {
                var hitTile = hit.transform.gameObject.GetComponent<Tile>();
                var eventArgs = new UpdateTileSelectionEventArgs(hitTile);
                try
                {
                    this.TileSelectionUpdated(hitTile, eventArgs);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
        }

        private void HandleReleaseAfterSwipe()
        {
            Debug.Log("finish selection");
            IsSelectingTiles = false;
            var eventArgs = new FinishTileSelectionEventArgs();

            try
            {
                this.TileSelectionFinished(null, eventArgs);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private void HandleNewSelectionStart()
        {
            int distance = 100;
            RaycastHit hit;
            if (Physics.Raycast(Pointer.PointerRayInWorldspace,
                out hit,
                distance,
                LayerMask.GetMask("Tiles")))
            {
                //start selection
                Debug.Log("start selection");
                var hitTile = hit.transform.gameObject.GetComponent<Tile>();
                IsSelectingTiles = true;
                var eventArgs = new StartTileSelectionEventArgs(hitTile);
                try
                {
                    this.TileSelectionStarted(hitTile, eventArgs);
                    //  
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
        }


        public void ProcessPointerEvents()
        {
            // Player selected something on screen
            if (IsSelectingTiles && Pointer.IsDown)
            {
                HandleSwipe();
            }
            else if (!IsSelectingTiles && Pointer.IsDown)
            {
                HandleNewSelectionStart();
            }
            else if (IsSelectingTiles && !Pointer.IsDown)
            {
                HandleReleaseAfterSwipe();
            }
        }
    }
}