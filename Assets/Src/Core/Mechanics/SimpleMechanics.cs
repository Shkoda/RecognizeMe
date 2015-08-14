#region imports

using System;
using Shkoda.RecognizeMe.Core.Game.Cell;
using Shkoda.RecognizeMe.Core.Game.Tile;

#endregion

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
            for (var row = 0; row < rowNumber; row++)
            {
                cells[row] = new CellModel[columnNumber];

                for (var column = 0; column < columnNumber; column++)
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

            for (var row = 0; row < properties.RowNumber; row++)
            {
                for (var column = 0; column < properties.ColumnNumber; column++)
                {
                    var tileValue = new TileValue((char) ('A' + random.Next(26)));
                    var model = new TileModel(tileValue);
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