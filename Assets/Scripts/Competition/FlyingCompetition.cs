using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCompetition : Competition
{
    public FlyingCompetition() {
        qualificationList = new List<CompetitionResult>();
        firstRoundList = new List<CompetitionResult>();
        secondRoundList = new List<CompetitionResult>();
        finalResults = new List<CompetitionResult>();
        qualificationSeriesCount = 1;
        competitionSeriesCount = 2;
    }

    public FlyingCompetition(HillData hd) : this() {
        hillData = hd;
    }

    public override void EndQualification() {
        qualificationList.Sort(CompetitionResult.Compare);
        int qualifiedSkiJumpersCount = qualificationList.Count < 40 ? qualificationList.Count : 40;
        List<CompetitionResult> qualifiedJumpers = qualificationList.GetRange(0, qualifiedSkiJumpersCount);

        for (int index = 1; index <= qualifiedSkiJumpersCount; index++) {
            CompetitionResult competitionResult = new CompetitionResult(index, index, qualifiedJumpers[index - 1].skiJumper, competitionSeriesCount);
            firstRoundList.Add(competitionResult);
        }
    }

    public override void EndFirstRound() {
        firstRoundList.Sort(CompetitionResult.Compare);
        int secondRoundJumpersCount = firstRoundList.Count < 30 ? firstRoundList.Count : 30; 
        secondRoundList = firstRoundList.GetRange(0, secondRoundJumpersCount);
    }
}
