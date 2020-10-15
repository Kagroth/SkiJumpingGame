using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="hillData", menuName="Hill Data", order=1)]
public class HillData : ScriptableObject
{
    public string hillName;
    public float kPoint;
    public float hsPoint;   

    public float pointPerMeter;
}
