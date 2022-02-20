using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkiJumperSimulator
{
    private HillData hillData;
    private Transform hillIdealTakeOffPoint;
    
    private WindArea[] windAreas;

    private float maxTakeOffStrength = 8;

    private float minTakeOffStrength = 1;

    private float windBias = 0.25f;

    public void SetHill(GameObject hill) {
        hillData = hill.GetComponent<Hill>().hillData;
        hillIdealTakeOffPoint = GameObject.FindGameObjectWithTag("IdealTakeOffPoint").GetComponent<Transform>();
        windAreas = hill.GetComponentsInChildren<WindArea>();   
    }
    
    public JumpResult SimulateJump(SkiJumperStats sjStats) {
        // new min, new max etc
        int oldMin = 0;
        int oldMax = 100;
        float newMin = -1f;
        float newMax = 1f;
        float oldRange = oldMax - oldMin;
        float newRange = newMax - newMin;

        float skill = sjStats.GetSkill();

        // Base distance calc
        float skillNormalized = ((skill - oldMin) * newRange) / oldRange + newMin;
        Debug.Log("Normalized skill: " + skillNormalized);
        float mean = this.hillData.kPoint + skillNormalized * (this.hillData.hsPoint - this.hillData.kPoint);
        float std = 3;
        float baseDistance = RandomNormal.NextFloat(mean, std);
        Debug.Log("Base distance = " + baseDistance);

        // Take off factor calc
        float skillNorm = skill / 100f;
        float shiftFromIdealTakeOffPoint = RandomNormal.NextFloat(1 - skillNorm, 1);
        shiftFromIdealTakeOffPoint = Mathf.Abs(shiftFromIdealTakeOffPoint);
        float  takeOffCoeff = (this.hillData.hsPoint - this.hillData.kPoint) * 0.2f;
        float takeOffFactor = (1 - shiftFromIdealTakeOffPoint) * takeOffCoeff; // hs - k * 0.2 is TAKE_OFF_COEFF
        takeOffFactor = Mathf.Clamp(takeOffFactor, -takeOffCoeff, takeOffCoeff);
        Debug.Log("Take off shift: " + takeOffFactor);

        // Wind factor calc
        float windForce = 0;

        foreach(WindArea wa in windAreas) {
            windForce += wa.GetWindForce();
        }

        windForce = windForce / windAreas.Length;

        float wind = RandomNormal.NextFloat(windForce, 1);
        float windCoeff = wind / 4f;
        float windShift = windCoeff * (this.hillData.hsPoint - this.hillData.kPoint) / 4;
        float finalDistance = baseDistance + takeOffFactor + windShift;
        finalDistance = this.RoundJumpDistance(finalDistance);
        Debug.Log("Final distance: " + finalDistance);
     
        JumpResult jumpResult = new JumpResult();
        jumpResult.jumpDistance = finalDistance;
        jumpResult.judges = GenerateJudgeNotes(finalDistance);

        float jumpStylePoints = jumpResult.judges[1].GetJumpStylePoints() + 
                                jumpResult.judges[2].GetJumpStylePoints() + 
                                jumpResult.judges[3].GetJumpStylePoints();
        
        float distancePoints = CalculateDistancePoints(hillData, finalDistance);
        jumpResult.jumpPoints = jumpStylePoints + distancePoints;

        System.Random rnd = new System.Random();
    
        jumpResult.judges = jumpResult.judges.OrderBy(judge => rnd.Next()).ToArray();

        return jumpResult;
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

    /* 
        Round jump distance to halves.
        For example:
        from 128.246324 => 128.0
        from 219.842100 => 220.0
        from 96.6490912 => 96.5
    */
    private float RoundJumpDistance(float jumpDistance) {
        jumpDistance = Mathf.Round(jumpDistance * 100) / 100;
        float decimalPart = jumpDistance % 1;
        float distanceToAdd = 0;

        if (decimalPart >= 0.75f) {
            distanceToAdd = 1;
        }
        else if (decimalPart < 0.75f && decimalPart >= 0.25f) {
            distanceToAdd = 0.5f;
        }

        jumpDistance = Mathf.Floor(jumpDistance);
        jumpDistance += distanceToAdd;

        return jumpDistance;
    }
}
