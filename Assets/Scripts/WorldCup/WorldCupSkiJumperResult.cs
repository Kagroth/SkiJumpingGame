using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCupSkiJumperResult
{
    public SkiJumper skiJumper;
    public int points;

    public WorldCupSkiJumperResult(SkiJumper newSkiJumper) {
        skiJumper = newSkiJumper;
        points = 0;
    }

    public static int Compare(WorldCupSkiJumperResult result1, WorldCupSkiJumperResult result2) {
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
