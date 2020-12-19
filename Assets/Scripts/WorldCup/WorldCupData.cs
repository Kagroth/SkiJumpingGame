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

    public static WorldCupClassification worldCupClassification;

    public static int currentCompetition;

    public static void CreateWorldCup() {
        currentCompetition = 0;
        worldCupParticipants = SkiJumperDatabase.LoadSkiJumpers();
        worldCupClassification = new WorldCupClassification(worldCupParticipants);
    }
    
    public static void CreateRandomCompetition() {
        isRandomCompetition = true;
        CreateWorldCup();
    }

    public static void CreateQuickWorldCup() {
        isRandomCompetition = false;
        CreateWorldCup();
    }

    public static void CreateQuickWorldCup(List<HillData> hills) {
        CreateQuickWorldCup();
        
        worldCupCompetitions = new List<ICompetition>();
        
        for (int index = 0; index < 3; index++) {
            ICompetition competitionToAdd;

            if (hills[index].kPoint >= 185) {
                competitionToAdd = new FlyingCompetition(hills[index]);
            }
            else {
                competitionToAdd = new NormalCompetition(hills[index]);
            }

            worldCupCompetitions.Add(competitionToAdd);
        }
    }


    // Zwraca liste wszystkich zawodnikow bioracych udzial w pucharze swiata
    // posortowana wg zdobytych punktow w klasyfikacji generalnej
    public static List<SkiJumper> GetWorldCupParticipants() {
        return worldCupClassification.worldCupList.OrderBy(wcsjr => wcsjr.points)
                                    .Select(wcsjr => wcsjr.skiJumper)
                                    .ToList();
    }

    public static void PushCompetition(HillData hillData) {
        if (worldCupCompetitions == null) {
            worldCupCompetitions = new List<ICompetition>();
        }

        worldCupCompetitions.Add(new NormalCompetition(hillData));
    }

    public static ICompetition GetCurrentCompetition() {
        return worldCupCompetitions[currentCompetition];
    }

    public static void NextCompetiion() {
        currentCompetition++;
    }

    // do przerobienia - rozne metody zliczania punktow oraz sledzenie roznych klasyfikacji: pś, t4s, pśwl itp
    public static void FinishCompetition(List<CompetitionResult> competitionResults) {        
        worldCupCompetitions[currentCompetition].Complete();

        for (int index = 0; index < 30; index++) {
            int pointsToAdd = pointsMatrix[index];
            SkiJumper skiJumperToFind = competitionResults[index].skiJumper;
            WorldCupSkiJumperResult wcsjr = worldCupClassification.worldCupList.Where(wcc => wcc.skiJumper.Equals(skiJumperToFind)).First();
            wcsjr.points += pointsToAdd;
            Debug.Log("Punkty skoczka " + wcsjr.skiJumper.skiJumperName + " po konkursie: " + wcsjr.points);
        }

        NextCompetiion();
    }
}
