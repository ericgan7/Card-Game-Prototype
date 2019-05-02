using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
   public Vector3[] QuadraticCurve(Vector3 start, Vector3 middle, Vector3 end, int steps)
    {
        Vector3[] curve = new Vector3[steps];
        float t = 0;
        for (int i = 0; i < steps; ++i)
        {
            t = (float)i / steps;
            Vector3 point = Mathf.Pow((1 - t), 2) * start + 2 * (1 - t) * t * middle + Mathf.Pow(t, 2) * end;
            curve[i] = point;
        }
        return curve;
    }
}
