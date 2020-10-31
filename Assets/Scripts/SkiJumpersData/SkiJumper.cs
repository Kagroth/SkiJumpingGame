using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkiJumper
{
    public string skiJumperName;
    public string country;
    public bool isComputer;

    public SkiJumper(string name, string countryToSet, bool computer) {
        skiJumperName = name;
        country = countryToSet;
        isComputer = computer;
    }
}
