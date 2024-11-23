using UnityEngine;

public static class RectTransformExtensions
{

    public static bool Overlap(this RectTransform a, RectTransform b)
    {
        // Get the world corners of both the item and the viewport.
        Vector3[] viewportCorners = new Vector3[4];
        a.GetWorldCorners(viewportCorners);

        return !(viewportCorners[0].y > b.position.y 
                || viewportCorners[2].y < b.position.y);
    }
}