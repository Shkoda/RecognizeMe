namespace Assets.Src.Core.Game.Cell
{
    public struct CellId
    {
        public readonly int Column;
        public readonly int Row;

        public CellId( int row, int column)
        {
            Row = row;
            Column = column;
        }

        public bool Equals(CellId other)
        {
            return Row == other.Row
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
            return string.Format("CellId [{1} {2}]",  Row, Column);
        }
    }
}