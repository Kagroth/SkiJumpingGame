using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CompetitionResult
{
    public int position;
    public SkiJumper skiJumper;
    public List<JumpResult> jumpResults;
    public float points;

    public int seriesCount;

    public CompetitionResult() {

    }

    public CompetitionResult(int pos, SkiJumper jumperToSet, int series) {
        position = pos;
        skiJumper = jumperToSet;
        jumpResults = new List<JumpResult>();
        points = 0;
        seriesCount = series;

        for (int index = 0; index < seriesCount; index++) {
            jumpResults.Add(new JumpResult());
        }
    }

    public void SetJumpResult(JumpResult jr, int serie) {
        jumpResults[serie - 1] = jr;

        points += jr.jumpPoints;
    }

    public static int Compare(CompetitionResult result1, CompetitionResult result2) {
        if (result1.points > result2.points) {
            return -1;
        }
        else if (result1.points == result2.points) {
            return 0;
        }
        else {
            return 1;
        }
    }
}
