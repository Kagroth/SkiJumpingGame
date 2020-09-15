using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HillCreator))]
public class HillCreatorEditor : Editor
{
    public override void OnInspectorGUI() {
        serializedObject.Update();
        HillCreator hillCreator = (HillCreator) target;

        DrawDefaultInspector();

        float takeOffHeight = EditorGUILayout.FloatField("Take-off Height", hillCreator.GetTakeOffHeight());
        hillCreator.SetTakeOffHeight(takeOffHeight);
        hillCreator.SetNewLandingSlopePosition();

        float Kpoint = EditorGUILayout.FloatField("K point", hillCreator.GetKPoint());
        Kpoint = Mathf.Clamp(Kpoint, 0, hillCreator.GetLandingSlopeCreator().GetMagnitude());
        hillCreator.SetKPoint(Kpoint);

        float hillSizePoint = EditorGUILayout.FloatField("Hill Size point", hillCreator.GetHillSizePoint());
        hillSizePoint = Mathf.Clamp(hillSizePoint, Kpoint, hillCreator.GetLandingSlopeCreator().GetMagnitude());
        hillCreator.SetHillSizePoint(hillSizePoint);

        if(GUILayout.Button("Create hill")) {
            hillCreator.CreateHill();
        }

        // Inrun section
        EditorGUILayout.BeginVertical();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Inrun:");
        EditorGUILayout.LabelField("Inrun control points");

        BezierCurveCreator inrunCreator = hillCreator.GetInrunCreator();

        Transform[] inrunCreatorControlPoints = inrunCreator.GetControlPoints();
        
        if (GUILayout.Button("Add new control point")) {
            inrunCreator.AddControlPoint();
        }      
        
        if (GUILayout.Button("Reset")) {
            inrunCreator.ResetBezier();
        }
        
        EditorGUILayout.Space();

        for(int i = 0; i < inrunCreatorControlPoints.Length; i++) {
            if (inrunCreatorControlPoints[i] == null) {
                continue;
            }

            EditorGUILayout.BeginHorizontal();
            float x = EditorGUILayout.FloatField("X", inrunCreatorControlPoints[i].position.x);
            float y = EditorGUILayout.FloatField("Y", inrunCreatorControlPoints[i].position.y);    
            EditorGUILayout.EndHorizontal();
            
            inrunCreatorControlPoints[i].position = new Vector3(x, y, 0);

            if (GUILayout.Button("Remove")) {
                inrunCreator.RemoveControlPoint(i);
            }
        }

        EditorGUILayout.FloatField("Magnitude", inrunCreator.GetMagnitude());

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // Landing slope section
        EditorGUILayout.LabelField("Landing slope:");
        EditorGUILayout.LabelField("Landing slope control points");
        
        BezierCurveCreator landingSlopeCreator = hillCreator.GetLandingSlopeCreator();

        Transform[] landingSlopeCreatorControlPoints = landingSlopeCreator.GetControlPoints();
        
        if (GUILayout.Button("Add new control point")) {
            landingSlopeCreator.AddControlPoint();
        }      
        
        if (GUILayout.Button("Reset")) {
            landingSlopeCreator.ResetBezier();
        }
        
        EditorGUILayout.Space();

        for(int i = 0; i < landingSlopeCreatorControlPoints.Length; i++) {
            if (landingSlopeCreatorControlPoints[i] == null) {
                continue;
            }

            EditorGUILayout.BeginHorizontal();
            float x = EditorGUILayout.FloatField("X", landingSlopeCreatorControlPoints[i].position.x);
            float y = EditorGUILayout.FloatField("Y", landingSlopeCreatorControlPoints[i].position.y);    
            EditorGUILayout.EndHorizontal();
            
            landingSlopeCreatorControlPoints[i].position = new Vector3(x, y, 0);

            if (GUILayout.Button("Remove")) {
                landingSlopeCreator.RemoveControlPoint(i);
            }
        }
        
        EditorGUILayout.FloatField("Magnitude", landingSlopeCreator.GetMagnitude());
        
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
