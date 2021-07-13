using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCompetition : Competition
{
    public FlyingCompetition() {
        qualificationList = new List<CompetitionResult>();
        results = new List<List<CompetitionResult>>();
        competitionSeriesCount = 2;
    }

    public FlyingCompetition(HillData hd) : this() {
        hillData = hd;
    }

    public override void EndQualification() {
        qualificationList.Sort(CompetitionResult.Compare);
        int qualifiedSkiJumpersCount = qualificationList.Count < 40 ? qualificationList.Count : 40;
        List<CompetitionResult> qualifiedJumpers = qualificationList.GetRange(0, qualifiedSkiJumpersCount);
        List<CompetitionResult> firstRoundList = new List<CompetitionResult>();
        
        for (int index = 1; index <= qualifiedSkiJumpersCount; index++) {
            CompetitionResult competitionResult = new CompetitionResult(index, index, qualifiedJumpers[index - 1].skiJumper);
            firstRoundList.Add(competitionResult);
        }

        results.Add(firstRoundList);
    }

    public void EndRound(int roundIndex) {
        results[roundIndex].Sort(CompetitionResult.Compare);
        int secondRoundJumpersCount = results[roundIndex].Count < 30 ? results[0].Count : 30; 
        List<CompetitionResult> secondRound = results[roundIndex].GetRange(0, secondRoundJumpersCount);
        results.Add(secondRound);
    }
}
