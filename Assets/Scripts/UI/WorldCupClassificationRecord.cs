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
    }
}
