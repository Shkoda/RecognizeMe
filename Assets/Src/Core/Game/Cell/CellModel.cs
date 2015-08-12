using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Src.Core.Game.Tile;
using Shkoda.RecognizeMe.Core.Game.Tile;

namespace Shkoda.RecognizeMe.Core.Game.Cell
{
    public class CellModel
    {
        public CellId CellId { get; private set; }
        public TileModel Tile { get; set; }

        public CellModel(int cellNumber, int row, int column)
        {
            this.CellId = new CellId(cellNumber, row, column);
        }

        public override string ToString()
        {
            var content = Tile == null ? "EMPTY" : Tile.TileValue.ToString();
            return String.Format("Cell model [{0}] {1}", CellId, content);
        }
    }
}