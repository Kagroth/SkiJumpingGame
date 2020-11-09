using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldCupUIManager : UIManager
{
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
        WorldCupData.CreateQuickWorldCup();
        // *********
        
        competitionListPanelRecords = new List<GameObject>();
        competitionListRecords = new List<CompetitionListRecord>();

        classificationListPanelRecords = new List<GameObject>();
        classificationListRecords = new List<WorldCupClassificationRecord>();

        currentCompetition = 0;
        
        List<WorldCupCompetition> worldCupCompetitions = WorldCupData.worldCupCompetitions;
        List<WorldCupSkiJumperResult> worldCupSkiJumperResults = WorldCupData.worldCupClassification;

        int index = 1;

        foreach (WorldCupCompetition wcc in worldCupCompetitions) {
            GameObject go = Instantiate(competitionListPanelRecordPrefab, competitionListPanelContent.transform);
            CompetitionListRecord clr = go.GetComponent<CompetitionListRecord>();
            clr.SetData(index, wcc.hillName, "I");
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
    }

    public void StartCompetition() {
        SceneManager.LoadScene("Competition");
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
