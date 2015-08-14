#region imports

using System;
using UnityEngine;

#endregion

namespace Shkoda.RecognizeMe.Core.Graphics
{
    public class Tile : MonoBehaviour
    {
        public Cell ContainingCell;
        private Quaternion currentRotation;
        private Color defaultColor, highlightedColor;
        private Quaternion defaultFlippedRotation;
        private Vector3 defaultPosition;
        private Quaternion defaultRotation;
        private Guid id;
        private Vector3 rightHandVector;
        private float rotStrength;

        public Tile()
        {
            id = Guid.NewGuid();
        }

        public Guid Id
        {
            get { return id; }
        }

        public TileValue TileValue { get; private set; }

        public void Start()
        {
            id = Guid.NewGuid();
            defaultColor = gameObject.GetComponent<Renderer>().material.color;
            highlightedColor = Color.red;
        }


        public void MoveInstantlyTo(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = currentRotation*rotation;
        }

        public void SetTileValue(TileValue value)
        {
            TileValue = value;
            //find matching texture part for tile visualization
            var uvForTile = TileFace.GetUvForTile(value.Char);
            var uvChanger = GetComponent<UvChanger>();
            uvChanger.ChangeUv(uvForTile);         
        }


        public void ToggleHighlight(bool turnOn)
        {
            var material = gameObject.GetComponent<Renderer>().material;
            if (turnOn)
            {
//                Debug.Log(string.Format("Highlight on {0}", this));
                material.color = highlightedColor;
            }
            else
            {
                material.color = defaultColor;
            }
        }


        public void Kill()
        {
            ContainingCell.Tile = null;
            Destroy(gameObject);
        }
    }
}