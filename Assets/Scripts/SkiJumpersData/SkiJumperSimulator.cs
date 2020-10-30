using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkiJumperSimulator : MonoBehaviour
{
    private HillData hillData;
    private Transform hillIdealTakeOffPoint;
    
    private WindArea[] windAreas;

    private float maxTakeOffStrength = 8;

    private float minTakeOffStrength = 1;

    private float windBias = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHill(GameObject hill) {
        hillData = hill.GetComponent<Hill>().hillData;
        hillIdealTakeOffPoint = GameObject.FindGameObjectWithTag("IdealTakeOffPoint").GetComponent<Transform>();
        windAreas = hill.GetComponentsInChildren<WindArea>();   
    }

    public void SimulateJump(SkiJumperComputer skiJumperComputer) {
        float takeOffStr = maxTakeOffStrength - Random.Range(0, 3f);
        
        takeOffStr = Mathf.Clamp(takeOffStr, minTakeOffStrength, maxTakeOffStrength);

        float bodyToSkisTilfCoeff = Random.Range(0.75f, 1f);
        float skiJumperTiltCoeff = Random.Range(0.75f, 1f);
        float flightCoeff = bodyToSkisTilfCoeff * skiJumperTiltCoeff;

        // float jumpCoeff = flightCoeff * takeOffStr;
        float jumpCoeff = flightCoeff + takeOffStr;
        jumpCoeff = Mathf.Clamp(jumpCoeff, minTakeOffStrength, maxTakeOffStrength);
        jumpCoeff = jumpCoeff / maxTakeOffStrength; // normalizacja 

        float windForce = 0;

        foreach(WindArea wa in windAreas) {
            windForce += wa.GetWindForce();
        }

        windForce = windForce / windAreas.Length;

        // (currVal - minVal) / (maxVal - minVal)
        float windCoeff = (windForce - (-1)) / (3.5f - (-1)); // normalizacja wiatru dla Range(-1, 3.5f) z WindArea

        // jumpCoeff = jumpCoeff + windCoeff;

        float jumpDistance = hillData.hsPoint * jumpCoeff + (hillData.hsPoint - hillData.kPoint) * windCoeff;

        jumpDistance = Mathf.Round(jumpDistance * 100) / 100;

        float decimalPart = jumpDistance % 1;
        float distanceToAdd = 0;

        if (decimalPart >= 0.75f)
        {
            distanceToAdd = 1;
        }
        else if (decimalPart < 0.75f && decimalPart >= 0.25f)
        {
            distanceToAdd = 0.5f;
        }

        jumpDistance = Mathf.Floor(jumpDistance);
        jumpDistance += distanceToAdd;

        Debug.Log("TakeOffStr: " + takeOffStr);        
        Debug.Log("BodyToSkisTiltCoeff: " + bodyToSkisTilfCoeff);
        Debug.Log("SkiJumperTiltCoeff: " + skiJumperTiltCoeff);
        Debug.Log("Flight coeff: " + flightCoeff);
        Debug.Log("Normalized jump coeff: " + jumpCoeff);
        Debug.Log("Wind coeff: " + windCoeff);

        Debug.Log("HS * jumpCoeff: " + hillData.hsPoint * jumpCoeff);
        Debug.Log("(HS - K) * windCoeff: " + (hillData.hsPoint - hillData.kPoint) * windCoeff);
        
        Debug.Log("Odleglosc: " + jumpDistance);

        skiJumperComputer.jumpResultData.jumpDistance = jumpDistance;
        skiJumperComputer.jumpResultData.judges = GenerateJudgeNotes(jumpDistance);

        float jumpStylePoints = skiJumperComputer.jumpResultData.judges[1].GetJumpStylePoints() + 
                                skiJumperComputer.jumpResultData.judges[2].GetJumpStylePoints() + 
                                skiJumperComputer.jumpResultData.judges[3].GetJumpStylePoints();
       
        float distancePoints = CalculateDistancePoints(hillData, jumpDistance);

        skiJumperComputer.jumpResultData.jumpPoints = jumpStylePoints + distancePoints;

        System.Random rnd = new System.Random();
    
        skiJumperComputer.jumpResultData.judges = skiJumperComputer.jumpResultData.judges.OrderBy(judge => rnd.Next()).ToArray();

        Debug.Log("Wynik komputera: " + skiJumperComputer.jumpResultData.jumpPoints);
    }

    private bool HasLanded(float jumpDistance, float rand) {
        bool landed = false;

        if (jumpDistance <= hillData.kPoint) {
            // prawdopodobienstwo ustania 98% (0-95 telemark, 95-98 both legs)
            if (rand > 0.98f) {
                landed = false;
            }
            else {
                landed = true;
            }
        }
        else if (jumpDistance > hillData.kPoint && jumpDistance < hillData.hsPoint) {
            // prawdopodobienstwo ustania 95% (0-93 telemark, 93-95 both legs)
            if (rand > 0.95f) {
                landed = false;
            }
            else {
                landed = true;
            }
        }
        else {
            // prawdopodobienstwo ustania 90% (0-50 telemark, 50-90 both legs)
            if (rand > 0.9f) {
                landed = false;
            }
            else {
                landed = true;
            }
        }

        return landed;
    }

    private string GenerateLandingType(float jumpDistance, float landingFloat) {
        if (jumpDistance <= hillData.kPoint) {
            // prawdopodobienstwo ustania 98% (0-95 telemark, 95-98 both legs)
            if (landingFloat > 0.95f && landingFloat < 0.98f) {
                return LandingData.BOTH_LEGS;
            }
            else {
                return LandingData.TELEMARK;
            }
        }
        else if (jumpDistance > hillData.kPoint && jumpDistance < hillData.hsPoint) {
            // prawdopodobienstwo ustania 95% (0-93 telemark, 93-95 both legs)
            if (landingFloat > 0.93f && landingFloat < 0.95f) {
                return LandingData.BOTH_LEGS;
            }
            else {
                return LandingData.TELEMARK;
            }
        }
        else {
            // prawdopodobienstwo ustania 90% (0-50 telemark, 50-90 both legs)
            if (landingFloat > 0.5f && landingFloat < 0.9f) {
                return LandingData.BOTH_LEGS;
            }
            else {
                return LandingData.TELEMARK;
            }
        }
    }

    private Judge[] GenerateJudgeNotes(float jumpDistance) {
        Judge[] judges = new Judge[5];
        string landing = "";

        float landingProbability = Random.Range(0f, 1f);

        bool landed = HasLanded(jumpDistance, landingProbability);

        if (landed) {
            landing = GenerateLandingType(jumpDistance, landingProbability);
        }

        for(int index = 0; index < judges.Length; index++) {
            judges[index] = new Judge("POL");
            judges[index].SetComputerPoints(landed, landing);
        }

        // odrzucenie najnizszej i najwyzszej noty
        judges = judges.OrderBy(judge => judge.GetJumpStylePoints()).ToArray();
        judges[0].Reject();
        judges[judges.Length - 1].Reject();        

        return judges;
    }

    private float CalculateDistancePoints(HillData hillData, float jumpDistance) {
        float basePoints = hillData.kPoint > 155 ? 120 : 60;
        float diff = jumpDistance - hillData.kPoint;
        float distancePoints = diff * hillData.pointPerMeter + basePoints;

        return distancePoints;
    }
}
