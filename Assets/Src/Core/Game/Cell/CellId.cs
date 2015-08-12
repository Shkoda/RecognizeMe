using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Src.Core.Game.Tile
{
    public struct CellId
    {
        public readonly int Number;
        public readonly int Row;
        public readonly int Column;

        public CellId(int number, int row, int column)
        {
            Number = number;
            Row = row;
            Column = column;
        }

        public bool Equals(CellId other)
        {
            return this.Number == other.Number
                   && this.Row == other.Row
                   && this.Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is CellId && this.Equals((CellId) obj);
        }

        public override string ToString()
        {
            return String.Format("CellId #{0} [{1} {2}]", Number, Row, Column);
        }
    }
}