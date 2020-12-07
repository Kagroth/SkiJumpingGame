using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class WorldCupData
{
    public static bool isRandomCompetition;
    public static List<int> pointsMatrix = new List<int>() {
        100, 80, 60, 50, 45, 40, 36, 32, 29, 26, 24, 22, 20, 18, 16,
        15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1
    };

    public static List<ICompetition> worldCupCompetitions;
    public static List<SkiJumper> worldCupParticipants;

    public static List<WorldCupSkiJumperResult> worldCupClassification;

    public static int currentCompetition;

    public static void CreateRandomCompetition() {
        isRandomCompetition = true;
    }

    public static void CreateQuickWorldCup() {
        isRandomCompetition = false;

        currentCompetition = 0;
        SkiJumperDatabase.GenerateSkiJumpersFile();
        worldCupParticipants = SkiJumperDatabase.LoadSkiJumpers();
        worldCupClassification = new List<WorldCupSkiJumperResult>();

        foreach (SkiJumper skiJumper in worldCupParticipants) {
            worldCupClassification.Add(new WorldCupSkiJumperResult(skiJumper));
        }

        /* worldCupCompetitions = new List<WorldCupCompetition>();
        worldCupCompetitions.Add(new WorldCupCompetition("Normal-HS98"));
        worldCupCompetitions.Add(new WorldCupCompetition("Large-HS140"));
        worldCupCompetitions.Add(new WorldCupCompetition("Fly-HS215")); */
    }

    // Zwraca liste wszystkich zawodnikow bioracych udzial w pucharze swiata
    // posortowana wg zdobytych punktow w klasyfikacji generalnej
    public static List<SkiJumper> GetWorldCupParticipants() {
        return worldCupClassification.OrderBy(wcsjr => wcsjr.points)
                                    .Select(wcsjr => wcsjr.skiJumper)
                                    .ToList();
    }

    public static ICompetition GetCurrentCompetition() {
        return worldCupCompetitions[currentCompetition];
    }

    public static void NextCompetiion() {
        currentCompetition++;
    }

    public static void FinishCompetition(List<CompetitionResult> competitionResults) {        
        // worldCupCompetitions[currentCompetition].completed = true;
        worldCupCompetitions[currentCompetition].Complete();

        // worldCupCompetitions[currentCompetition].SetResults(competitionResults);

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
