using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector2 MoveLimit = new Vector2(6.3f, 3.4f);

    public static Vector3 ClampPosition(Vector3 position)
    {
        return new Vector3(
            Mathf.Clamp(position.x, -MoveLimit.x, MoveLimit.x),
            Mathf.Clamp(position.y, -MoveLimit.y, MoveLimit.y),
            0);
    }

    public static Vector3 GetDirection(float angle)
    {
        return new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
    }

    public static float GetAngle(Vector2 from, Vector2 to)
    {
        var dx = to.x - from.x;
        var dy = to.y - from.y;
        var radian = Mathf.Atan2(dy, dx);
        return radian * Mathf.Rad2Deg;
    }
}
