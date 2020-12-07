using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICompetition {

    void SetCompetitionParticipants(List<SkiJumper> skiJumpers);
    void StartQualification();
    void EndQualification();

    void StartRound();
    void NextJumper();
    void EndFirstRound();
    void EndRound();
    void Complete();
}

