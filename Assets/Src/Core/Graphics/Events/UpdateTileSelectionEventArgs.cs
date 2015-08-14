#region imports

using System;

#endregion

namespace Shkoda.RecognizeMe.Core.Graphics.Events
{
    public class UpdateTileSelectionEventArgs : EventArgs
    {
        public readonly Tile Tile;

        public UpdateTileSelectionEventArgs(Tile tile)
        {
            Tile = tile;
        }

        public bool Cancel { get; set; }
    }
}