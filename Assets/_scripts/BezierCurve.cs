using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Vector3 point1 = Vector3.zero;
    public Vector3 point2 = Vector3.zero;
    public Vector3 point3 = Vector3.zero;
    public LineRenderer lineRenderer;
    public int vertexCount = 12;

    void LateUpdate()
    {
        var pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(point1, point2, ratio);
            var tangentLineVertex2 = Vector3.Lerp(point2, point3, ratio);
            var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierpoint);
        }
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }
}
