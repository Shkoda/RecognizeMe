namespace Shkoda.RecognizeMe.Core.Graphics
{
    using System;

    using JetBrains.Annotations;

    using UnityEngine;

    public class UvChanger : MonoBehaviour
    {
        public static Rect FrontUvSpace = new Rect(0f, 0f, 0.495f, 0.722f);

        public static Rect BackUvSpace = new Rect(0.505f, 0f, 0.5f, 0.722f);
        
        /// <summary>
        /// 0 - not known, 1 - point is lying in front uv space, 2 - point is lying in back uv space
        /// </summary>
        private byte[] uvSpaces;
                      
        private Vector2[] defaultUvs;

        private Vector2[] defaultFrontUvs;

        private Vector2[] defaultBackUvs;

        private MeshFilter meshFilter;

        private Vector2[] currentUvs;

        private Vector2[] tempUvs;

        /// <summary>
        ///     Change uv from default (0,0,1,1) space to the one specified in uv parameter for every vertex in mesh
        /// </summary>
        public void ChangeUv(Rect uv, bool front)
        {
            var uvXMin = uv.xMin;
            var uvXMax = uv.xMax;
            var uvYMin = uv.yMin;
            var uvYMax = uv.yMax;

            Rect currentSpace = front ? FrontUvSpace : BackUvSpace;

            float spaceXMin = currentSpace.xMin;
            float spaceYMin = currentSpace.yMin;
            float spaceWidth = currentSpace.width;
            float spaceHeight = currentSpace.height;

            if (front)
            {
                for (int i = 0; i < this.defaultUvs.Length; i++)
                {
                    // check if current point lies in front uv space
                    if (this.uvSpaces[i] == 1)
                    {
                        var resultUv = this.TranslateUv(
                            this.defaultUvs[i], 
                            uvXMin, 
                            uvXMax, 
                            uvYMin, 
                            uvYMax, 
                            spaceXMin, 
                            spaceYMin, 
                            spaceWidth, 
                            spaceHeight);
                        tempUvs[i] = resultUv;
                    }
                    else
                    {
                        tempUvs[i] = currentUvs[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.defaultUvs.Length; i++)
                {
                    // check if current point lies in front uv space
                    if (this.uvSpaces[i] == 2)
                    {
                        var resultUv = this.TranslateUv(
                            this.defaultUvs[i],
                            uvXMin,
                            uvXMax,
                            uvYMin,
                            uvYMax,
                            spaceXMin,
                            spaceYMin,
                            spaceWidth,
                            spaceHeight);
                        tempUvs[i] = resultUv;
                    }
                    else
                    {
                        tempUvs[i] = currentUvs[i];
                    }
                }
            }

            Array.Copy(this.tempUvs, this.currentUvs, this.defaultUvs.Length);
            this.meshFilter.mesh.uv = currentUvs;
        }

        private Vector2 TranslateUv(
            Vector2 startUv, 
            float uvXMin, 
            float uvXMax, 
            float uvYMin, 
            float uvYMax, 
            float spaceUvXMin, 
            float spaceUvYMin, 
            float spaceUvWidth, 
            float spaceUvHeight)
        {
            var relativeToFaceUv = new Vector2
                                       {
                                           x = (startUv.x - spaceUvXMin) / spaceUvWidth, 
                                           y = (startUv.y - spaceUvYMin) / spaceUvHeight
                                       };
            var resultUv = new Vector2
                               {
                                   x = Mathf.Lerp(uvXMin, uvXMax, relativeToFaceUv.x), 
                                   y = 1 - Mathf.Lerp(uvYMin, uvYMax, 1 - relativeToFaceUv.y), 
                               };
            return resultUv;
        }

        // Use this for initialization
        [UsedImplicitly]
        private void Awake()
        {
            this.meshFilter = this.GetComponent<MeshFilter>();
            this.defaultUvs = new Vector2[this.meshFilter.sharedMesh.uv.Length];
            this.uvSpaces = new byte[this.defaultUvs.Length];
            this.currentUvs = new Vector2[this.defaultUvs.Length];
            this.tempUvs = new Vector2[this.defaultUvs.Length];

            Array.Copy(this.defaultUvs, this.currentUvs, this.defaultUvs.Length);
            Array.Copy(this.meshFilter.sharedMesh.uv, this.defaultUvs, this.defaultUvs.Length);

            // Precalculate all this shit to a table to reduce time for changin uv's later in game
            for (int i = 0; i < this.defaultUvs.Length; i++)
            {
                if (FrontUvSpace.Contains(this.defaultUvs[i]))
                {
                    this.uvSpaces[i] = 1;
                }
                else if (BackUvSpace.Contains(this.defaultUvs[i]))
                {
                    this.uvSpaces[i] = 2;
                }
                else
                {
                    this.uvSpaces[i] = 0;
                }
            }
        }
    }
}