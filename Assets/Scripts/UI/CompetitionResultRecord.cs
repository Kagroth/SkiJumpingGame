﻿using System.Collections;
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
        firstJump.text = "0";
        secondJump.text = "0";
        resultPoints.text = "0";
    }    
}