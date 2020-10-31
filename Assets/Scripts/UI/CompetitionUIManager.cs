using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
public class CompetitionUIManager : UIManager
{
    [System.Serializable]
    public struct View {
        public string name;
        public GameObject viewPanel;

        public void SwitchView(View viewToShow) {
            viewPanel.SetActive(false);
            viewToShow.viewPanel.SetActive(true);
        }

        public void Show() {
            viewPanel.SetActive(true);
        }
    } 

    public View[] views;

    private View currentView;

    private SkiJumperSimulator skiJumperSimulator;

    // Competition View UI
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
    private WindMeterUI windMeter;

    // ************************************

    // Competition Info UI

    public Text hillName;
    public GameObject resultsScrollPanelContent;

    public GameObject competitionScrollPanelRecordPrefab;
    private List<GameObject> competitionScrollPanelRecords;

    private List<CompetitionResult> skiJumpersResults;

    private PlayerController playerController;
    // *************************************************
    
    private bool playerNextMove;
    private int currentJumper;
    private int currentSerie;

    private string competitionState;
    private const string COMPUTER_NEXT = "COMPUTER_NEXT";
    private const string PLAYER_NEXT = "PLAYER_NEXT";
    private const string END_NEXT = "END_NEXT";

    public override void Init()
    {
        base.Init();
        skiJumperSimulator = new SkiJumperSimulator();
        skiJumperSimulator.SetHill(hill);
        competitionScrollPanelRecords = new List<GameObject>();
        skiJumpersResults = new List<CompetitionResult>();

        currentView = views.Where(view => view.name.Equals("CompetitionInfo")).First();
        InputManager.SetInputMode(InputManager.COMPETITION_UI);
        
        HideJumpResultPanel();
        playerController = GameObject.FindObjectOfType<PlayerController>();

        helpPanelShow = false;
        playerController.skiJumperStartJumpHandler += HideJumpResultPanel;
        playerController.skiJumperJumpFinishedHandler += ShowJumpResultPanel;
        playerController.skiJumperEndJumpHandler += SwitchToInfo;
        
        RenderSkiJumpers();

        currentJumper = 0;
        currentSerie = 1;        
        
        NextState();
        currentView.Show();
    }

    public void ShowJumpResultPanel() {
        JumpResult jrd = playerController.GetJumpResultData();
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

    public void SwitchToCompetition() {
        View competitionView = views.Where(view => view.name.Equals("CompetitionView")).First();
        currentView.SwitchView(competitionView);
        currentView = competitionView;
        windMeter = GetComponentInChildren<WindMeterUI>();
        windMeter.Init();
        InputManager.SetInputMode(InputManager.SKI_JUMPER);
        playerController.ResetSkiJumper();
    }

    public void SwitchToInfo() {
        View competitionInfo = views.Where(view => view.name.Equals("CompetitionInfo")).First();
        currentView.SwitchView(competitionInfo);
        currentView = competitionInfo;
        InputManager.SetInputMode(InputManager.COMPETITION_UI);
        RenderPlayerResult();
        NextState();
    }

    public void RunSimulation() {
        if (competitionState == PLAYER_NEXT) {
            SwitchToCompetition();
            return;
        }
        else if (competitionState == COMPUTER_NEXT) {
            StartCoroutine(RunCompetition());
        }
        else if (competitionState == END_NEXT) {
            // zakoncz konkurs
            SceneManager.LoadScene("MainMenu");
            return;
        }
    }

    public IEnumerator RunCompetition() {
        while(true) {
            if (competitionState == PLAYER_NEXT || competitionState == END_NEXT) {
                yield break;
            }

            // symulacja skoku komputera
            JumpResult computerResult = skiJumperSimulator.SimulateJump();
            skiJumpersResults[currentJumper].SetJumpResult(computerResult, currentSerie);
            skiJumpersResults.Sort(CompetitionResult.Compare);

            Debug.Log("Sorted list: ");

            foreach (CompetitionResult cr in skiJumpersResults) {
                Debug.Log(cr.skiJumper.skiJumperName + " - " + cr.jumpResults[0].jumpDistance);
            }

            // CompetitionResultRecord crr = competitionScrollPanelRecords[currentJumper].GetComponent<CompetitionResultRecord>();
            // crr.competitionResult.SetJumpResult(computerResult, currentSerie);
            Debug.Log("Odleglosc komputera: " + computerResult.jumpDistance);
            // crr.resultPoints.text = crr.competitionResult.points.ToString() + "pkt";

            /* if (currentSerie == 1) {
                crr.firstJump.text = crr.competitionResult.jumpResults[0].jumpDistance.ToString() + "m";
            }
            else {
                crr.secondJump.text = crr.competitionResult.jumpResults[1].jumpDistance.ToString()+ "m";
            } */
            RenderResultList();
            currentJumper++;

            yield return new WaitForSeconds(0.5f);
            NextState();
        }   
    }

    private void NextState() {   
        Text playButtonText = currentView.viewPanel.GetComponentInChildren<Button>().gameObject.GetComponentInChildren<Text>();
        
        if (currentJumper == competitionScrollPanelRecords.Count && currentSerie == 2) {
            competitionState = END_NEXT;
            playButtonText.text = "Zakończ";
            return;
        }
        else if (currentJumper == competitionScrollPanelRecords.Count) {
            currentJumper = 0;
            currentSerie++;
        }

        SetIsPlayerNext();

        if (playerNextMove) {
            competitionState = PLAYER_NEXT;
            playButtonText.text = "Skacz";
        }
        else {
            competitionState = COMPUTER_NEXT;
            playButtonText.text = "Kontynuuj";
        }
    }

    public void SetIsPlayerNext() {
        if (competitionScrollPanelRecords.ElementAt(currentJumper).GetComponent<CompetitionResultRecord>().competitionResult.skiJumper.isComputer) {
            playerNextMove = false;
        }
        else {
            playerNextMove = true;
        }
        Debug.Log("PlayerNextMove: " + playerNextMove);
    }

    public void RenderSkiJumpers() {
        List<SkiJumper> skiJumpers = SkiJumperDatabase.LoadSkiJumpers();
        int position = 1;
        GameObject record;
        CompetitionResult competitionResult;
        CompetitionResultRecord competitionResultRecord;

        foreach (SkiJumper sj in skiJumpers) {
            record = Instantiate(competitionScrollPanelRecordPrefab, resultsScrollPanelContent.transform);
            competitionResult = new CompetitionResult(position, sj, 2);
            competitionResultRecord = record.GetComponent<CompetitionResultRecord>();
            competitionResultRecord.SetCompetitionResult(competitionResult); 
            competitionResultRecord.Render();

            competitionScrollPanelRecords.Add(record);
            skiJumpersResults.Add(competitionResult);
            position++;
        }

        GameObject playerRecord = Instantiate(competitionScrollPanelRecordPrefab, resultsScrollPanelContent.transform);
        SkiJumper player = new SkiJumper("Gracz", "Polska", false);
        CompetitionResult playerCr = new CompetitionResult(position, player, 2);
        CompetitionResultRecord playerCrpr = playerRecord.GetComponent<CompetitionResultRecord>();
        playerCrpr.SetCompetitionResult(playerCr);
        playerCrpr.Render();

        competitionScrollPanelRecords.Add(playerRecord);
        skiJumpersResults.Add(playerCr);
    }

    public void RenderResultList() {
        int index = 0;
        CompetitionResultRecord crr;

        foreach (CompetitionResult cr in skiJumpersResults) {
            crr = competitionScrollPanelRecords[index].GetComponent<CompetitionResultRecord>();
            cr.position = index + 1;
            crr.SetCompetitionResult(cr);
            crr.Render();
            index++;
        }
    }

    public void RenderPlayerResult() {
        JumpResult jr = playerController.GetJumpResultData();
        CompetitionResultRecord crr = competitionScrollPanelRecords[currentJumper].GetComponent<CompetitionResultRecord>();
        
        skiJumpersResults[currentJumper].SetJumpResult(jr, currentSerie);
        skiJumpersResults.Sort(CompetitionResult.Compare);
        
        /* crr.competitionResult.SetJumpResult(jr, currentSerie);
        crr.resultPoints.text = crr.competitionResult.points.ToString() + "pkt";

        if (currentSerie == 1) {
            crr.firstJump.text = crr.competitionResult.jumpResults[0].jumpDistance.ToString() + "m";
        }
        else {
            crr.secondJump.text = crr.competitionResult.jumpResults[1].jumpDistance.ToString()+ "m";
        } */

        RenderResultList();
        currentJumper++;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.currentInputMode == InputManager.SKI_JUMPER) {
            if (Input.GetKeyDown(KeyCode.H)) {
                ToggleHelpPanel();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
