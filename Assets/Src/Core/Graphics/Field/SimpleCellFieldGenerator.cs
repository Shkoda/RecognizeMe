using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Src.Core.Graphics.Field
{
    class SimpleCellFieldGenerator : MonoBehaviour
    {
        [EditorAssigned]
        public GameObject CellPrefab;
        [EditorAssigned]
        public GameObject ContainerObject;
        [EditorAssigned] 
        public GameObject TablePlane;
        [EditorAssigned]
        public int RowNumber = 8;
        [EditorAssigned]
        public int ColumnNumber = 8; 
        
        public void Start()
        {
//            var bounds = CellPrefab.GetComponent<MeshCollider>().bounds;
            var cellBounds = CellPrefab.GetComponent<MeshRenderer>().bounds.size;
            var fieldBounds = TablePlane.GetComponent<MeshRenderer>().bounds.size;

            var xDefaultOffset = (fieldBounds.x - cellBounds.x*ColumnNumber)/2;
            var yDefaultOffset = 0;
            var zDefaultOffset = (fieldBounds.z - cellBounds.z * ColumnNumber) / 2;


            Debugger.Log(String.Format("bounds = {0}", cellBounds));
            
            // this.glowInstance = (GameObject)Instantiate(this.EyeGlowPrefab);
            for (int row = 0; row < RowNumber; row++)
            {
                for (int column = 0; column < ColumnNumber; column++)
                {
                    GameObject cell = (GameObject) Instantiate(this.CellPrefab);
                    cell.transform.parent = ContainerObject.transform;
                    cell.transform.position = TablePlane.transform.position;

                    float xOffset = (float) (cellBounds.x* column) - fieldBounds.x/2 + cellBounds.x/2 + xDefaultOffset;
                    float yOffset = 0.0001f;
                    float zOffset = (float)(cellBounds.z * row) - fieldBounds.z / 2 +cellBounds.z/2 + zDefaultOffset;

                    cell.transform.localPosition += new Vector3( xOffset, yOffset, zOffset);

                    Debugger.Log(String.Format("cell [{0}, {1}] has offset [{2},{3},{4}]", row, column,
                        xOffset, yOffset, zOffset));

                }
            }
        }
    }
}
