using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompetitionResultRecord : MonoBehaviour
{
    public bool faded = false;
    public Text position;
    public Text bibNumer;
    public Image countryFlag;
    public Text skiJumperName;
    public Text firstJump;
    public Text secondJump;
    public Text resultPoints;
    public CompetitionResult competitionResult;

    public void SetCompetitionResult(CompetitionResult cr) {
        competitionResult = cr;
        bibNumer.text = cr.bib.ToString();
        Debug.Log("Kraj: " + cr.skiJumper.country);
        countryFlag.sprite = Resources.Load<Sprite>("Sprites/Country Flags/" + cr.skiJumper.country);
    }

    public void SetFade(bool fade) {
        faded = fade;
    }

    public void SetColor(Color newColor) {
        position.color      = newColor;
        bibNumer.color      = newColor;
        skiJumperName.color = newColor;
        firstJump.color     = newColor;
        secondJump.color    = newColor;
        resultPoints.color  = newColor;
    }

    public void Render() {

        position.text = competitionResult.position.ToString();
        skiJumperName.text = competitionResult.skiJumper.skiJumperName;
        resultPoints.text = competitionResult.points.ToString();
        
        if (competitionResult.jumpResults[0] != null) {
            firstJump.text = competitionResult.jumpResults[0].jumpDistance.ToString() + "m";
        }
        
        if (competitionResult.jumpResults.Count == 2) {
            if (competitionResult.jumpResults[1] != null) {
                secondJump.text = competitionResult.jumpResults[1].jumpDistance.ToString() + "m";
            }
        }
        else {
            secondJump.text = "-";
        }

        if (!competitionResult.skiJumper.isComputer) {
            if (faded) {
                SetColor(new Color(0, 0, 200));
            }
            else {
                SetColor(Color.blue);
            }
        }
        else {
            if (faded) {
                SetColor(Color.grey);                
            }
            else {
                SetColor(Color.black);
            }
        }

        
    }    
}
