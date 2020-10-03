using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingData
{
    public const string TELEMARK = "Telemark";
    public const string BOTH_LEGS = "BothLegs";

    private string currentLanding; 
    private float distanceToLandingSlope;
    private Vector3 landingPoint;

    public LandingData(string currLanding) {
        currentLanding = currLanding;
    }

    public string GetLandingType() {
        return currentLanding;
    } 

    public float GetDistance() {
        return distanceToLandingSlope;
    }

    public Vector3 GetLandingPoint() {
        return landingPoint;
    }

    public void SetDistanceToLandingSlope(float distance) {
        distanceToLandingSlope = distance;
    }

    public void SetLandingPoint(Vector3 landingP) {
        landingPoint = landingP;
    }
}
