#region imports

using Assets.Src.Core.Game.Cell;
using Shkoda.RecognizeMe.Core.Game.Tile;

#endregion

namespace Shkoda.RecognizeMe.Core.Game.Cell
{
    public class CellModel
    {
        public CellModel(int row, int column)
        {
            CellId = new CellId( row, column);
        }

        public CellId CellId { get; private set; }
        public TileModel Tile { get; set; }

        public override string ToString()
        {
            var content = Tile == null ? "EMPTY" : Tile.TileValue.ToString();
            return string.Format("Cell model [{0}] {1}", CellId, content);
        }
    }
}