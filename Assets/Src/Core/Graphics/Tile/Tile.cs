using System;
using Assets.Src.Core.Game.Tile;
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

        private Color defaultColor, highlightedColor;

        public void Start()
        {
            defaultColor = this.gameObject.GetComponent<Renderer>().material.color;
            highlightedColor = Color.red;
        }

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
            Rect uvForTile = TileFace.GetUvForTile(value);
            var uvChanger = GetComponent<UvChanger>();
            uvChanger.ChangeUv(uvForTile);
            this.TileValue = value;
        }

        public void FlyTo(Cell dest, float delay)
        {
            this.FlyTo(dest, delay, true);
        }

        
        public void ToggleHighlight(bool turnOn)
        {
            var material = this.gameObject.GetComponent<Renderer>().material;
            if (turnOn)
            {
                Debug.Log(String.Format("Highlight on {0}", this));
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
            var targetAngle = this.currentRotation*dest.GetRotationFor(this);
            var source = this.transform.position;
            var midPoint = source + (target - source)/2;
            midPoint.y += MidPointElevation;

            if (cancelOtherAnimations)
            {
                LeanTween.cancel(this.gameObject);
            }

            if (allowRise && this.transform.position.y < 0.2f)
            {
                LeanTween.moveX(this.gameObject, target.x, Time).setEase(LeanTweenType.easeOutExpo).setDelay(delay);
                LeanTween.moveZ(this.gameObject, target.z, Time).setEase(LeanTweenType.easeOutExpo).setDelay(delay);
                LeanTween.moveY(this.gameObject, this.transform.position.y + 0.2f, Time/7f).setDelay(delay);
                LeanTween.moveY(this.gameObject, target.y, 6*Time/7f)
                    .setEase(LeanTweenType.easeOutExpo)
                    .setDelay(delay + Time/7f);
            }
            else
            {
                LeanTween.move(this.gameObject, target, Time).setEase(LeanTweenType.easeOutExpo).setDelay(delay);
            }

            LeanTween.rotate(this.gameObject, targetAngle.eulerAngles, Time)
                .setEase(LeanTweenType.easeOutExpo)
                .setDelay(delay);

            this.MovedToDeck(this, dest.CellId);
            this.defaultPosition = target;
        }

        public event Action<Tile, CellId> MovedToDeck = delegate { };

        public void Init()
        {
//            Rect uvForTile = TileFace.GetUvForBack();
//            Rect uvForTile = TileFace.GetUvForBack();
            Rect uvForTile = TileFace.GetUvForTile(TileValue);
            var uvChanger = this.GetComponent<UvChanger>();
            uvChanger.ChangeUv(uvForTile);
//            this.currentRotation = defaultFlippedRotation;
//            this.ResetRotation();
        }

        private void ResetRotation()
        {
            this.transform.rotation = this.currentRotation;
        }
    }
}