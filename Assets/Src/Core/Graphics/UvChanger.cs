namespace Shkoda.RecognizeMe.Core.Graphics
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine;

    public class UvChanger : MonoBehaviour
    {
        /// <summary>
        ///     Change uv from default (0,0,1,1) space to the one specified in uv parameter for every vertex in mesh
        /// </summary>
        public void ChangeUv(Rect uv)

        {
            var material = gameObject.GetComponent<Renderer>().material;

            material.SetTextureOffset("_MainTex", new Vector2(uv.xMin, 0.8f-uv.yMin));
            material.SetTextureScale("_MainTex", new Vector2(0.17f, 0.2f));
        }
    }
}