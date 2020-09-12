using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurveMesh))]
public class NewBehaviourScript : Editor
{
    public override void OnInspectorGUI() {
        serializedObject.Update();
        DrawDefaultInspector();

        BezierCurveMesh bcm = (BezierCurveMesh) target;

        if (GUILayout.Button("Utwórz Mesh")) {
            bcm.RenderHill();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
