using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiJumperComputer
{
    public struct JumpResultData {
        public float jumpDistance;
        public Judge[] judges;
        public float jumpPoints;

        public JumpResultData(float distance, Judge[] judgesArr, float points) {
            jumpDistance = distance;
            judges = judgesArr;
            jumpPoints = points;
        }
    }

    public JumpResultData jumpResultData;


}
