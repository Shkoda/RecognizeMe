namespace Assets.Src.Core.Game.Cell
{
    public struct CellId
    {
        public readonly int Column;
        public readonly int Number;
        public readonly int Row;

        public CellId(int number, int row, int column)
        {
            Number = number;
            Row = row;
            Column = column;
        }

        public bool Equals(CellId other)
        {
            return Number == other.Number
                   && Row == other.Row
                   && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is CellId && Equals((CellId) obj);
        }

        public override string ToString()
        {
            return string.Format("CellId #{0} [{1} {2}]", Number, Row, Column);
        }
    }
}