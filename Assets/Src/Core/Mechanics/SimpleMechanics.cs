using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalPlay.Solitaire.Tools;
using Shkoda.RecognizeMe.Core.Game.Cell;
using Shkoda.RecognizeMe.Core.Game.Tile;
using Shkoda.RecognizeMe.Core.Graphics;
using Graphics = Shkoda.RecognizeMe.Core.Graphics.Graphics;

namespace Shkoda.RecognizeMe.Core.Mechanics
{
    public class SimpleMechanics : Mechanics
    {
        private GameProperties gameProperties;

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
                    var cell = new CellModel(columnNumber * row + column, row, column);
                    cells[row][column] = cell;
                }
            }
        }

        public override void Deal(GameProperties properties)
        {
            var random = new Random();

            for (int row = 0; row < properties.RowNumber; row++)
            {
                for (int column = 0; column < properties.ColumnNumber; column++)
                {
                    TileValue tileValue = new TileValue((char) random.Next(26));
                    TileModel model = new TileModel(tileValue);
                    Set.Add(model);

                    Cells[row][column].Tile = model;
                }
            }
        }

        public override void DealTutorial()
        {
            throw new NotImplementedException();
        }
    }
}