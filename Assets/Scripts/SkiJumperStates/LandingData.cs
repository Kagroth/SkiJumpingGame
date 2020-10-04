using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingData
{
    public const string TELEMARK = "Telemark";
    public const string BOTH_LEGS = "BothLegs";

    public const float TELEMARK_MIN_HEIGTH = 3;
    public const float BOTH_LEGS_MIN_HEIGTH = 2;

    public const float TELEMARK_MIN_ANGLE = 0;
    public const float BOTH_LEGS_MIN_ANGLE = 0;

    private string currentLanding; 
    private float distanceToLandingSlope;
    private Vector3 landingPoint;

    public LandingData() {
    
    }

    public string GetLandingType() {
        return currentLanding;
    } 

    public void SetLandingType(string currLanding) {
        currentLanding = currLanding;
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
