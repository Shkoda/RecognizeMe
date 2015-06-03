using System;
using UnityEngine;

namespace Shkoda.RecognizeMe.Core.Graphics
{
    public class Tile : MonoBehaviour
    {
        private Guid id;

        public Guid Id
        {
            get { return id; }
        }

        public Cell ContainingCell;
        private Quaternion currentRotation;
        private Quaternion defaultFlippedRotation;
        private Vector3 defaultPosition;
        private Quaternion defaultRotation;
        private Vector3 rightHandVector;
        private float rotStrength;
        public TileValue TileValue { get; private set; }

        public Tile()
        {
            id = System.Guid.NewGuid();
        }

        public void MoveInstantlyTo(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = currentRotation*rotation;
        }

        public void SetTileValue(TileValue value)
        {
            Rect uvForCard = TileFace.GetUvForCard(value);
            var uvChanger = GetComponent<UvChanger>();
            uvChanger.ChangeUv(uvForCard, true);
            this.TileValue = value;
        }
    }
}