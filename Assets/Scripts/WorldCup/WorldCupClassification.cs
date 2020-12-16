using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// przepisanie na wzorzec builder
public class WorldCupClassification
{
    public List<SkiJumper> worldCupParticipants;
    public List<WorldCupSkiJumperResult> worldCupList;
    public List<WorldCupSkiJumperResult> worldCupFlyingList;
    public List<FourHillSkiJumperResult> fourHillTournamentList;

    public WorldCupClassification() {
        worldCupList = new List<WorldCupSkiJumperResult>();
        worldCupFlyingList = new List<WorldCupSkiJumperResult>();
        fourHillTournamentList = new List<FourHillSkiJumperResult>();

        foreach (SkiJumper skiJumper in worldCupParticipants) {
            WorldCupSkiJumperResult normalResult = new WorldCupSkiJumperResult(skiJumper);
            WorldCupSkiJumperResult flyingResult = new WorldCupSkiJumperResult(skiJumper);
            FourHillSkiJumperResult fourHillResult = new FourHillSkiJumperResult(skiJumper);

            worldCupList.Add(normalResult);
            worldCupFlyingList.Add(flyingResult);
            fourHillTournamentList.Add(fourHillResult);
        }
    }

    public WorldCupClassification(List<SkiJumper> participantsToSet) : this() {
        worldCupParticipants = participantsToSet;
    }
}
