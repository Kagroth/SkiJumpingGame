using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpResult
{
    [SerializeField]
    public float jumpDistance;

    [SerializeField]
    public Judge[] judges;

    [SerializeField]
    public float jumpPoints;

    public JumpResult(float distance, Judge[] judgesArr, float points)
    {
        jumpDistance = distance;
        judges = judgesArr;
        jumpPoints = points;
    }
}
