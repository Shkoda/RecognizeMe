using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Src.Core.Game.Tile;
using JetBrains.Annotations;
using Shkoda.RecognizeMe.Core;
using Shkoda.RecognizeMe.Core.Graphics;
using UnityEngine;

namespace Assets.Src.Core.Graphics.Field
{
    public class SimpleCellFieldGenerator : MonoBehaviour
    {
        [EditorAssigned] public GameObject CellPrefab;
        [EditorAssigned] public GameObject ContainerObject;
        [EditorAssigned] public GameObject TablePlane;

        [EditorAssigned] public GameProperties GameProperties;

        private List<Cell> cells;

        public List<Cell> Cells
        {
            get { return cells; }
        }

        public void GenerateCells()
        {
            cells = new List<Cell>();
//            var bounds = TilePrefab.GetComponent<MeshCollider>().bounds;
            var cellBounds = CellPrefab.GetComponent<MeshRenderer>().bounds.size;
            var fieldBounds = TablePlane.GetComponent<MeshRenderer>().bounds.size;

            var xDefaultOffset = (fieldBounds.x - cellBounds.x*GameProperties.ColumnNumber)/2;
            var yDefaultOffset = 0;
            var zDefaultOffset = (fieldBounds.z - cellBounds.z*GameProperties.RowNumber)/2;


            Debugger.Log(String.Format("bounds = {0}", cellBounds));

            // this.glowInstance = (GameObject)Instantiate(this.EyeGlowPrefab);
            for (int row = 0; row < GameProperties.RowNumber; row++)
            {
                for (int column = 0; column < GameProperties.ColumnNumber; column++)
                {
                    GameObject cellGameObject = (GameObject) Instantiate(this.CellPrefab);
                    cellGameObject.name = String.Format("cell [{0}, {1}]", row, column);

                    cellGameObject.transform.parent = ContainerObject.transform;
                    cellGameObject.transform.position = TablePlane.transform.position;

                    float xOffset = (float) (cellBounds.x*column) - fieldBounds.x/2 + cellBounds.x/2 + xDefaultOffset;
                    float yOffset = 0.0001f;
                    float zOffset = (float) (cellBounds.z*row) - fieldBounds.z/2 + cellBounds.z/2 + zDefaultOffset;

                    cellGameObject.transform.localPosition += new Vector3(xOffset, yOffset, zOffset);

//                    Debugger.Log(String.Format("cell [{0}, {1}] has offset [{2},{3},{4}]", row, column,
//                        xOffset, yOffset, zOffset));
                    var cell = cellGameObject.GetComponent<Cell>();
                    cell.CellId = new CellId(row*GameProperties.ColumnNumber + column, row, column);
                    cells.Add(cell);
                }
            }
        }
    }
}