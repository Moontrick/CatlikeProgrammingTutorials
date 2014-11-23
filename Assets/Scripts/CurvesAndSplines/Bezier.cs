using UnityEngine;

public static class Bezier 
{

    
    // Quadratic Bezier formula
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        float oneMinusTSq = oneMinusT * oneMinusT;
        return oneMinusTSq * p0 + 2f * oneMinusT * t * p1 + t * t * p2;

        // Same as this:
        // Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t)
        // Faster to just do the quadratic formula instead of making 3 calls to Vector3.Lerp
    }


}
