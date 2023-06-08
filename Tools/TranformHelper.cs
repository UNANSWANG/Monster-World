using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TranformHelper
{
    private const float dotThreshold = 0.5f;
    public static bool IsFacingTarget(this Transform transform,Transform target)
    {
        var vectorTotarget = target.position - transform.position;
        vectorTotarget.Normalize();

        float dot = Vector3.Dot(transform.forward,vectorTotarget);
        return dot >= dotThreshold;
    }
}
