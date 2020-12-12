using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class WorldCupUIManager : UIManager
{
    public Text nextCompetitionLabel;
    public GameObject nextCompetitionButton;

    // Competition List Panel
    public GameObject competitionListPanelContent;

    public GameObject competitionListPanelRecordPrefab;
    private List<GameObject> competitionListPanelRecords;
    private List<CompetitionListRecord> competitionListRecords;
    // *********************************************************

    // Classification List Panel
    public GameObject classificationListPanelContent;
    public GameObject classificationListPanelRecordPrefab;

    private List<GameObject> classificationListPanelRecords;
    private List<WorldCupClassificationRecord> classificationListRecords;

    // *********************************************************

    private int currentCompetition;

    public override void Init()
    {
        base.Init();
        // temporary
        // WorldCupData.CreateQuickWorldCup();
        // *********
        
        competitionListPanelRecords = new List<GameObject>();
        competitionListRecords = new List<CompetitionListRecord>();

        classificationListPanelRecords = new List<GameObject>();
        classificationListRecords = new List<WorldCupClassificationRecord>();

        currentCompetition = 0;
        
        List<ICompetition> worldCupCompetitions = WorldCupData.worldCupCompetitions;
        List<WorldCupSkiJumperResult> worldCupSkiJumperResults = WorldCupData.worldCupClassification;
        
        worldCupSkiJumperResults.Sort(WorldCupSkiJumperResult.Compare);

        int index = 1;

        foreach (ICompetition wcc in worldCupCompetitions) {
            GameObject go = Instantiate(competitionListPanelRecordPrefab, competitionListPanelContent.transform);
            CompetitionListRecord clr = go.GetComponent<CompetitionListRecord>();
            clr.SetData(index, wcc.GetHillName(), "I");

            if (wcc.IsCompleted()) {
                clr.Complete();
            }
            else if ((index - 1) == WorldCupData.currentCompetition) {
                clr.SetColor(Color.yellow);
                nextCompetitionLabel.text = "Następny konkurs: " + wcc.GetHillName();
            }

            competitionListRecords.Add(clr);
            competitionListPanelRecords.Add(go);
            index++;
        }

        index = 1;

        foreach (WorldCupSkiJumperResult wcjr in worldCupSkiJumperResults) {
            GameObject go = Instantiate(classificationListPanelRecordPrefab, classificationListPanelContent.transform);
            WorldCupClassificationRecord wccr = go.GetComponent<WorldCupClassificationRecord>();
            wccr.SetData(index, wcjr);

            classificationListPanelRecords.Add(go);
            classificationListRecords.Add(wccr);
            index++;
        }

        if (WorldCupData.currentCompetition == WorldCupData.worldCupCompetitions.Count) {
            Text playButtonText = nextCompetitionButton.GetComponentInChildren<Text>();
            playButtonText.text = "Zakończ puchar";
        }
    }

    public void StartCompetition() {
        if (WorldCupData.currentCompetition == WorldCupData.worldCupCompetitions.Count) {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        SceneManager.LoadScene("Competition");
        return;
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
