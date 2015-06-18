using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shkoda.RecognizeMe.Core.Graphics.Events;
using UnityEngine;

namespace Shkoda.RecognizeMe.Core.Graphics
{
   public class GameSet : MonoBehaviour
    {
        public event EventHandler<StartTileSelectionEventArgs> TileSelectionStarted = delegate { };
        public event EventHandler<UpdateTileSelectionEventArgs> TileSelectionUpdated = delegate { };
        public event EventHandler<FinishTileSelectionEventArgs> TileSelectionFinished= delegate { };

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
//            foreach (var card in this.allCards)
//            {
//                this.DefaultDeck.PushAndMoveInstantly(card);
//            }
        }


    }
}
