using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompetitionUIManager : UIManager
{
    [SerializeField]
    private GameObject jumpResultPanel;
    
    [SerializeField]
    private Text distanceValue;

    [SerializeField]
    private Text resultValue;

    [SerializeField]
    private Text[] judgePoints; 

    private PlayerController playerController;

    public override void Init()
    {
        base.Init();
        playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.skiJumperStartJumpHandler += HideJumpResultPanel;
        playerController.skiJumperEndJumpHandler += ShowJumpResultPanel;
    }

    public void ShowJumpResultPanel() {
        PlayerController.JumpResultData jrd = playerController.GetJumpResultData();
        distanceValue.text = jrd.jumpDistance.ToString();
        resultValue.text = jrd.jumpPoints.ToString();
        
        for(int index = 0; index < judgePoints.Length; index++) {
            judgePoints[index].text = jrd.judges[index].GetJumpStylePoints().ToString();
            /*
                DODAC PRZYCIEMNIENIE / SKREŚLENIE
                NOT KTORE WYPADAJA
            */
        }

        jumpResultPanel.SetActive(true);
    }

    public void HideJumpResultPanel() {
        jumpResultPanel.SetActive(false);
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
