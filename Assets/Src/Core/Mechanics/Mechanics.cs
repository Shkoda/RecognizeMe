#region imports

using System.Collections.Generic;
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

        public void Reset()
        {
//            throw new System.NotImplementedException();
        }
    }
}