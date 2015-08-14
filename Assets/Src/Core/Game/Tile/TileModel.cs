#region imports

using Shkoda.RecognizeMe.Core.Game.Cell;

#endregion

namespace Shkoda.RecognizeMe.Core.Game.Tile
{
    public class TileModel
    {
        public TileModel(TileValue tileValue)
        {
            TileValue = tileValue;
        }

        public CellModel Cell { get; set; }
        public TileValue TileValue { get; private set; }

        public override string ToString()
        {
            var cellString = Cell == null ? "[UNDEFINED]" : Cell.CellId.ToString();
            return string.Format("tile model {0} with cell model {1}", TileValue, cellString);
        }
    }
}