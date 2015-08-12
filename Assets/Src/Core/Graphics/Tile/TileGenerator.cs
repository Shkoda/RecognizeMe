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

        public List<Tile> GenerateTiles(List<TileModel> tileModels, Dictionary<CellId, Cell> allCells)
        {
            List<Tile> tiles = new List<Tile>();
            var cellBounds = TilePrefab.GetComponent<MeshRenderer>().bounds.size;
            var fieldBounds = TablePlane.GetComponent<MeshRenderer>().bounds.size;

            var xDefaultOffset = (fieldBounds.x - cellBounds.x*GameProperties.ColumnNumber)/2;
            var yDefaultOffset = 0;
            var zDefaultOffset = (fieldBounds.z - cellBounds.z*GameProperties.RowNumber)/2;

//            Debug.Log("allCells :: " + allCells.Values.ToList().AsString());

            foreach (var tileModel in tileModels)
            {
                var containingCell = allCells[tileModel.Cell.CellId];

                TileValue tileValue = tileModel.TileValue;

                var tile = GenerateTile(tileValue).GetComponent<Tile>();
                tile.ContainingCell = containingCell;
                containingCell.Tile = tile;
                tiles.Add(tile);
            }

            return tiles;
        }

        public GameObject GenerateTile(TileValue tileValue)
        {
//            Debug.Log(str);
            //            var bounds = TilePrefab.GetComponent<MeshCollider>().bounds;
            var tileBounds = TilePrefab.GetComponent<MeshRenderer>().bounds.size;
            var fieldBounds = TablePlane.GetComponent<MeshRenderer>().bounds.size;

            var xDefaultOffset = (fieldBounds.x - tileBounds.x*GameProperties.ColumnNumber)/2;
            var yDefaultOffset = 0;
            var zDefaultOffset = (fieldBounds.z - tileBounds.z*GameProperties.RowNumber)/2;


//            Debugger.Log(String.Format("bounds = {0}", tileBounds));

            // this.glowInstance = (GameObject)Instantiate(this.EyeGlowPrefab);
//            for (int row = 0; row < GameProperties.RowNumber; row++)
//            {
//                for (int column = 0; column < GameProperties.ColumnNumber; column++)
//                {
            GameObject tileGameObject = (GameObject) Instantiate(this.TilePrefab);


            tileGameObject.transform.parent = ContainerObject.transform;
            tileGameObject.transform.position = TablePlane.transform.position;
//
//            float xOffset = (float) (tileBounds.x*cell.CellId.Row) - fieldBounds.x/2 + tileBounds.x/2 + xDefaultOffset;
//            float yOffset = 0.0001f;
//            float zOffset = (float) (tileBounds.z*cell.CellId.Column) - fieldBounds.z/2 + tileBounds.z/2 +
//                            zDefaultOffset;
//
//            tileGameObject.transform.localPosition += new Vector3(xOffset, yOffset, zOffset);

            //                    Debugger.Log(String.Format("cell [{0}, {1}] has offset [{2},{3},{4}]", row, column,
            //                        xOffset, yOffset, zOffset));
            var tile = tileGameObject.GetComponent<Tile>();
            tile.SetTileValue(tileValue);

            tileGameObject.name = String.Format("tile  {0}", tileValue);
//                    tileGameObject.name = String.Format("tile [{0}, {1}] -- {2}", cell.CellId.Row, cell.CellId.Column, tileValue);


//                }
//            }
            return tileGameObject;
        }
    }
}