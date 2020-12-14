using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCompetition : ICompetition
{
    public HillData hillData;

    public bool completed;

    private int qualificationSeriesCont;
    private int competitionSeriesCount;

    public List<CompetitionResult> qualificationList;
    public List<CompetitionResult> firstRoundList;
    public List<CompetitionResult> secondRoundList;
    public List<CompetitionResult> finalResults; 

    public NormalCompetition() {
        qualificationList = new List<CompetitionResult>();
        firstRoundList = new List<CompetitionResult>();
        secondRoundList = new List<CompetitionResult>();
        finalResults = new List<CompetitionResult>();
        qualificationSeriesCont = 1;
        competitionSeriesCount = 2;
    }

    public NormalCompetition(HillData hd) : this() {
        hillData = hd;
    }

    public string GetHillName() {
        return hillData.hillName;
    }

    public void SetCompetitionParticipants(List<SkiJumper> skiJumpers) {
        for (int index = 1; index <= skiJumpers.Count; index++) {
            qualificationList.Add(new CompetitionResult(index, index, skiJumpers[index - 1], qualificationSeriesCont));
        }
    }

    public int GetQualificationSeriesCount() {
        return qualificationSeriesCont;
    }
    
    public int GetCompetitionSeriesCount() {
        return competitionSeriesCount;
    }

    public List<CompetitionResult> GetQualificationList() {
        return qualificationList;
    }

    public List<CompetitionResult> GetRoundList(int round) {
        if (round == 1) {
            return firstRoundList;
        }
        else if (round == 2) {
            return secondRoundList;
        }
        else {
            return null;
        }
    }

    public void StartQualification() {

    }

    public void EndQualification() {
        qualificationList.Sort(CompetitionResult.Compare);
        int qualifiedSkiJumpersCount = qualificationList.Count < 50 ? qualificationList.Count : 50;
        List<CompetitionResult> qualifiedJumpers = qualificationList.GetRange(0, qualifiedSkiJumpersCount);

        for (int index = 1; index <= qualifiedSkiJumpersCount; index++) {
            CompetitionResult competitionResult = new CompetitionResult(index, index, qualifiedJumpers[index - 1].skiJumper, competitionSeriesCount);
            firstRoundList.Add(competitionResult);
        }
    }

    public void EndFirstRound() {
        firstRoundList.Sort(CompetitionResult.Compare);
        int secondRoundJumpersCount = firstRoundList.Count < 30 ? firstRoundList.Count : 30; 
        secondRoundList = firstRoundList.GetRange(0, secondRoundJumpersCount);
    }

    public void StartRound() {

    }

    public void EndRound() {

    }

    public void NextJumper() {

    }

    public void Complete() {
        completed = true;
    }

    public bool IsCompleted() {
        return completed;
    }
}
