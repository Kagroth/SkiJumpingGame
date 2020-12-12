using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICompetition {
    string GetHillName();
    int GetQualificationSeriesCount();
    int GetCompetitionSeriesCount();
    void SetCompetitionParticipants(List<SkiJumper> skiJumpers);
    
    List<CompetitionResult> GetQualificationList();
    List<CompetitionResult> GetRoundList(int round);

    void StartQualification();
    void EndQualification();

    void StartRound();
    void NextJumper();
    void EndFirstRound();
    void EndRound();
    void Complete();
    bool IsCompleted();
}

