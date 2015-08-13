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
        public event EventHandler<StartTileSelectionEventArgs> TileSelectionStarted = delegate { };
        public event EventHandler<UpdateTileSelectionEventArgs> TileSelectionUpdated = delegate { };
        public event EventHandler<FinishTileSelectionEventArgs> TileSelectionFinished = delegate { };

        [EditorAssigned] public SimpleCellFieldGenerator CellGenerator;
        [EditorAssigned] public TileGenerator TileGenerator;
        private List<Tile> allTiles;

//        private List<Cell> allCells;
        private Dictionary<CellId, Cell> allCells;

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

//            Debug.Log("GameSet.Init() is done");
        }

        public void InitTiles(List<TileModel> tileModels)
        {
//            Debug.Log("GameSte.initTiles()");
            var generatedTiles = TileGenerator.GeneratePhysicalTiles(tileModels, allCells);
//            Debug.Log(string.Format("GameSte.initTiles() -- generated tiles :: {0}", generatedTiles.AsString()));

            //collect all new decks and all new tiles 
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
    }
}