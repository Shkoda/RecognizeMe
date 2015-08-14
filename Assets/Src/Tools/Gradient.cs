// This code is writeen by an unknown authors

#region imports

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#endregion

[AddComponentMenu("UI/Effects/Gradient")]
public class Gradient : BaseVertexEffect
{
    public enum Direction
    {
        Vertical,
        Horizontal
    }

    [SerializeField] private Color32 bottomColor = Color.black;
    [SerializeField] private Direction direction = Direction.Vertical;
    [SerializeField] private Color32[] horizontalColors = {Color.white, Color.black};
    [SerializeField] private Color32 topColor = Color.white;

    public override void ModifyVertices(List<UIVertex> vertexList)
    {
        if (!IsActive() || (direction == Direction.Horizontal && horizontalColors.Length < 2))
        {
            return;
        }

        if (direction == Direction.Horizontal)
        {
            var gr = gameObject.GetComponent<Graphic>();
            CorrectA(gr, ref horizontalColors[0]);
            CorrectA(gr, ref horizontalColors[1]);
            ModifyVerticesHorizontal(vertexList);
        }
        else
        {
            var gr = gameObject.GetComponent<Graphic>();
            CorrectA(gr, ref topColor);
            CorrectA(gr, ref bottomColor);
            ModifyVerticesVertical(vertexList);
        }
    }

    private void CorrectA(Graphic gr, ref Color32 color)
    {
        color = new Color32(color.r, color.g, color.b, (byte) (gr.color.a*255));
    }

    private void ModifyVerticesVertical(List<UIVertex> vertexList)
    {
        var count = vertexList.Count;

        if (count == 0)
        {
            return;
        }

        var topY = vertexList[0].position.y;
        var bottomY = vertexList[0].position.y;

        for (var i = 1; i < count; i++)
        {
            var y = vertexList[i].position.y;
            if (y > topY)
            {
                topY = y;
            }
            else if (y < bottomY)
            {
                bottomY = y;
            }
        }

        var textHeight = topY - bottomY;

        for (var i = 0; i < count; i++)
        {
            var uiVertex = vertexList[i];

            var lerp = (uiVertex.position.y - bottomY)/textHeight;
            // float y = Mathf.Asin((lerpArg - 0.5f) * 2) / 3.1416f + 0.5f;
            //float lerp = Mathf.Clamp01(y);
            uiVertex.color = Color32.Lerp(bottomColor, topColor, lerp);

            vertexList[i] = uiVertex;
        }
    }

    private void ModifyVerticesHorizontal(List<UIVertex> vertexList)
    {
        var count = vertexList.Count;

        if (count == 0)
        {
            return;
        }

        var leftX = vertexList[0].position.x;
        var rightX = vertexList[0].position.x;

        for (var i = 1; i < count; i++)
        {
            var x = vertexList[i].position.x;
            if (x > rightX)
            {
                rightX = x;
            }
            else if (x < leftX)
            {
                leftX = x;
            }
        }

        var textWidth = rightX - leftX;
        var numColors = horizontalColors.Length;
        var colorSegmentWidth = textWidth/(numColors - 1);

        for (var i = 0; i < count; i++)
        {
            var uiVertex = vertexList[i];

            var localPositionX = uiVertex.position.x - leftX;

            var fromColorIndex = Mathf.Clamp(Mathf.FloorToInt(localPositionX/colorSegmentWidth), 0, numColors - 1);
            var toColorIndex = Mathf.Clamp(fromColorIndex + 1, 0, numColors - 1);

            uiVertex.color = Color32.Lerp(horizontalColors[fromColorIndex], horizontalColors[toColorIndex],
                (localPositionX%colorSegmentWidth)/colorSegmentWidth);
            vertexList[i] = uiVertex;
        }
    }
}