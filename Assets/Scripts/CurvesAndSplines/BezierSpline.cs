using UnityEngine;
using System;

public class BezierSpline : MonoBehaviour 
{


    public Vector3[] points;


    // Returns the total number of curves in the spline
    public int CurveCount
    {
        get
        {
            return (points.Length - 1) / 3;
        }
    }


    public void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
    }


    public Vector3 GetPoint(float t)
    {
        int i;

        // Default to the last curve if t is greater or equal to 1
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= 1;
            i *= 3;
        }

        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
    }


    public Vector3 GetVelocity(float t)
    {
        int i;

        // Default to last curve if t is greater or equal to 1
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }

        return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position;
    }


    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }


    public void AddCurve()
    {
        Vector3 point = points[points.Length - 1];   // First point of new curve is the same as the last point of the previous
        Array.Resize(ref points, points.Length + 3); // Increase the size of our points array by the amount of new points needed

        // Offset the x value of each new point by 1 so they don't sit on top of each other
        point.x += 1f;
        points[points.Length - 3] = point;
        point.x += 1f;
        points[points.Length - 2] = point;
        point.x += 1f;
        points[points.Length - 1] = point;
    }


}
