#region imports

using System.Collections.Generic;
using System.Linq;
using Shkoda.RecognizeMe.Core.Graphics;

#endregion

namespace Assets.Src.Core.Game.Lexical

{
    public class Word
    {
        public readonly string CharSequence;

        public Word(List<Tile> tiles)
        {
            CharSequence = new string(tiles.Select(tile => tile.TileValue.Char).ToArray());
        }

        public bool IsValid
        {
            get { return WordDictionary.IsValid(this); }
        }
    }
}