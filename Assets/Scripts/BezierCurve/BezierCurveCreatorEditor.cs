using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurveCreator))]
public class BezierCurveEditor : Editor
{
    public override void OnInspectorGUI() {
        serializedObject.Update();
        BezierCurveCreator bezierCurveCreator = (BezierCurveCreator) target;
        DrawDefaultInspector();

        /*

        if (GUILayout.Button("Dodaj punkt kontrolny")) {
            bezierCurve.AddControlPoint();
        }      
        
        if (GUILayout.Button("Reset")) {
            bezierCurve.ResetBezier();
        }
        */

        Transform[] controlPoints = bezierCurveCreator.GetControlPoints();
        
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Control points");

        for (int i = 0; i < controlPoints.Length; i++) {
            if (controlPoints[i] == null) {
                continue;
            }

            EditorGUILayout.BeginHorizontal();
            float x = EditorGUILayout.FloatField("X", controlPoints[i].position.x);
            float y = EditorGUILayout.FloatField("Y", controlPoints[i].position.y);    
            EditorGUILayout.EndHorizontal();
            
            controlPoints[i].position = new Vector3(x, y, 0);

            if (GUILayout.Button("Usuń")) {
                bezierCurveCreator.RemoveControlPoint(i);
            }
        }
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}