using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldCupClassificationRecord : MonoBehaviour
{
    public Text position;
    public Image countryFlag;
    public Text skiJumperName;
    public Text points;

    public void SetData(int newPosition, WorldCupSkiJumperResult worldCupSkiJumperResult) {
        position.text = newPosition.ToString();
        skiJumperName.text = worldCupSkiJumperResult.skiJumper.skiJumperName;
        countryFlag.sprite = Resources.Load<Sprite>("Sprites/Country Flags/" + worldCupSkiJumperResult.skiJumper.country);
        points.text = worldCupSkiJumperResult.points.ToString();

        if (worldCupSkiJumperResult.skiJumper.isComputer) {
            SetColor(Color.black);
        }
        else {
            SetColor(new Color(0, 0, 200));
        }
    }
    
    public void SetColor(Color newColor) {
        position.color      = newColor;
        skiJumperName.color = newColor;
        points.color        = newColor;
    }
}
