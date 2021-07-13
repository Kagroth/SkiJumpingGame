using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PositionResult : IResultType
{
    public static List<int> pointsMatrix = new List<int>() {
        100, 80, 60, 50, 45, 40, 36, 32, 29, 26, 24, 22, 20, 18, 16,
        15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1
    };

    public void ApplyPoints(List<WorldCupSkiJumperResult> classification, List<CompetitionResult> competitionResults) {
        int pointsToAdd = 0;
        SkiJumper skiJumperToFind = null;
        WorldCupSkiJumperResult wcsjr = null;

        for (int index = 0; index < 30; index++) {
            pointsToAdd = pointsMatrix[index];
            skiJumperToFind = competitionResults[index].skiJumper;
            wcsjr = classification.Where(wcc => wcc.skiJumper.Equals(skiJumperToFind)).First();
            wcsjr.points += pointsToAdd;
        }
    }
}
