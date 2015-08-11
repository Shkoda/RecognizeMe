using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Shkoda.RecognizeMe.Core.Graphics
{
    public class TileFace : MonoBehaviour
    {
        private static readonly Dictionary<TileValue, Rect> tilesCoordinates = new Dictionary<TileValue, Rect>();
        [EditorAssigned] public int TileColumnsPerHorizontal;
        [EditorAssigned] public int TileRowsPerVertical;
        [EditorAssigned] public Texture2D TileSheet;
        [EditorAssigned] public Rect TileSpaceInSheet;
        private static Rect backsCoordinates;
        public static Rect GetUvForCard(TileValue value)
        {
            return tilesCoordinates[value];
        }

        public static Rect GetUvForBack()
        {
            return backsCoordinates;
        }

        private void Awake()
        {
            var tileSheetHeight = TileSheet.height;
            var tileSheetWidth = TileSheet.width;

            // Coordinates in [0-1]
            var uvCardsSpace = new Rect(
                TileSpaceInSheet.xMin/tileSheetWidth,
                TileSpaceInSheet.yMin/tileSheetHeight,
                TileSpaceInSheet.width/tileSheetWidth,
                TileSpaceInSheet.height/tileSheetHeight);

            var oneCardWidth = uvCardsSpace.width/(TileColumnsPerHorizontal);
            var oneCardHeight = uvCardsSpace.height/TileRowsPerVertical;
            var oneColumnWidth = oneCardWidth;


            for (var letterNumber = 0; letterNumber < 26; letterNumber++)
            {
                var row = letterNumber/TileColumnsPerHorizontal;
                var column = letterNumber - row*TileColumnsPerHorizontal;

                var tileValue = new TileValue((char) ('A' + letterNumber));


                var verticalOffset = row*oneCardHeight;
                var horizontalOffset = column*oneColumnWidth;


                var uvRect = new Rect(horizontalOffset, verticalOffset, oneCardWidth, oneCardHeight);
                tilesCoordinates.Add(tileValue, uvRect);

                // Debug.Log(string.Format("Uv for {0} is {1}", cardValue, uvRect));
            }
        }
    }
}