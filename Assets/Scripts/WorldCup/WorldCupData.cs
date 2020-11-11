using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class WorldCupData
{
    public static List<int> pointsMatrix = new List<int>() {
        100, 80, 60, 50, 45, 40, 36, 32, 29, 26, 24, 22, 20, 18, 16,
        15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1
    };

    public static List<WorldCupCompetition> worldCupCompetitions;
    public static List<SkiJumper> worldCupParticipants;

    public static List<WorldCupSkiJumperResult> worldCupClassification;

    public static int currentCompetition;

    public static void CreateWorldCup() {
        worldCupCompetitions = new List<WorldCupCompetition>();
    }

    public static void CreateQuickWorldCup() {
        currentCompetition = 0;
        SkiJumperDatabase.GenerateSkiJumpersFile();
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

    public static void FinishCompetition(List<CompetitionResult> competitionResults) {
        worldCupCompetitions[currentCompetition].completed = true;

        worldCupCompetitions[currentCompetition].SetResults(competitionResults);

        for (int index = 0; index < 30; index++) {
            int pointsToAdd = pointsMatrix[index];
            SkiJumper skiJumperToFind = competitionResults[index].skiJumper;
            WorldCupSkiJumperResult wcsjr = worldCupClassification.Where(wcc => wcc.skiJumper.Equals(skiJumperToFind)).First();
            wcsjr.points += pointsToAdd;
            Debug.Log("Punkty skoczka " + wcsjr.skiJumper.skiJumperName + " po konkursie: " + wcsjr.points);
        }

        NextCompetiion();
    }
}
