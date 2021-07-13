using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICompetition {
    string GetHillName();
    int GetCompetitionSeriesCount(); // liczba serii konkursowych
    void SetCompetitionParticipants(List<SkiJumper> skiJumpers);
    List<CompetitionResult> GetQualificationList();
    List<CompetitionResult> GetRoundList(int round); // pobierz wyniki po "round" seriach (po 1 serii, 2 serii itd.)
    IResultType GetResultType();
    void StartQualification();
    void EndQualification();
    void StartRound(int roundIndex);
    void NextJumper();
    void EndRound(int roundIndex);
    void Complete();
    bool IsCompleted();
}

