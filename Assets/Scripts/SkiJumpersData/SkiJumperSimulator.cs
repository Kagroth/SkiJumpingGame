using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // skiJumperComputer.jumpResultData.jumpDistance = jumpDistance;
    }
}
