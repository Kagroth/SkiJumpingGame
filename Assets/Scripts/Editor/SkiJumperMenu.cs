using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SkiJumperMenu
{
    [MenuItem("Ski Jumper/Generate ski jumpers file")]
    public static void GenerateSkiJumpers() {
        SkiJumperDatabase.GenerateSkiJumpersFile();
    }
}
