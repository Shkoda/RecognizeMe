using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shkoda.RecognizeMe.Core.Game.Cell;

namespace Shkoda.RecognizeMe.Core.Game.Tile
{
    public class TileModel
    {
        public CellModel Cell { get; set; }
        public TileValue TileValue { get; private set; }

        public TileModel(TileValue tileValue)
        {
            TileValue = tileValue;
        }

        public override string ToString()
        {
            var cellString = Cell == null ? "[UNDEFINED]" : Cell.CellId.ToString();
            return String.Format("tile model {0} with cell model {1}", TileValue, cellString);
        }
    }
}