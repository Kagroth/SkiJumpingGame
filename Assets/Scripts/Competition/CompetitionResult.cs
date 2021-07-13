using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CompetitionResult
{
    public int position;
    public int bib;
    public SkiJumper skiJumper;
    public List<JumpResult> jumpResults;
    public float points;

    public CompetitionResult() {

    }

    public CompetitionResult(int pos, int bibNumer, SkiJumper jumperToSet) {
        position = pos;
        bib = bibNumer;
        skiJumper = jumperToSet;
        jumpResults = new List<JumpResult>();
        points = 0;
    }

    public void PushJumpResult(JumpResult jr) {
        jumpResults.Add(jr);
    }

    public static int Compare(CompetitionResult result1, CompetitionResult result2) {
        if (result1.points > result2.points) {
            return -1;
        }
        else if (result1.points == result2.points) {
            if (result1.bib > result2.bib) {
                return 1;
            }
            else if (result1.bib < result2.bib) {
                return -1;
            }
            else {
                return 0;
            }
        }
        else {
            return 1;
        }
    }
}
