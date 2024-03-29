﻿using System.Collections;
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

    private WorldCupData worldCupData;

    private int currentCompetition;

    public override void Init()
    {        
        competitionListPanelRecords = new List<GameObject>();
        competitionListRecords = new List<CompetitionListRecord>();

        classificationListPanelRecords = new List<GameObject>();
        classificationListRecords = new List<WorldCupClassificationRecord>();

        currentCompetition = 0;
        
        List<ICompetition> worldCupCompetitions = worldCupData.worldCupCompetitions;
        List<WorldCupSkiJumperResult> worldCupSkiJumperResults = worldCupData.worldCupClassification.worldCupList;
        
        worldCupSkiJumperResults.Sort(WorldCupSkiJumperResult.Compare);

        int index = 1;

        foreach (ICompetition wcc in worldCupCompetitions) {
            GameObject go = Instantiate(competitionListPanelRecordPrefab, competitionListPanelContent.transform);
            CompetitionListRecord clr = go.GetComponent<CompetitionListRecord>();
            clr.SetData(index, wcc.GetHillName(), "I");

            if (wcc.IsCompleted()) {
                clr.Complete();
            }
            else if ((index - 1) == worldCupData.currentCompetition) {
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

        if (worldCupData.currentCompetition == worldCupData.worldCupCompetitions.Count) {
            Text playButtonText = nextCompetitionButton.GetComponentInChildren<Text>();
            playButtonText.text = "Zakończ puchar";
        }
    }
    public override void Init(GameManager gameManager)
    {
        worldCupData = gameManager.GetWorldCupData();
        base.Init(gameManager);
        this.Init();
    }
    public void StartCompetition() {
        if (worldCupData.currentCompetition == worldCupData.worldCupCompetitions.Count) {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        SceneManager.LoadScene("Competition");
        return;
    }
}
