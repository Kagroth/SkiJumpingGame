using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SkiJumperStats
{
    private int skill;

    public SkiJumperStats(int skill) {
        this.SetSkill(skill);
    }

    public void SetSkill(int skill) {
        this.skill = Mathf.Clamp(skill, 0, 100);
    }

    public int GetSkill() {
        return this.skill;
    }
}
