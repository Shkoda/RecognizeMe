#region imports

using System.Collections;
using Assets.Src.Core.Game.Cell;
using JetBrains.Annotations;
using UnityEngine;

#endregion

namespace Shkoda.RecognizeMe.Core.Graphics
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private GameObject animationPrefab;
//        private BoxCollider boxCollider;

        public CellId CellId;
        private Vector3 defaultColliderBorderSize;
        private Vector3 position;
        private Tile tile;
//        public Guid Id { get; private set; }

        public Tile Tile { get; set; }

        public void PushAndMoveInstantly(Tile tile)
        {
            PushTile(tile);
            tile.MoveInstantlyTo(GetPositionFor(tile), GetRotationFor(tile));
        }

        public Tile PopTile()
        {
            var popped = Tile;
            popped.ContainingCell = null;
            Tile = null;
            UpdateColliderSize();
            return popped;
        }

        public void PushTile(Tile tile)
        {
            this.tile = tile;
            tile.ContainingCell = this;
            UpdateColliderSize();
        }

        public Vector3 GetPositionFor(Tile tile)
        {
            if (this.tile != tile)
            {
                Debug.LogError(string.Format("There's no such tile ({0}) in this cell ({1}).", tile, this));
                return Vector3.zero;
            }

            return position;
        }

        public Quaternion GetRotationFor(Tile tile)
        {
            if (this.tile != tile)
            {
                Debug.LogError(string.Format("There's no such tile ({0}) in this cell ({1}).", tile, this));
                return Quaternion.identity;
            }

            // Cell has different default angle, that is rotated -90 degress by x axis
            return transform.rotation; // * Quaternion.Euler(90, 0, 0);
        }

        public void RemoveTile(Tile tile)
        {
            tile.ContainingCell = null;
            this.tile = null;
        }

        private IEnumerator DoAnimate(float delay)
        {
            if (!delay.Equals(0))
            {
                yield return new WaitForSeconds(delay);
            }

            if (animationPrefab == null)
            {
                Debug.LogError("Foundation animation activated but prefab is null");
                yield break;
            }

            yield return new WaitForSeconds(.2f);

            var anim = Instantiate(animationPrefab);
            anim.transform.SetParent(gameObject.transform);
            anim.SetActiveRecursively(false);
            anim.transform.localPosition = new Vector3(0, 0.005f, 0);
            anim.transform.localRotation = Quaternion.identity;
            anim.transform.localScale = Vector3.one;
            anim.SetActiveRecursively(true);

            // var script = anim.transform.GetChild(0).GetComponent<NcCurveAnimation>();
            // script.ResetAnimation();
            yield return new WaitForSeconds(3);

            Destroy(anim);
        }

        [UsedImplicitly]
        private void Awake()
        {
            position = transform.position;
//            boxCollider = GetComponent<BoxCollider>();
//            defaultColliderBorderSize = boxCollider.size - new Vector3(1, 1, 1);
        }

        private void UpdateColliderSize()
        {
//            if (!boxCollider)
//            {
//                Debug.LogWarning("boxCollider was destroyed");
//                return;
//            }
//
//
//            boxCollider.size = new Vector3(1, 1, 1) + defaultColliderBorderSize;
//            boxCollider.center = Vector3.zero;
        }

        public bool HasId(CellId id)
        {
            return CellId.Equals(id);
        }

        public override string ToString()
        {
            var content = Tile == null ? "EMPTY" : Tile.TileValue.ToString();
            return string.Format("{0} [{1}]", CellId, content);
        }
    }
}