using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCupData
{
    public static List<WorldCupCompetition> worldCupCompetitions;
    public static List<SkiJumper> worldCupParticipants;

    public static List<WorldCupSkiJumperResult> worldCupClassification;

    public static int currentCompetition;

    public static void CreateWorldCup() {
        worldCupCompetitions = new List<WorldCupCompetition>();
    }

    public static void CreateQuickWorldCup() {
        currentCompetition = 0;
        worldCupParticipants = SkiJumperDatabase.LoadSkiJumpers();
        worldCupClassification = new List<WorldCupSkiJumperResult>();

        foreach (SkiJumper skiJumper in worldCupParticipants) {
            worldCupClassification.Add(new WorldCupSkiJumperResult(skiJumper));
        }

        worldCupCompetitions = new List<WorldCupCompetition>();
        worldCupCompetitions.Add(new WorldCupCompetition("Normal-HS98"));
        worldCupCompetitions.Add(new WorldCupCompetition("Large-HS140"));
        worldCupCompetitions.Add(new WorldCupCompetition("Fly-HS215"));
    }

    public static WorldCupCompetition GetCurrentCompetition() {
        return worldCupCompetitions[currentCompetition];
    }

    public static void NextCompetiion() {
        currentCompetition++;
    }
}
