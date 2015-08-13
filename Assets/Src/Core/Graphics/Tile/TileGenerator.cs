using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Src.Core.Game.Tile;
using GlobalPlay.Tools;
using JetBrains.Annotations;
using Shkoda.RecognizeMe.Core.Game.Tile;
using UnityEngine;
using Random = System.Random;

namespace Shkoda.RecognizeMe.Core.Graphics
{
    public class TileGenerator : MonoBehaviour
    {
        [EditorAssigned] public GameObject TilePrefab;
        [EditorAssigned] public GameObject ContainerObject;
        [EditorAssigned] public GameObject TablePlane;
        [EditorAssigned] public GameProperties GameProperties;

        private Vector3 cellBounds;
        private Vector3 fieldBounds;

        private float xDefaultOffset, yDefaultOffset, zDefaultOffset;

        public void Awake()
        {
            cellBounds = TilePrefab.GetComponent<MeshRenderer>().bounds.size;
            fieldBounds = TablePlane.GetComponent<MeshRenderer>().bounds.size;

            xDefaultOffset = (fieldBounds.x - cellBounds.x*GameProperties.ColumnNumber)/2;
            yDefaultOffset = 0;
            zDefaultOffset = (fieldBounds.z - cellBounds.z*GameProperties.RowNumber)/2;
        }

        public List<Tile> GeneratePhysicalTiles(List<TileModel> tileModels, Dictionary<CellId, Cell> allCells)
        {
            List<Tile> tiles = new List<Tile>();


//            Debug.Log("allCells :: " + allCells.Values.ToList().AsString());

            foreach (var tileModel in tileModels)
            {
                var containingCell = allCells[tileModel.Cell.CellId];
                var row = containingCell.CellId.Row;
                var column = containingCell.CellId.Column;

                TileValue tileValue = tileModel.TileValue;

                var tileGameObject = GenerateTile(tileValue);

                tileGameObject.transform.position = TablePlane.transform.position;

                var offset = TileOffset(row, column);
                tileGameObject.transform.localPosition += offset;


                var tile = tileGameObject.GetComponent<Tile>();
                tile.ContainingCell = containingCell;
                containingCell.Tile = tile;
                tiles.Add(tile);
            }

            return tiles;
        }

        private Vector3 TileOffset(int row, int column)
        {
            float xOffset = (float) (cellBounds.x*column) - fieldBounds.x/2 + cellBounds.x/2 + xDefaultOffset;
            float yOffset = 0.0002f;
            float zOffset = (float) (cellBounds.z*row) - fieldBounds.z/2 + cellBounds.z/2 + zDefaultOffset;
            return new Vector3(xOffset, yOffset, zOffset);
        }


//        private void SetInitioalPosition(GameObject tile, )

        public GameObject GenerateTile(TileValue tileValue)
        {
            GameObject tileGameObject = (GameObject) Instantiate(this.TilePrefab);


            tileGameObject.transform.parent = ContainerObject.transform;
            tileGameObject.transform.position = TablePlane.transform.position;

            var tile = tileGameObject.GetComponent<Tile>();
            tile.SetTileValue(tileValue);

            tileGameObject.name = String.Format("tile {0}", tileValue);

            return tileGameObject;
        }
    }
}