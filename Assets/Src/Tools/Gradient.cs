// This code is writeen by an unknown authors
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient")]
public class Gradient : BaseVertexEffect
{
    [SerializeField]
    private Direction direction = Direction.Vertical;
    [SerializeField]
    private Color32 topColor = Color.white;
    [SerializeField]
    private Color32 bottomColor = Color.black;
    [SerializeField]
    private Color32[] horizontalColors = { Color.white, Color.black };

    public enum Direction
    {
        Vertical,
        Horizontal
    }

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
        color = new Color32(color.r, color.g, color.b, (byte)(gr.color.a * 255));
    }

    void ModifyVerticesVertical(List<UIVertex> vertexList)
    {
        int count = vertexList.Count;

        if (count == 0)
        {
            return;
        }

        float topY = vertexList[0].position.y;
        float bottomY = vertexList[0].position.y;

        for (int i = 1; i < count; i++)
        {
            float y = vertexList[i].position.y;
            if (y > topY)
            {
                topY = y;
            }
            else if (y < bottomY)
            {
                bottomY = y;
            }
        }

        float textHeight = topY - bottomY;

        for (int i = 0; i < count; i++)
        {
            UIVertex uiVertex = vertexList[i];

            float lerp = (uiVertex.position.y - bottomY) / textHeight;
            // float y = Mathf.Asin((lerpArg - 0.5f) * 2) / 3.1416f + 0.5f;
            //float lerp = Mathf.Clamp01(y);
            uiVertex.color = Color32.Lerp(bottomColor, topColor, lerp);

            vertexList[i] = uiVertex;
        }
    }

    void ModifyVerticesHorizontal(List<UIVertex> vertexList)
    {
        int count = vertexList.Count;

        if (count == 0)
        {
            return;
        }

        float leftX = vertexList[0].position.x;
        float rightX = vertexList[0].position.x;

        for (int i = 1; i < count; i++)
        {
            float x = vertexList[i].position.x;
            if (x > rightX)
            {
                rightX = x;
            }
            else if (x < leftX)
            {
                leftX = x;
            }
        }

        float textWidth = rightX - leftX;
        int numColors = horizontalColors.Length;
        float colorSegmentWidth = textWidth / (numColors - 1);

        for (int i = 0; i < count; i++)
        {
            UIVertex uiVertex = vertexList[i];

            float localPositionX = uiVertex.position.x - leftX;

            int fromColorIndex = Mathf.Clamp(Mathf.FloorToInt(localPositionX / colorSegmentWidth), 0, numColors - 1);
            int toColorIndex = Mathf.Clamp(fromColorIndex + 1, 0, numColors - 1);

            uiVertex.color = Color32.Lerp(horizontalColors[fromColorIndex], horizontalColors[toColorIndex], (localPositionX % colorSegmentWidth) / colorSegmentWidth);
            vertexList[i] = uiVertex;
        }
    }
}



