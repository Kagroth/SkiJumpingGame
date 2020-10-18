using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameplayPanel;
    
    public Text hillInfo;
    public Text lastScore;
    public Text bestScore;
    public Text landingType;

    public GameObject helpPanel;

    public GameObject jumpResultsPanel;
    public Text distanceValue;
    public Text resultValue;
    public Text[] judgePoints;

    private bool helpPanelShow = false;
    private bool jumpResultsPanelShow = false;

    private void Awake() {
        helpPanel.SetActive(helpPanelShow);    
        jumpResultsPanel.SetActive(jumpResultsPanelShow);
    }

    public void ToggleHelpPanel() {
        helpPanelShow = !helpPanelShow;
        helpPanel.SetActive(helpPanelShow);
        gameplayPanel.SetActive(!helpPanelShow);
    }

    public void ToggleJumpResultPanel() {
        jumpResultsPanelShow = !jumpResultsPanelShow;
        jumpResultsPanel.SetActive(jumpResultsPanel);
    }

    public void SetJumpResultData(float jumpDistance, Judge[] judgePointsArr, float result) {
        distanceValue.text = jumpDistance.ToString();
        resultValue.text = result.ToString();

        for(int index = 0; index < judgePoints.Length; index++) {
            judgePoints[index].text = judgePointsArr[index].GetJumpStylePoints().ToString();
            /*
                DODAC PRZYCIEMNIENIE / SKREŚLENIE
                NOT KTORE WYPADAJA
            */
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
