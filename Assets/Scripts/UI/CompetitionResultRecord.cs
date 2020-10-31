using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompetitionResultRecord : MonoBehaviour
{
    public Text position;
    public Text skiJumperName;
    public Text firstJump;
    public Text secondJump;
    public Text resultPoints;
    public CompetitionResult competitionResult;

    public void SetCompetitionResult(CompetitionResult cr) {
        competitionResult = cr;
    }

    public void Render() {
        position.text = competitionResult.position.ToString();
        skiJumperName.text = competitionResult.skiJumper.skiJumperName;
        resultPoints.text = competitionResult.points.ToString();

        if (competitionResult.jumpResults[0] != null) {
            firstJump.text = competitionResult.jumpResults[0].jumpDistance.ToString() + "m";
        }

        if (competitionResult.jumpResults[1] != null) {
            secondJump.text = competitionResult.jumpResults[1].jumpDistance.ToString() + "m";
        }

        if (!competitionResult.skiJumper.isComputer) {
            skiJumperName.color = new Color(0, 0, 255, 150);
        }
        else {
            skiJumperName.color = Color.black;
        }
    }    
}
