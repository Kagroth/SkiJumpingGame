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
    private Text jumpPoints;

    [SerializeField]
    private Text resultPoints;

    [SerializeField]
    private Text[] judgePoints; 

    [SerializeField]
    private Text position;

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
    private List<CompetitionResultRecord> competitionResultRecords; 
    // *************************************************
    
    // Ski Jumpers Lists    
    private List<CompetitionResult> qualificationsResults;
    private List<CompetitionResult> competitionResults;

    private List<CompetitionResult> currentContextResults;

    private List<CompetitionResult> startList;

    private List<SkiJumper> skiJumpersList;
    private PlayerController playerController;

    private int qualificationSeriesCount;
    private int competitionSeriesCount;

    private int currentContextSeriesCount;

    private bool playerNextMove;
    
    private int jumperPointerMoveDirection;
    private int currentJumper;
    private int currentSerie;
    private int completedJumps;

    private string competitionState;

    private IEnumerator computerPlayerSimulationCoroutine;

    private const string QUALIFICATION_ROUND = "QUALIFICATION"; 
    private const string COMPETITION_ROUND = "COMPETITION_ROUND";

    private const string COMPETITION_END = "COMPETITION_END"; 

    private string roundState;
    
    private const string COMPUTER_NEXT = "COMPUTER_NEXT";
    private const string PLAYER_NEXT = "PLAYER_NEXT";
    private const string END_NEXT = "END_NEXT";

    public override void Init()
    {
        base.Init();
        skiJumperSimulator = new SkiJumperSimulator();
        skiJumperSimulator.SetHill(hill);
        
        competitionResultRecords      = new List<CompetitionResultRecord>();
        startList                     = new List<CompetitionResult>();
        competitionResults            = new List<CompetitionResult>();
        qualificationsResults         = new List<CompetitionResult>();
        competitionScrollPanelRecords = new List<GameObject>();
        skiJumpersList                = new List<SkiJumper>();

        SkiJumperDatabase.GenerateSkiJumpersFile();
        skiJumpersList = SkiJumperDatabase.LoadSkiJumpers();

        currentView = views.Where(view => view.name.Equals("CompetitionInfo")).First();
        InputManager.SetInputMode(InputManager.COMPETITION_UI);
        
        HideJumpResultPanel();
        helpPanelShow = false;
        
        playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.skiJumperStartJumpHandler    += HideJumpResultPanel;
        playerController.skiJumperJumpFinishedHandler += ShowJumpResultPanel;
        playerController.skiJumperEndJumpHandler      += SwitchToInfo;

        currentSerie = 1;
        qualificationSeriesCount = 1;
        competitionSeriesCount = 2;

        ResetNextJumperPointer();
        StartQualification();
        CreateCompetitionResultRecords();
        RenderResultList();
        NextState();

        currentView.Show();
    }

    private void LogList(List<CompetitionResult> listToLog, string listName) {
        string listStr = "";
        listStr += "Lista startowa - " + listName +": \n"; 
        listStr += "Liczba zawodników: " + listToLog.Count.ToString() + "\n";

        foreach (CompetitionResult cr in listToLog) {
            listStr += cr.skiJumper.skiJumperName + "\n";
        }

        Debug.Log(listStr);
    }

    private void CopyList(List<CompetitionResult> sourceList, List<CompetitionResult> destinationList) {
        destinationList.Clear();

        foreach (CompetitionResult cr in sourceList) {
            destinationList.Add(cr);
        }
    }

    private bool ListReferenceEquals(List<CompetitionResult> listOne, List<CompetitionResult> listTwo) {
        return Object.ReferenceEquals(listOne, listTwo);
    }

    private bool ListContentEquals(List<CompetitionResult> listOne, List<CompetitionResult> listTwo) {
        if (listOne.Count != listTwo.Count) {
            return false;
        }

        for (int index = 0; index < listOne.Count; index++) {
            if (listOne.Contains(listTwo[index])) {
                continue;
            }

            return false;
        }

        return true;
    }

    public void ShowJumpResultPanel() {
        SetPlayerResult();
        RenderResultList();

        JumpResult jrd = playerController.GetJumpResultData();
        distanceValue.text = jrd.jumpDistance.ToString();
        jumpPoints.text = jrd.jumpPoints.ToString();
        resultPoints.text = startList[currentJumper].points.ToString();
        position.text = startList[currentJumper].position.ToString();

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
        NextJumper();
        NextState();
    }
    private void StartQualification() {
        CreateQualificationList();
        competitionState = QUALIFICATION_ROUND;
        currentContextResults = qualificationsResults;
        currentContextSeriesCount = qualificationSeriesCount;
    }

    private void StartCompetition() {
        competitionState = COMPETITION_ROUND;
        CreateCompetitionList();
        FadeResultRecords(50);
        ResetNextJumperPointer();
        currentSerie = 1;
        currentContextResults = competitionResults;
        currentContextSeriesCount = competitionSeriesCount;
        RenderResultList();
        Debug.Log("currentJumper pointer przed 1 seria: " + currentJumper);
    }

    private void StartNextCompetitionRound() {
        currentSerie++;
        CreateSecondRoundCompetitionList();
        FadeResultRecords(30);
        ResetNextJumperPointer();
        RenderResultList();

        for (int index = 0; index < competitionResultRecords.Count; index++) {
            Debug.Log((index + 1) + " - " + competitionResultRecords[index].faded);
        }
        Debug.Log("currentJumper pointer przed 2 seria: " + currentJumper);
    }

    public void RunSimulation() {
        if (roundState == COMPUTER_NEXT) {
            StartCoroutine(SimulateComputerJump());
        }
        else if (roundState == PLAYER_NEXT) {
            SwitchToCompetition();
        }
        else if (roundState == END_NEXT) {
            if (competitionState == QUALIFICATION_ROUND && currentSerie == currentContextSeriesCount) {
                // 1 seria konkursowa
                StartCompetition();
                NextState();
            }
            else if (competitionState == COMPETITION_ROUND) {
                if (currentSerie == currentContextSeriesCount) {
                    // zakoncz konkurs
                    SceneManager.LoadScene("MainMenu");
                    return;
                }
                else {
                    // 2 seria konkursowa
                    StartNextCompetitionRound();
                    NextState();
                }
            }
        }
    }


    public IEnumerator SimulateComputerJump() {
        JumpResult computerResult;

        while (roundState == COMPUTER_NEXT) {
            computerResult = skiJumperSimulator.SimulateJump();
            startList[currentJumper].SetJumpResult(computerResult, currentSerie);
            currentContextResults.Sort(CompetitionResult.Compare);

            RenderResultList();
            NextJumper();

            yield return new WaitForSeconds(0.1f);
            NextState();
        }

        yield break;  
    }

    private void NextJumper() {
        currentJumper += jumperPointerMoveDirection;
        completedJumps++;
    }

    private void NextState() {   
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
            playButtonText.text = "Kontynuuj";
        }

        Debug.Log("Current RoundState: " + roundState);
        Debug.Log("Current jumper: " + currentJumper);
        Debug.Log("Current competition scroll panel records: " + competitionScrollPanelRecords.Count);
        Debug.Log("Current serie: " + currentSerie);

    }

    public void SetIsPlayerNext() {
        if (startList[currentJumper].skiJumper.isComputer) {
            playerNextMove = false;
        }
        else {
            playerNextMove = true;
        }
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

        GameObject playerRecord = Instantiate(competitionScrollPanelRecordPrefab, resultsScrollPanelContent.transform);
        SkiJumper player = new SkiJumper("Gracz", Country.POLAND, false);
        CompetitionResult playerCr = new CompetitionResult(currentContextResults.Count, currentContextResults.Count + 1, player, currentContextSeriesCount);
        CompetitionResultRecord playerCrpr = playerRecord.GetComponent<CompetitionResultRecord>();
        playerCrpr.SetCompetitionResult(playerCr);
        playerCrpr.Render();

        competitionScrollPanelRecords.Add(playerRecord);
        competitionResultRecords.Add(playerCrpr);
        currentContextResults.Add(playerCr);
        startList.Add(playerCr);
    }

    private void FadeResultRecords(int from) {
        if (from >= competitionResultRecords.Count) {
            return;
        }

        for (int index = from; index < competitionResultRecords.Count; index++) {
            competitionResultRecords[index].SetFade(true);
            competitionResultRecords[index].Render();
        }

        for (int index = 0; index < competitionResultRecords.Count; index++) {
            Debug.Log((index + 1) + " - " + competitionResultRecords[index].faded);
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
        InitCompetitionList(skiJumpersList, qualificationsResults, qualificationSeriesCount);
        CopyList(qualificationsResults, startList);
    }

    private void CreateCompetitionList() {
        int qualifiedSkiJumpersCount = qualificationsResults.Count < 50 ? qualificationsResults.Count : 50;
        List<CompetitionResult> qualifiedSkiJumpersResults = qualificationsResults.GetRange(0, qualifiedSkiJumpersCount);        
        qualifiedSkiJumpersResults.Sort(CompetitionResult.Compare);
        qualifiedSkiJumpersResults.Reverse();
        skiJumpersList = qualifiedSkiJumpersResults.Select(cr => cr.skiJumper).ToList();
        InitCompetitionList(skiJumpersList, competitionResults, competitionSeriesCount);
        CopyList(competitionResults, startList);
    }

    private void CreateSecondRoundCompetitionList() {
        competitionResults.Sort(CompetitionResult.Compare);
        int secondRoundJumpersCount = competitionResults.Count < 30 ? qualificationsResults.Count : 30; 
        List<CompetitionResult> secondRoundResults = competitionResults.GetRange(0, secondRoundJumpersCount);
        CopyList(secondRoundResults, startList);

        startList.Reverse();
    }
    private void ResetNextJumperPointer() {
        currentJumper = 0;
        completedJumps = 0;
        jumperPointerMoveDirection = 1;
    }
    public void SetPlayerResult() {
        JumpResult jr = playerController.GetJumpResultData();

        startList[currentJumper].SetJumpResult(jr, currentSerie);
        currentContextResults.Sort(CompetitionResult.Compare);
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
