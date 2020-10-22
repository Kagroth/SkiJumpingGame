using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WindMeterUI : MonoBehaviour
{
    public Image positiveWind;
    public Image negativeWind;

    private AreaEffector2D[] windAreas;

    private int maxWindForce = 4;

    private void Awake() {
        
    }

    void Start()
    {
        windAreas = GameObject.FindGameObjectsWithTag("WindArea")
                              .Select(windAreaGameObject => {
                                  return windAreaGameObject.GetComponent<AreaEffector2D>();
                                })
                              .ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        float windForce = 0; 

        foreach (AreaEffector2D ae2D in windAreas) {
            if (ae2D.forceAngle == 90) {
                windForce += ae2D.forceMagnitude;
            }
            else {
                windForce -= ae2D.forceMagnitude;
            }
        }

        windForce = (windForce - 1) / windAreas.Length;

        if (windForce > 0) {
            positiveWind.fillAmount = Mathf.Lerp(positiveWind.fillAmount, Mathf.Abs(windForce / maxWindForce), Time.deltaTime);
            negativeWind.fillAmount = 0;
        }
        else if (windForce == 0) {
            positiveWind.fillAmount = 0;
            negativeWind.fillAmount = 0;
        }
        else {
            positiveWind.fillAmount = 0;
            negativeWind.fillAmount = Mathf.Lerp(negativeWind.fillAmount, Mathf.Abs(windForce / maxWindForce), Time.deltaTime);
        }

        // Debug.Log("WindForce: " + windForce);
    }
}
