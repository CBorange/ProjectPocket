using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtil
{
    public static float GetAngleBetweenVecExceptY(Vector3 VecA, Vector3 VecB)
    {
        float width = VecB.x - VecA.x;
        float height = VecB.z - VecA.z;
        return Mathf.Atan2(height, width) * Mathf.Rad2Deg;
    }
}
