using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
public class CompetitionUIManager : UIManager
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
        
        // ******************************************************************
        competition = WorldCupData.GetCurrentCompetition();
        competition.SetCompetitionParticipants(WorldCupData.GetWorldCupParticipants());
        Debug.Log("WorldCup participants count: " + WorldCupData.GetWorldCupParticipants().Count);
        Debug.Log("Current competition participants count: " + competition.GetQualificationList().Count);
        // ******************************************************************
        currentView = views.Where(view => view.name.Equals("CompetitionInfo")).First();
        InputManager.SetInputMode(InputManager.COMPETITION_UI);
        
        HideJumpResultPanel();
        helpPanelShow = false;
        
        InitPlayerController();

        currentSerie = 1;
        ResetNextJumperPointer();
        StartQualification();
        CreateCompetitionResultRecords();
        RenderResultList();
        roundState = NextState();

        currentView.Show();
    }
    public override void Init(GameManager gameManager)
    {
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
    }
    private void StartQualification() {
        CreateQualificationList();
        competitionState = QUALIFICATION_ROUND;
        currentContextResults = competition.GetQualificationList();
        currentContextSeriesCount = competition.GetQualificationSeriesCount();
    }

    private void StartCompetition() {
        competitionState = COMPETITION_ROUND;
        CreateCompetitionList();
        FadeResultRecords(50);
        ResetNextJumperPointer();
        currentSerie = 1;
        currentContextResults = competition.GetRoundList(1);
        currentContextSeriesCount = competition.GetCompetitionSeriesCount();
        RenderResultList();
    }

    private void StartNextCompetitionRound() {
        currentSerie++;
        CreateSecondRoundCompetitionList();
        FadeResultRecords(30);
        ResetNextJumperPointer();
        RenderResultList();
    }

    /*
        Metoda podpieta pod przycisk w prefabie JumpUI w scenie Competition
    */
    public void RunSimulation() {
        if (roundState == END_NEXT) {
            Debug.Log("Koniec rundy");
            if (competitionState == QUALIFICATION_ROUND && currentSerie == currentContextSeriesCount) {
                // 1 seria konkursowa
                competition.EndQualification();
                StartCompetition();
                roundState = NextState();
            }
            else if (competitionState == COMPETITION_ROUND) {
                if (currentSerie == currentContextSeriesCount) {
                    // zakoncz konkurs
                    if (WorldCupData.isRandomCompetition) {
                        SceneManager.LoadScene("MainMenu");
                        return;
                    }
                    
                    WorldCupData.FinishCompetition(currentContextResults);
                    SceneManager.LoadScene("WorldCup");
                    return;
                }
                else {
                    // 2 seria konkursowa
                    competition.EndFirstRound();
                    StartNextCompetitionRound();
                    roundState = NextState();
                }
            }
        }

        while (roundState != END_NEXT) {
            if (roundState == COMPUTER_NEXT) {
                SimulateCurrentComputerJump();
            }        
            else if (roundState == PLAYER_NEXT) {
                SwitchToCompetition();
                break;
            }

            roundState = NextState();
        }        
    }

    public void SimulateCurrentComputerJump() {
        JumpResult computerResult = skiJumperSimulator.SimulateJump();
        startList[currentJumper].SetJumpResult(computerResult, currentSerie);
    }

    private void NextJumper() {
        currentJumper += jumperPointerMoveDirection;
        completedJumps++;
    }

    private string NextState() {
        NextJumper();

        if (currentJumper == startList.Count) {
            return END_NEXT;
        }

        if (startList[currentJumper].skiJumper.isComputer) {
            return COMPUTER_NEXT;
        }

        return PLAYER_NEXT;
    }

    /* private void NextState() {   
        Text playButtonText = currentView.viewPanel.GetComponentInChildren<Button>().gameObject.GetComponentInChildren<Text>();
        
        if (currentJumper == startList.Count) {
            Debug.Log("CURRENT JUMPER MADAFAKA " + currentJumper);
            Debug.Log("Rozmiar listy startowej " + startList.Count);
            roundState = END_NEXT;
            playButtonText.text = "Zakończ";
            return;
        }    

        SetIsPlayerNext();
        if (playerNextMove) {
            roundState = PLAYER_NEXT;
            playButtonText.text = "Skacz";
        }
        else {
            roundState = COMPUTER_NEXT;
            // playButtonText.text = "Kontynuuj";
        }

        Debug.Log("Current RoundState: " + roundState);
        Debug.Log("Current jumper: " + currentJumper);
        Debug.Log("Current competition scroll panel records: " + competitionScrollPanelRecords.Count);
        Debug.Log("Current serie: " + currentSerie);

    } */
    
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
            CompetitionResult cr = new CompetitionResult(index, index, sj, seriesCount);
            outputList.Add(cr);
            index++;
        }
    }

    private void CreateQualificationList() {
        List<CompetitionResult> qualificationList = competition.GetQualificationList();
        ListUtils.CopyList(qualificationList, startList);
    }

    private void CreateCompetitionList() {
        List<CompetitionResult> firstRoundList = competition.GetRoundList(1);
        ListUtils.CopyList(firstRoundList, startList);
    }

    private void CreateSecondRoundCompetitionList() { 
        List<CompetitionResult> secondRoundList = competition.GetRoundList(2);
        ListUtils.CopyList(secondRoundList, startList);
        startList.Reverse();
    }
    private void ResetNextJumperPointer() {
        currentJumper = -1;
        completedJumps = 0;
        jumperPointerMoveDirection = 1;
    }
    public void SetPlayerResult() {
        JumpResult jr = playerController.GetJumpResultData();

        startList[currentJumper].SetJumpResult(jr, currentSerie);
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
