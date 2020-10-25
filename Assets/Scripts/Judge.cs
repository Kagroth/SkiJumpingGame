using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Judge
{
    public string country;
    private float jumpStylePoints;
    private bool rejected;

    public Judge(string country)
    {
        this.country = country;
        rejected = false;
        jumpStylePoints = 0;
    }

    public float GetJumpStylePoints() {
        return jumpStylePoints;
    }

    public void Reject() {
        rejected = true;
    }

    public bool IsRejected() {
        return rejected;
    }

    /*
        noty za styl
        lot - 5
        lądowanie - 5
        upadek - 7
        nieprawidlowa pozycja na odjezdzie - 3 -> moze byc zalezny od momentu podejscia do lądowania
    */    
    public void CalculateJumpStylePoints(bool landed, string landingType, float flightTilt)
    {
        float landingPoints = 0;
        float flightPoints = GetFlightPoints(flightTilt);        

        if (landed) {
            jumpStylePoints += 7;
            landingPoints = GetLandingPoints(landingType);
        }

        jumpStylePoints += landingPoints + flightPoints;
        jumpStylePoints += 3; // tymczasowo, patrz komentarz wyżej
        
        RandomizePoints();
    }

    private void RandomizePoints() {
        float random = Random.Range(0f, 1f);
        Debug.Log("RANDOM: " + random);
        float chunk = 0;

        if (random >= 0.67f) {
            chunk = 0.5f;
        }
        else if (random >= 0.33f && random < 0.67f) {
            chunk = 0;
        }
        else {
            chunk = -0.5f;
        }

        if ((jumpStylePoints == 20 && chunk > 0) || (jumpStylePoints == 0 && chunk < 0)) {
            chunk = 0;
        }

        jumpStylePoints += chunk;
    }

    private float GetLandingPoints(string landingType)
    {
        float landingPoints = 0;

        if (landingType == LandingData.TELEMARK)
        {
            landingPoints = 5;
        }
        else if (landingType == LandingData.BOTH_LEGS)
        {
            landingPoints = 3;
        }

        return landingPoints;
    }

    private float GetFlightPoints(float flightTiltChange)
    {
        float flightPoints = 0;

        if (flightTiltChange < 15)
        {
            flightPoints = 5;
        }
        else if (flightTiltChange >= 15 && flightTiltChange < 30)
        {
            flightPoints = 4.5f;
        }
        else if (flightTiltChange >= 30 && flightTiltChange < 60)
        {
            flightPoints = 4;
        }
        else if (flightTiltChange >= 60 && flightTiltChange < 90)
        {
            flightPoints = 3.5f;
        }
        else if (flightTiltChange >= 90 && flightTiltChange < 120)
        {
            flightPoints = 3;
        }
        else if (flightTiltChange >= 120 && flightTiltChange < 150)
        {
            flightPoints = 2.5f;
        }
        else if (flightTiltChange >= 150 && flightTiltChange < 180)
        {
            flightPoints = 2;
        }
        else if (flightTiltChange >= 180 && flightTiltChange < 210)
        {
            flightPoints = 1.5f;
        }
        else if (flightTiltChange >= 210 && flightTiltChange < 240)
        {
            flightPoints = 1;
        }
        else if (flightTiltChange >= 240 && flightTiltChange < 360)
        {
            flightPoints = 0.5f;
        }
        else
        {
            flightPoints = 0;
        }

        return flightPoints;
    }
}
