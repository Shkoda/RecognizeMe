#region imports

using System;
using System.Collections.Generic;
using Assets.Src.Core.Game.Lexical;
using Shkoda.RecognizeMe.Core.Game.Cell;
using Shkoda.RecognizeMe.Core.Game.Tile;

#endregion

namespace Shkoda.RecognizeMe.Core.Mechanics
{
    public abstract class Mechanics
    {
        protected readonly List<TileModel> tiles = new List<TileModel>();
        protected CellModel[][] cells;
        protected GameProperties gameProperties;
        protected Word word;
        public CellModel[][] Cells
        {
            get { return cells; }
        }

        public int Seed { get; set; }

        public List<TileModel> Tiles
        {
            get { return tiles; }
        }

        public abstract void GenerateRandomTileSet(GameProperties properties);
        public abstract void GenerateTutorialTileSet();

        public virtual void NewWord(Char Char)
        {
            word = new Word(Char);
        }

        public virtual bool UpdateWord(Char Char)
        {
            Word newWord = word + Char;
            if (newWord.IsValid)
            {
                word = newWord;
                return true;
            }
            return false;
        }

        public virtual bool DestroyWord()
        {
            return false;

        }


        public void Reset()
        {
//            throw new System.NotImplementedException();
        }
    }
}