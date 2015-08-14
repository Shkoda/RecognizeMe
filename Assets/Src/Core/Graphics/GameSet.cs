#region imports

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Src.Core.Game.Cell;
using Assets.Src.Core.Graphics.Field;
using JetBrains.Annotations;
using Shkoda.RecognizeMe.Core.Game.Tile;
using Shkoda.RecognizeMe.Core.Graphics.Events;
using UnityEngine;

#endregion

namespace Shkoda.RecognizeMe.Core.Graphics
{
    public class GameSet : MonoBehaviour
    {
        /// <summary>
        /// Is set to true when cards were moved between decks on last frame
        /// </summary>
        private bool cardsMovedDirtyFlag;

        public bool NoModeEnabled { get; private set; }
        public bool IsSelectingTiles { get; private set; }

        public IEnumerator Init()
        {
            // Need to wait one frame for all shit to be cleaned on scene.
            // Remove this line and you're fucking dead.
            yield return null;

            CellGenerator.GenerateCells();

            allCells =
                GameObject.FindGameObjectsWithTag("Cell")
                    .Select(o => o.GetComponent<Cell>())
                    .Where(cell => cell != null)
                    .ToDictionary(cell => cell.CellId, cell => cell);
        }

        public void InitTiles(List<TileModel> tileModels)
        {
            TileGenerator.GeneratePhysicalTiles(tileModels, allCells);

            allTiles =
                GameObject.FindGameObjectsWithTag("Tile")
                    .Select(o => o.GetComponent<Tile>())
                    .Where(tile => tile != null)
                    .ToList();

            foreach (var tile in allTiles)
            {
                tile.Init();
            }
        }

        public Cell CellFromId(CellId id)
        {
            return allCells[id];
        }

        public void SetTileValue(CellId cellId, TileValue tileValue)
        {
            var cell = CellFromId(cellId);
            cell.Tile.SetTileValue(tileValue);
        }

        private void HandleSwipe()
        {
            Debug.Log("swiping pointer");
            var distance = 100;
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
                    TileSelectionUpdated(hitTile, eventArgs);
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
                TileSelectionFinished(null, eventArgs);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private void HandleNewSelectionStart()
        {
            var distance = 100;
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
                    TileSelectionStarted(hitTile, eventArgs);
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

        #region fields

        public event EventHandler<StartTileSelectionEventArgs> TileSelectionStarted = delegate { };
        public event EventHandler<UpdateTileSelectionEventArgs> TileSelectionUpdated = delegate { };
        public event EventHandler<FinishTileSelectionEventArgs> TileSelectionFinished = delegate { };

        [EditorAssigned] public SimpleCellFieldGenerator CellGenerator;
        [EditorAssigned] public TileGenerator TileGenerator;

        private List<Tile> allTiles;
        private Dictionary<CellId, Cell> allCells;

        #endregion
    }
}