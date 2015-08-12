using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalPlay.Solitaire.Tools;
using Shkoda.RecognizeMe.Core.Game.Cell;
using Shkoda.RecognizeMe.Core.Game.Tile;
using Shkoda.RecognizeMe.Core.Graphics;
using Graphics = Shkoda.RecognizeMe.Core.Graphics.Graphics;
using UnityEngine;
using Random = System.Random;

namespace Shkoda.RecognizeMe.Core.Mechanics
{
    public class SimpleMechanics : Mechanics
    {
        public SimpleMechanics()
        {
            gameProperties = Graphics.Graphics.Instance.GameProperties;

            var columnNumber = gameProperties.ColumnNumber;
            var rowNumber = gameProperties.RowNumber;

            cells = new CellModel[rowNumber][];
            for (int row = 0; row < rowNumber; row++)
            {
                cells[row] = new CellModel[columnNumber];

                for (int column = 0; column < columnNumber; column++)
                {
                    var cell = new CellModel(columnNumber*row + column, row, column);
                    cells[row][column] = cell;
                }
            }
        }

        public override void GenerateRandomTileSet(GameProperties properties)
        {
//            Debug.Log("SimpleMechanics.GenerateRandomTileSet(GameProperties properties)");
            var random = new Random();

            for (int row = 0; row < properties.RowNumber; row++)
            {
                for (int column = 0; column < properties.ColumnNumber; column++)
                {
                    TileValue tileValue = new TileValue((char) ('A' + random.Next(26)));
                    TileModel model = new TileModel(tileValue);
                    tiles.Add(model);

                    Cells[row][column].Tile = model;
                    model.Cell = Cells[row][column];

//                    Debug.Log(string.Format("generated {0}", model));
                }
            }
        }

        public override void GenerateTutorialTileSet()
        {
            throw new NotImplementedException();
        }
    }
}