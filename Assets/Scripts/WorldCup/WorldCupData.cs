using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldCupData
{
    public bool isRandomCompetition;
    public List<int> pointsMatrix = new List<int>() {
        100, 80, 60, 50, 45, 40, 36, 32, 29, 26, 24, 22, 20, 18, 16,
        15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1
    };

    public List<ICompetition> worldCupCompetitions;
    public List<SkiJumper> worldCupParticipants;

    public WorldCupClassification worldCupClassification;

    public int currentCompetition;

    public WorldCupData(List<SkiJumper> skiJumpers) {
        worldCupParticipants = skiJumpers;
    }

    public void SetParticipants(List<SkiJumper> participants) {
        worldCupParticipants = new List<SkiJumper>();
        ListUtils.CopyList(participants, worldCupParticipants);
    }

    public void CreateWorldCup() {
        currentCompetition = 0;

        if (worldCupParticipants == null || worldCupParticipants.Count == 0) {
            worldCupParticipants = SkiJumperDatabase.LoadSkiJumpers();
        }

        worldCupClassification = new WorldCupClassification(worldCupParticipants);
    }
    
    public void CreateRandomCompetition(HillData hillData) {
        isRandomCompetition = true;
        PushCompetition(hillData);
        CreateWorldCup();
    }

    public void CreateQuickWorldCup() {
        isRandomCompetition = false;
        CreateWorldCup();
    }

    public void CreateQuickWorldCup(List<HillData> hills) {
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
    public List<SkiJumper> GetWorldCupParticipants() {
        return worldCupClassification.worldCupList.OrderBy(wcsjr => wcsjr.points)
                                    .Select(wcsjr => wcsjr.skiJumper)
                                    .ToList();
    }

    public void PushCompetition(HillData hillData) {
        if (worldCupCompetitions == null) {
            worldCupCompetitions = new List<ICompetition>();
        }

        worldCupCompetitions.Add(new NormalCompetition(hillData));
    }

    public ICompetition GetCurrentCompetition() {
        return worldCupCompetitions[currentCompetition];
    }

    public void NextCompetiion() {
        currentCompetition++;
    }

    // do przerobienia - rozne metody zliczania punktow oraz sledzenie roznych klasyfikacji: pś, t4s, pśwl itp
    public void FinishCompetition(List<CompetitionResult> competitionResults) {        
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

    public void ApplyPoints(List<CompetitionResult> competitionResults) {
        ICompetition currentComp = worldCupCompetitions[currentCompetition];
        IResultType currentCompResultType = currentComp.GetResultType();

        currentCompResultType.ApplyPoints(worldCupClassification.worldCupList, competitionResults);
    }
}
