using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Converts RectTransform into world position rect 
/// Link Reference:
/// https://answers.unity.com/questions/1100493/convert-recttransformrect-to-rect-world.html
/// </summary>
static public class RectTransformExt{
    static public Rect GetWorldRect(RectTransform rt, Vector2 scale) {
        // Convert the rectangle to world corners and grab the top left
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 topLeft = corners[0];

        // Rescale the size appropriately based on the current Canvas scale
        Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

        return new Rect(topLeft, scaledSize);
    }
}
