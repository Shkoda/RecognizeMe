using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkoda.RecognizeMe.Core.Graphics.Events
{
    public class StartTileSelectionEventArgs : EventArgs
    {
        public readonly Tile Tile;
        public bool Cancel { get; set; }

        public StartTileSelectionEventArgs(Tile tile)
        {
            Tile = tile;
        }
    }
}