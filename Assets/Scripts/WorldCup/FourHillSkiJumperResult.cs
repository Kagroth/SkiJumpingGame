using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourHillSkiJumperResult
{
    public SkiJumper skiJumper;
    public float points;

    public FourHillSkiJumperResult(SkiJumper jumper) {
        skiJumper = jumper;
        points = 0;
    }

    public static int Compare(FourHillSkiJumperResult result1, FourHillSkiJumperResult result2) {
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
