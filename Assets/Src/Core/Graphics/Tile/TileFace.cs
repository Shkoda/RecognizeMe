#region imports

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

#endregion

namespace Shkoda.RecognizeMe.Core.Graphics
{
    public class TileFace : MonoBehaviour
    {
        private static readonly Dictionary<Char, Rect> tilesCoordinates = new Dictionary<Char, Rect>();
        [EditorAssigned] public int TileColumnsPerHorizontal;
        [EditorAssigned] public int TileRowsPerVertical;
        [EditorAssigned] public Texture2D TileSheet;
        [EditorAssigned] public Rect TileSpaceInSheet;

        public static Rect GetUvForTile(Char Char)
        {
//            Debug.Log(string.Format("uv for {0} is {1}", value, tilesCoordinates[value]));
            return tilesCoordinates[Char];
        }

        private void Awake()
        {
            var tileSheetHeight = TileSheet.height;
            var tileSheetWidth = TileSheet.width;

            if (TileSpaceInSheet.width < 0.1 || TileSpaceInSheet.height < 0.1)
            {
                TileSpaceInSheet.width = TileSheet.width;
                TileSpaceInSheet.height = TileSheet.height;
            }

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

                var tileValue =(char)('A' + letterNumber);

                var verticalOffset = row*oneCardHeight;
                var horizontalOffset = column*oneColumnWidth;


                var uvRect = new Rect(horizontalOffset, verticalOffset, oneCardWidth, oneCardHeight);
                tilesCoordinates.Add(tileValue, uvRect);

//                 Debug.Log(string.Format("Uv for {0} is {1} in row={2} column={3}", tileValue, uvRect, row, column));
            }
        }
    }
}