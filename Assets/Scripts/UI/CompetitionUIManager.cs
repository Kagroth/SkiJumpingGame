using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CompetitionUIManager : UIManager
{
    [SerializeField]
    private Text bestScore;

    [SerializeField]
    private GameObject jumpResultPanel;
    
    [SerializeField]
    private Text distanceValue;

    [SerializeField]
    private Text resultValue;

    [SerializeField]
    private Text[] judgePoints; 

    private bool helpPanelShow;

    [SerializeField]
    private GameObject helpPanel;
    
    [SerializeField]
    public GameObject gameplayPanel;

    private PlayerController playerController;

    private WindMeterUI windMeter;

    public override void Init()
    {
        base.Init();
        HideJumpResultPanel();
        playerController = GameObject.FindObjectOfType<PlayerController>();
        windMeter = GetComponentInChildren<WindMeterUI>();
        helpPanelShow = false;
        playerController.skiJumperStartJumpHandler += HideJumpResultPanel;
        playerController.skiJumperEndJumpHandler += ShowJumpResultPanel;
        playerController.skiJumperEndJumpHandler += ShowBestDistance;
        windMeter.Init();
    }

    public void ShowBestDistance() {
        float bestDistance = playerController.GetBestDistance();

        bestScore.text = "Najlepszy wynik: " + bestDistance.ToString();
    }

    public void ShowJumpResultPanel() {
        PlayerController.JumpResultData jrd = playerController.GetJumpResultData();
        distanceValue.text = jrd.jumpDistance.ToString();
        resultValue.text = jrd.jumpPoints.ToString();
        
        for(int index = 0; index < judgePoints.Length; index++) {
            judgePoints[index].text = jrd.judges[index].GetJumpStylePoints().ToString();
            
            if (jrd.judges[index].IsRejected()) {
                judgePoints[index].color = Color.red;
            }
            else {
                judgePoints[index].color = Color.black;
            }
        }

        jumpResultPanel.SetActive(true);
    }

    public void HideJumpResultPanel() {
        jumpResultPanel.SetActive(false);
    }

    public void ToggleHelpPanel() {
        helpPanelShow = !helpPanelShow;
        helpPanel.SetActive(helpPanelShow);
        gameplayPanel.SetActive(!helpPanelShow);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) {
            ToggleHelpPanel();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
