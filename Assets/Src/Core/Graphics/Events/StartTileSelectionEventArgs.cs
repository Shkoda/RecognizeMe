#region imports

using System;

#endregion

namespace Shkoda.RecognizeMe.Core.Graphics.Events
{
    public class StartTileSelectionEventArgs : EventArgs
    {
        public readonly Tile Tile;

        public StartTileSelectionEventArgs(Tile tile)
        {
            Tile = tile;
        }

        public bool Cancel { get; set; }
    }
}