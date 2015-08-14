#region imports

using System;
using Assets.Src.Core.Game.Cell;
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
            var uvForTile = TileFace.GetUvForTile(value);
            var uvChanger = GetComponent<UvChanger>();
            uvChanger.ChangeUv(uvForTile);
            TileValue = value;
        }

        public void FlyTo(Cell dest, float delay)
        {
            FlyTo(dest, delay, true);
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

        private void FlyTo(Cell dest, float delay, bool cancelOtherAnimations, bool allowRise = true)
        {
            const float Time = 0.7f;
            const float MidPointElevation = 0.2f;

            var target = dest.GetPositionFor(this);
            var targetAngle = currentRotation*dest.GetRotationFor(this);
            var source = transform.position;
            var midPoint = source + (target - source)/2;
            midPoint.y += MidPointElevation;

            if (cancelOtherAnimations)
            {
                LeanTween.cancel(gameObject);
            }

            if (allowRise && transform.position.y < 0.2f)
            {
                LeanTween.moveX(gameObject, target.x, Time).setEase(LeanTweenType.easeOutExpo).setDelay(delay);
                LeanTween.moveZ(gameObject, target.z, Time).setEase(LeanTweenType.easeOutExpo).setDelay(delay);
                LeanTween.moveY(gameObject, transform.position.y + 0.2f, Time/7f).setDelay(delay);
                LeanTween.moveY(gameObject, target.y, 6*Time/7f)
                    .setEase(LeanTweenType.easeOutExpo)
                    .setDelay(delay + Time/7f);
            }
            else
            {
                LeanTween.move(gameObject, target, Time).setEase(LeanTweenType.easeOutExpo).setDelay(delay);
            }

            LeanTween.rotate(gameObject, targetAngle.eulerAngles, Time)
                .setEase(LeanTweenType.easeOutExpo)
                .setDelay(delay);

            MovedToDeck(this, dest.CellId);
            defaultPosition = target;
        }

        public event Action<Tile, CellId> MovedToDeck = delegate { };

        public void Init()
        {
//            Rect uvForTile = TileFace.GetUvForBack();
//            Rect uvForTile = TileFace.GetUvForBack();
            var uvForTile = TileFace.GetUvForTile(TileValue);
            var uvChanger = GetComponent<UvChanger>();
            uvChanger.ChangeUv(uvForTile);
//            this.currentRotation = defaultFlippedRotation;
//            this.ResetRotation();
        }

        private void ResetRotation()
        {
            transform.rotation = currentRotation;
        }
    }
}