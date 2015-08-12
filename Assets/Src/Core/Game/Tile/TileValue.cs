using System;
using UnityEngine;
using System.Collections;


public struct TileValue
{
    public readonly char Char;

    public TileValue(char CharValue)
    {
        Char = CharValue;
    }

    public override string ToString()
    {
        return String.Format("'{0}'", Char);
    }
}