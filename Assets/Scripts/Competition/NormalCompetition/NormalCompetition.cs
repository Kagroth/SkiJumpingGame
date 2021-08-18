using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCompetition : Competition
{
    public NormalCompetition() {
        qualificationList = new List<CompetitionResult>();
        results = new List<List<CompetitionResult>>();
        competitionSeriesCount = 2;

        for (int i = 0; i < competitionSeriesCount; i++) {
            List<CompetitionResult> listCR = new List<CompetitionResult>();
            results.Add(listCR);
        }
    }

    public NormalCompetition(HillData hd) : this() {
        hillData = hd;
    }

    public override void EndQualification() {
        qualificationList.Sort(CompetitionResult.Compare);
        int qualifiedSkiJumpersCount = qualificationList.Count < 50 ? qualificationList.Count : 50;
        List<CompetitionResult> qualifiedJumpers = qualificationList.GetRange(0, qualifiedSkiJumpersCount);
        List<CompetitionResult> firstRoundList = new List<CompetitionResult>();

        for (int index = 1; index <= qualifiedSkiJumpersCount; index++) {
            CompetitionResult competitionResult = new CompetitionResult(index, index, qualifiedJumpers[index - 1].skiJumper);
            firstRoundList.Add(competitionResult);
        }

        results[0] = firstRoundList;
    }

    public override void EndRound(int roundIndex) {
        if (roundIndex >= competitionSeriesCount) {
            Debug.LogError("RoundIndex higher or equal to competitionSeriesCount \nRoundIndex: " + roundIndex + "\nCompetition Series Count: " + competitionSeriesCount);
            return;
        }

        results[roundIndex].Sort(CompetitionResult.Compare);
        int nextRoundJumpersCount = 0;
        List<CompetitionResult> nextRoundList = null;

        if (roundIndex == 0) {
            nextRoundJumpersCount = results[roundIndex].Count < 30 ? results[0].Count : 30; 
        }
        else {
            nextRoundJumpersCount = results[roundIndex].Count;
        }

        nextRoundList = results[roundIndex].GetRange(0, nextRoundJumpersCount);
        results[roundIndex + 1] = nextRoundList;
    }
}
