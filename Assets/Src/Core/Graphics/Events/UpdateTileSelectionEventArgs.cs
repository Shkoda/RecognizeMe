using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkoda.RecognizeMe.Core.Graphics.Events
{
    public class UpdateTileSelectionEventArgs : EventArgs
    {    public readonly Tile Tile;
        public bool Cancel { get; set; }

        public UpdateTileSelectionEventArgs(Tile tile)
        {
            Tile = tile;
        }
    }
}