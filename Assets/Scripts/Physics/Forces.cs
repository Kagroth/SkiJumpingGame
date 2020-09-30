using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forces
{
    public const float liftCoefficient = 0.03f;
    public const float dragCoefficient = 0.028f;
    public const float airDensity = 1.2f;

    public static Vector3 Lift(Vector2 velocity, float area) {
        Vector3 liftForce = velocity.magnitude * velocity.magnitude * 0.5f * liftCoefficient * airDensity * Vector2.Perpendicular(velocity.normalized) * area;

        return liftForce;
    }

    public static Vector3 Drag(Vector2 velocity, float area) {
        Vector3 dragForce = velocity.magnitude * velocity.magnitude * dragCoefficient * airDensity * 0.5f * -velocity.normalized * area;

        return dragForce;
    }
}
