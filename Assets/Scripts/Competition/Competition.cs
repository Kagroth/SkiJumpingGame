using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Competition : ICompetition
{
    public HillData hillData;
    public bool completed;

    protected int qualificationSeriesCount;
    protected int competitionSeriesCount;

    public List<CompetitionResult> qualificationList;
    public List<CompetitionResult> firstRoundList;
    public List<CompetitionResult> secondRoundList;
    public List<CompetitionResult> finalResults;

    public string GetHillName() {
        return hillData.hillName;
    }

    public void SetCompetitionParticipants(List<SkiJumper> skiJumpers) {
        for (int index = 1; index <= skiJumpers.Count; index++) {
            qualificationList.Add(new CompetitionResult(index, index, skiJumpers[index - 1], qualificationSeriesCont));
        }
    }

    public int GetQualificationSeriesCount() {
        return qualificationSeriesCount;
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
    
    public virtual void EndQualification() {

    }

    public virtual void EndFirstRound() {

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
