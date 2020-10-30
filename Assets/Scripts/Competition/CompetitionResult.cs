using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetitionResult
{
    public int position;
    public SkiJumper skiJumper;
    public JumpResult firstJumpResult;
    public JumpResult secondJumpResult;
    public float points;

    public CompetitionResult() {

    }

    public CompetitionResult(int pos, SkiJumper jumperToSet) {
        position = pos;
        skiJumper = jumperToSet;
        firstJumpResult = null;
        secondJumpResult = null;
        points = 0;
    }
}
