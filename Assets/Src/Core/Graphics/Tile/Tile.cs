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
        private bool isFlipped;
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

        public void FlyTo(Cell dest, float delay)
        {
            this.FlyTo(dest, delay, true);
        }

        private void FlyTo(Cell dest, float delay, bool cancelOtherAnimations, bool allowRise = true)
        {
            const float Time = 0.7f;
            const float MidPointElevation = 0.2f;

            var target = dest.GetPositionFor(this);
            var targetAngle = this.currentRotation * dest.GetRotationFor(this);
            var source = this.transform.position;
            var midPoint = source + (target - source) / 2;
            midPoint.y += MidPointElevation;

            if (cancelOtherAnimations)
            {
                LeanTween.cancel(this.gameObject);
            }

            if (allowRise && this.transform.position.y < 0.2f)
            {
                LeanTween.moveX(this.gameObject, target.x, Time).setEase(LeanTweenType.easeOutExpo).setDelay(delay);
                LeanTween.moveZ(this.gameObject, target.z, Time).setEase(LeanTweenType.easeOutExpo).setDelay(delay);
                LeanTween.moveY(this.gameObject, this.transform.position.y + 0.2f, Time / 7f).setDelay(delay);
                LeanTween.moveY(this.gameObject, target.y, 6 * Time / 7f).setEase(LeanTweenType.easeOutExpo).setDelay(delay + Time / 7f);
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
        public void InitAsFlipped()
        {
//            Rect uvForTile = TileFace.GetUvForBack();
            Rect uvForTile = TileFace.GetUvForBack();
            var uvChanger = this.GetComponent<UvChanger>();
            uvChanger.ChangeUv(uvForTile, false);
            this.currentRotation = defaultFlippedRotation;
            this.ResetRotation();
            this.isFlipped = true;
        }
        private void ResetRotation()
        {
            this.transform.rotation = this.currentRotation;
        }

    }
}