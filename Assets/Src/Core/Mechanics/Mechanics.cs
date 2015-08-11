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
        protected CellModel[][] cells;

        public CellModel[][] Cells
        {
            get { return cells; }
        }

        public int Seed { get; set; }

        protected readonly List<TileModel> Set = new List<TileModel>();
        public abstract void Deal(GameProperties properties);
        public abstract void DealTutorial();

        public void Reset()
        {
//            throw new System.NotImplementedException();
        }
    }
}

