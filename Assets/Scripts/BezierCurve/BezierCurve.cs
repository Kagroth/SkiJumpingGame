using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierCurve : MonoBehaviour
{
    [SerializeField]
    Transform[] controlPoints;
    
    [SerializeField]
    
    Vector3[] bezierPoints;

    [SerializeField]
    
    float magnitude;

    public Transform[] GetControlPoints() {
        if (controlPoints == null) {
            Debug.Log("Tablica controlPoints nie jest zainicjalizowana");
            return null;
        }

        return controlPoints;
    }

    public void SetControlPoints(Transform[] newControlPoints) {
        controlPoints = newControlPoints;
    }

    public Vector3[] GetBezierPoints() {
        return bezierPoints;
    }

    public void SetBezierPoints(Vector3[] newBezierPoints) {
        bezierPoints = newBezierPoints;
    }

    public float GetMagnitude() {
        return magnitude;
    }

    public void SetMagnitude(float newMagnitude) {
        magnitude = newMagnitude;
    }
}
