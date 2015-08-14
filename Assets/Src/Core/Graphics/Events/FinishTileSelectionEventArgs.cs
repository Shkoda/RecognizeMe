#region imports

using System;

#endregion

namespace Shkoda.RecognizeMe.Core.Graphics.Events
{
    public class FinishTileSelectionEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }
}