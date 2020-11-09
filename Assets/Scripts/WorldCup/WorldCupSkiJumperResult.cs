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
}
