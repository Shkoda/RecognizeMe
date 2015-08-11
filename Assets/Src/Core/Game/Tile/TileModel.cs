using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shkoda.RecognizeMe.Core.Game.Cell;

namespace Shkoda.RecognizeMe.Core.Game.Tile
{
   public class TileModel
    {
       public CellModel Cell{ get; set; }
       public TileValue TileValue { get; private set; }

       public TileModel(TileValue tileValue)
       {
           TileValue = tileValue;
       }
    }
}
