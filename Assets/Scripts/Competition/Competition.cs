using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Competition : ICompetition
{
    /*
        Zamiast bezposrednio trzymac ResulType w klasie, utworzyc liste atrybutow 
        odpowiadajacych rodzajom konkursow (normalny, loty, t4s itp), ktore to beda
        przechowywac ResultType
    */
    public HillData hillData;
    public bool completed;
    protected int competitionSeriesCount;
    public List<CompetitionResult> qualificationList;

    /* results contains competition result lists for every round/serie
    e.g. 
    Normal Competition / KO Competition would have:
        List:
            0 - first serie
            1 - second (last) serie
    
    Flying Championship:
        List:
            0 - first serie
            1 - second serie
            2 - third serie
            3 - last serie
    */
    public List<List<CompetitionResult>> results;

    public IResultType resultType;

    public string GetHillName() {
        return hillData.hillName;
    }

    public void SetCompetitionParticipants(List<SkiJumper> skiJumpers) {
        for (int index = 1; index <= skiJumpers.Count; index++) {
            qualificationList.Add(new CompetitionResult(index, index, skiJumpers[index - 1]));
        }
    }
    
    public int GetCompetitionSeriesCount() {
        return competitionSeriesCount;
    }

    public List<CompetitionResult> GetQualificationList() {
        return qualificationList;
    }

    public List<CompetitionResult> GetRoundList(int round) {
        if (round >= results.Count) {
            Debug.LogError("Too high round value");
            return null;
        }

        return results[round];
    }

    public IResultType GetResultType() {
        return resultType;
    }

    public void StartQualification() {

    }
    
    public virtual void EndQualification() {

    }

    public void StartRound(int roundIndex) {

    }

    public virtual void EndRound(int roundIndex) {

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
