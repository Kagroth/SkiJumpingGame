using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkiJumperComputer
{
    [SerializeField]
    public string firstname;
    
    [SerializeField]
    public string lastname;
    
    [SerializeField]
    public string country;

    [System.Serializable]
    public struct JumpResultData {
        
        [SerializeField]
        public float jumpDistance;
        
        [SerializeField]
        public Judge[] judges;
        
        [SerializeField]
        public float jumpPoints;

        public JumpResultData(float distance, Judge[] judgesArr, float points) {
            jumpDistance = distance;
            judges = judgesArr;
            jumpPoints = points;
        }
    }

    [SerializeField]
    public JumpResultData jumpResultData;

    public SkiJumperComputer() {
        jumpResultData = new JumpResultData(0, new Judge[5], 0);
    }

    public SkiJumperComputer(string firstname, string lastname, string country) {
        this.firstname = firstname;
        this.lastname = lastname;
        this.country = country;
        jumpResultData = new JumpResultData(0, new Judge[5], 0);
    }

}
