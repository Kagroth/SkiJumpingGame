using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCupCompetition
{
    public string hillName;
    public List<CompetitionResult> results;
    public bool completed;

    public WorldCupCompetition(string newHillName) {
        hillName = newHillName;
        results = new List<CompetitionResult>();
        completed = false;
    }

    public void SetResults(List<CompetitionResult> newResults) {
        results = newResults;
    }
}
