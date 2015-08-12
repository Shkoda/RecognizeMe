using System.Collections.Generic;
using GlobalPlay.Solitaire.Tools;
using Shkoda.RecognizeMe.Core.Game.Cell;
using Shkoda.RecognizeMe.Core.Game.Tile;
using Shkoda.RecognizeMe.Core.Graphics;

namespace Shkoda.RecognizeMe.Core.Mechanics
{
    using UnityEngine;
    using System.Collections;

    public abstract class Mechanics
    {
        protected GameProperties gameProperties;
        protected CellModel[][] cells;

        public CellModel[][] Cells
        {
            get { return cells; }
        }

        public int Seed { get; set; }

        protected readonly List<TileModel> tiles = new List<TileModel>();

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