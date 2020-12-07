using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCompetition : ICompetition
{
    public HillData hillData;

    public bool completed;

    public int currentJumperIndex;

    public int currentSerie;
    public int qualificationSeriesCont;
    public int competitionSeriesCount;

    public List<CompetitionResult> startList;
    public List<CompetitionResult> currentContextResultsList;
    public List<CompetitionResult> qualificationList;
    public List<CompetitionResult> firstRoundList;
    public List<CompetitionResult> secondRoundList;
    public List<CompetitionResult> finalResults; 

    public NormalCompetition() {
        startList = new List<CompetitionResult>();
        qualificationList = new List<CompetitionResult>();
    }

    public NormalCompetition(HillData hd) {
        hillData = hd;
    }
    private void CopyList(List<CompetitionResult> sourceList, List<CompetitionResult> destinationList) {
        destinationList.Clear();

        foreach (CompetitionResult cr in sourceList) {
            destinationList.Add(cr);
        }
    }

    public void SetCompetitionParticipants(List<SkiJumper> skiJumpers) {
        for (int index = 1; index < skiJumpers.Count; index++) {
            qualificationList.Add(new CompetitionResult(index, index, skiJumpers[index - 1], qualificationSeriesCont));
        }
    }

    public void StartQualification() {
        currentJumperIndex = 0;
        CopyList(qualificationList, startList);
        currentContextResultsList = qualificationList;
    }

    public void EndQualification() {
        qualificationList.Sort(CompetitionResult.Compare);
        firstRoundList = qualificationList.GetRange(0, 50);
        CopyList(firstRoundList, startList);
        currentContextResultsList = firstRoundList;
    }

    public void EndFirstRound() {
        firstRoundList.Sort(CompetitionResult.Compare);
        secondRoundList = firstRoundList.GetRange(0, 30);
        currentContextResultsList = secondRoundList;
    }

    public void StartRound() {

    }

    public void EndRound() {

    }

    public void NextJumper() {
        currentJumperIndex++;
    }

    public void SetJumpResult(JumpResult jumpResult) {
        startList[currentJumperIndex].SetJumpResult(jumpResult, currentSerie);
    }

    public void Complete() {
        completed = true;
    }
}
