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

        public Word()
        {
            CharSequence = string.Empty;
        }

        public Word(List<Tile> tiles)
        {
            CharSequence = new string(tiles.Select(tile => tile.TileValue.Char).ToArray());
        }

        public Word(string charSequence)
        {
            CharSequence = charSequence;
        }    
        public Word(char Char)
        {
            CharSequence = Char.ToString();
        }

        public bool IsValid
        {
            get { return WordDictionary.IsValid(this); }
        }

        public override string ToString()
        {
            var description = IsValid ? "valid word" : "invalid sequence";
            return string.Format("{0} '{1}'", description, CharSequence);
        }

        #region operator +

        public static Word operator +(Word w1, Word w2)
        {
            return new Word(w1.CharSequence + w2.CharSequence);
        }

        public static Word operator +(Word w, Tile t)
        {
            return new Word(w.CharSequence + t.TileValue.Char);
        }

        public static Word operator +(Word w, char c)
        {
            return new Word(w.CharSequence + c);
        }

        #endregion
    }
}