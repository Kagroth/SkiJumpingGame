using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class CompetitionManager : UIManager
{
    public View[] views;
    private View currentView;
    private SkiJumperSimulator skiJumperSimulator;

    // Competition View UI
    [SerializeField] private GameObject jumpResultPanel;
    [SerializeField] private Text distanceValue;
    [SerializeField] private Text jumpPoints;
    [SerializeField] private Text resultPoints;
    [SerializeField] private Text[] judgePoints; 
    [SerializeField] private Text position;
    private bool helpPanelShow;

    [SerializeField] private GameObject helpPanel;
    [SerializeField] public GameObject gameplayPanel;
    private WindMeterUI windMeter;

    // ************************************
    public ICompetition competition;

    // Competition Info UI
    public Text hillName;
    public GameObject resultsScrollPanelContent;
    public GameObject competitionScrollPanelRecordPrefab;
    private List<GameObject> competitionScrollPanelRecords;
    private List<CompetitionResultRecord> competitionResultRecords; 
    // *************************************************
    
    private WorldCupData worldCupData;

    // Ski Jumpers Lists
    private List<CompetitionResult> currentContextResults;
    private List<CompetitionResult> startList;
    private PlayerController playerController;
    private int currentContextSeriesCount;
    private bool playerNextMove;
    private int jumperPointerMoveDirection;
    private int currentJumper;
    private int currentSerie;
    private int completedJumps;
    private string competitionState;
    private const string QUALIFICATION_ROUND = "QUALIFICATION"; 
    private const string COMPETITION_ROUND = "COMPETITION_ROUND";
    private string roundState;
    private const string COMPUTER_NEXT = "COMPUTER_NEXT";
    private const string PLAYER_NEXT = "PLAYER_NEXT";
    private const string END_NEXT = "END_NEXT";

    public override void Init()
    {
        skiJumperSimulator = new SkiJumperSimulator();
        skiJumperSimulator.SetHill(hill);
        
        competitionResultRecords      = new List<CompetitionResultRecord>();
        startList                     = new List<CompetitionResult>();
        competitionScrollPanelRecords = new List<GameObject>();
        
        competition = worldCupData.GetCurrentCompetition();
        competition.SetCompetitionParticipants(worldCupData.GetWorldCupParticipants());

        currentView = views.Where(view => view.name.Equals("CompetitionInfo")).First();
        InputManager.SetInputMode(InputManager.COMPETITION_UI);
        
        HideJumpResultPanel();
        helpPanelShow = false;
        
        // InitPlayerController();

        StartQualification();
        CreateCompetitionResultRecords();
        RenderResultList();
        roundState = NextState();

        currentView.Show();
    }
    public override void Init(GameManager gameManager)
    {
        this.worldCupData = gameManager.GetWorldCupData();
        base.Init(gameManager);
        this.Init();
    }
    public void InitPlayerController() {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.skiJumperStartJumpHandler    += HideJumpResultPanel;
        playerController.skiJumperJumpFinishedHandler += ShowJumpResultPanel;
        playerController.skiJumperEndJumpHandler      += SwitchToInfo;
    }

    public void ShowJumpResultPanel() {
        SetPlayerResult();
        RenderResultList();

        JumpResult jrd     = playerController.GetJumpResultData();
        distanceValue.text = jrd.jumpDistance.ToString();
        jumpPoints.text    = jrd.jumpPoints.ToString();
        resultPoints.text  = startList[currentJumper].points.ToString();
        position.text      = startList[currentJumper].position.ToString();

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
        roundState = NextState();
        
        if (roundState == COMPUTER_NEXT) {
            RunSimulation();
        }
    }
    private void StartQualification() {
        CreateQualificationList();
        competitionState = QUALIFICATION_ROUND;
        currentContextResults = competition.GetQualificationList();
        currentContextSeriesCount = 0;
        ResetNextJumperPointer();
    }

    private void StartCompetition() {
        competitionState = COMPETITION_ROUND;
        CreateRoundList(0);
        FadeResultRecords(50);
        ResetNextJumperPointer();
        currentSerie = 0;
        currentContextResults = competition.GetRoundList(0);
        currentContextSeriesCount = competition.GetCompetitionSeriesCount();
        RenderResultList();
    }

    private void StartNextCompetitionRound() {
        competitionState = COMPETITION_ROUND;
        currentSerie++;
        currentContextResults = competition.GetRoundList(currentSerie);
        CreateRoundListReversed(currentSerie);
        FadeResultRecords(30);
        ResetNextJumperPointer();
        RenderResultList();
    }
    
    // Metoda podpieta pod przycisk w prefabie JumpUI w scenie Competition   
    public void RunSimulation() {
        Debug.Log("Running simulation - Round State: " + roundState + ", Competition State: " + competitionState);
        Debug.Log("Current serie: " + currentSerie);

        if (roundState == END_NEXT) {
            Debug.Log("Koniec rundy");
            if (competitionState == QUALIFICATION_ROUND) {
                // 1 seria konkursowa
                Debug.Log("Rozpoczynam pierwsza serie");
                competition.EndQualification();
                StartCompetition();
                roundState = NextState();
                return;
            }
            else if (competitionState == COMPETITION_ROUND) {
                Debug.Log("Current serie: " + currentSerie);
                Debug.Log("Current series context count: " + currentContextSeriesCount);
                if (currentSerie >= currentContextSeriesCount - 1) {
                    Debug.Log("Koniec konkursu");
                    // zakoncz konkurs
                    if (worldCupData.isRandomCompetition) {
                        SceneManager.LoadScene("MainMenu");
                        return;
                    }
                    
                    worldCupData.FinishCompetition(currentContextResults);
                    SceneManager.LoadScene("WorldCup");
                    return;
                }
                else {
                    // nastepna seria seria konkursowa
                    competition.EndRound(currentSerie);
                    StartNextCompetitionRound();
                    Debug.Log("Rozpoczynam serie nr " + currentSerie);
                    roundState = NextState();
                    return;
                }
            }
        }       

        while (roundState == COMPUTER_NEXT) {
            SimulateCurrentComputerJump();
            roundState = NextState();
        }
        
        currentContextResults.Sort(CompetitionResult.Compare);
        RenderResultList();
        
        /* if (roundState == PLAYER_NEXT) {
            SwitchToCompetition();
            return;
        }
        else {
            currentContextResults.Sort(CompetitionResult.Compare);
            RenderResultList();
        } */
    }

    public void SimulateCurrentComputerJump() {
        JumpResult computerResult = skiJumperSimulator.SimulateJump();
        startList[currentJumper].PushJumpResult(computerResult);
    }

    private void NextJumper() {
        currentJumper += jumperPointerMoveDirection;
        completedJumps++;
    }

    private string NextState() {
        NextJumper();
        
        Text playButtonText = currentView.viewPanel.GetComponentInChildren<Button>().gameObject.GetComponentInChildren<Text>();

        if (completedJumps == startList.Count + 1) {
        // if (currentJumper == startList.Count) {
            playButtonText.text = "Zakończ";
            return END_NEXT;
        }
        else {
            return COMPUTER_NEXT;
        }
        
        // playButtonText.text = "Skacz";

        /* if (startList[currentJumper].skiJumper.isComputer) {
            return COMPUTER_NEXT;
        }

        return PLAYER_NEXT; */
    }
    
    private void CreateCompetitionResultRecords() {
        GameObject record;
        CompetitionResultRecord competitionResultRecord;

        foreach (CompetitionResult cr in currentContextResults) {
            record = Instantiate(competitionScrollPanelRecordPrefab, resultsScrollPanelContent.transform);
            competitionResultRecord = record.GetComponent<CompetitionResultRecord>();
            competitionResultRecord.SetCompetitionResult(cr); 
            competitionResultRecord.Render();

            competitionScrollPanelRecords.Add(record);
            competitionResultRecords.Add(competitionResultRecord);
        }
    }

    private void FadeResultRecords(int from) {
        if (from >= competitionResultRecords.Count) {
            return;
        }

        for (int index = from; index < competitionResultRecords.Count; index++) {
            competitionResultRecords[index].SetFade(true);
            competitionResultRecords[index].Render();
        }
    }

    public void RenderResultList() {
        int index = 0;

        foreach (CompetitionResult cr in currentContextResults) {
            cr.position = index + 1;
            competitionResultRecords[index].SetCompetitionResult(cr);
            competitionResultRecords[index].Render();
            index++;
        }
    }

    private void InitCompetitionList(List<SkiJumper> skiJumpersListSource, List<CompetitionResult> outputList, int seriesCount) {
        int index = 1;

        foreach(SkiJumper sj in skiJumpersListSource) {
            CompetitionResult cr = new CompetitionResult(index, index, sj);
            outputList.Add(cr);
            index++;
        }
    }

    private void CreateQualificationList() {
        List<CompetitionResult> qualificationList = competition.GetQualificationList();
        ListUtils.CopyList(qualificationList, startList);
    }

    private void CreateRoundList(int round) {
        List<CompetitionResult> nextRoundList = competition.GetRoundList(round);
        ListUtils.CopyList(nextRoundList, startList);
    }
    private void CreateRoundListReversed(int round) {
        List<CompetitionResult> nextRoundList = competition.GetRoundList(round);
        ListUtils.CopyList(nextRoundList, startList);
        startList.Reverse();
    }

    private void ResetNextJumperPointer() {
        completedJumps = 0;

        if (competitionState == QUALIFICATION_ROUND || (competitionState == COMPETITION_ROUND && currentSerie == 0)) {
            currentJumper = -1;
            jumperPointerMoveDirection = 1;
        }
        else {
            currentJumper = startList.Count;
            jumperPointerMoveDirection = -1;
        }
    }
    public void SetPlayerResult() {
        JumpResult jr = playerController.GetJumpResultData();

        startList[currentJumper].PushJumpResult(jr);
        currentContextResults.Sort(CompetitionResult.Compare);
    }
    
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
