using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PointResult : IResultType
{
    public void ApplyPoints(List<WorldCupSkiJumperResult> classification, List<CompetitionResult> competitionResults) {
        float pointsToAdd = 0;
        SkiJumper skiJumperToFind = null;
        WorldCupSkiJumperResult wcsjr = null;

        for (int index = 0; index < 30; index++) {
            pointsToAdd = competitionResults[index].points;
            skiJumperToFind = competitionResults[index].skiJumper;
            wcsjr = classification.Where(wcc => wcc.skiJumper.Equals(skiJumperToFind)).First();
            // wcsjr.points += pointsToAdd; float to int error
        }
    }
}
