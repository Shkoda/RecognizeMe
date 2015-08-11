using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Src.Core.Game.Tile;
using GlobalPlay.Tools;
using Shkoda.RecognizeMe.Core.Graphics.Events;
using UnityEngine;

namespace Shkoda.RecognizeMe.Core.Graphics
{
    public class GameSet : MonoBehaviour
    {
        public event EventHandler<StartTileSelectionEventArgs> TileSelectionStarted = delegate { };
        public event EventHandler<UpdateTileSelectionEventArgs> TileSelectionUpdated = delegate { };
        public event EventHandler<FinishTileSelectionEventArgs> TileSelectionFinished = delegate { };

        private List<Tile> allTiles;

        private List<Cell> allCells;

        public IEnumerator Init()
        {
            // Need to wait one frame for all shit to be cleaned on scene.
            // Remove this line and you're fucking dead.
            yield return null;

            // And now collect all new decks and all new cards 
            this.allTiles =
                GameObject.FindGameObjectsWithTag("Tile")
                    .Select(o => o.GetComponent<Tile>())
                    .Where(tile => tile != null)
                    .ToList();

            this.allCells =
                GameObject.FindGameObjectsWithTag("Cell")
                    .Select(o => o.GetComponent<Cell>())
                    .Where(cell => cell != null)
                    .ToList();

            // Move all cards to default deck
//            foreach (var tile in this.allCards)
//            {
//                this.DefaultDeck.PushAndMoveInstantly(tile);
//            }
        }

        public void InitTiles()
        {
            foreach (var tile in this.allTiles)
            {
                Debug.Log("~~ "+tile);
                tile.InitAsFlipped();
            }
        }


        public void MoveTopCard(CellId source, CellId dest, float delay)
        {
            var sourceCell = this.CellFromId(source);
            if (sourceCell == null)
            {
                Debug.Log("all cells :: "+allCells.AsString());
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
            // TODO Dictionary shit
            return this.allCells.Find(cell => cell.HasId(id));
        }

        public void SetTileValue(CellId cellId, TileValue tileValue)
        {
            var deck = this.CellFromId(cellId);
            deck.Tile.SetTileValue(tileValue);
        }
    }
}