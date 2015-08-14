public struct TileValue
{
    public readonly char Char;

    public TileValue(char CharValue)
    {
        Char = CharValue;
    }

    public override string ToString()
    {
        return string.Format("'{0}'", Char);
    }
}