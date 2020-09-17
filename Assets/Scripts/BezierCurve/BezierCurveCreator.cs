#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class BezierCurveCreator : MonoBehaviour
{
    [SerializeField]
    GameObject bezierCurvePrefab;

    private LineRenderer lineRenderer;

    [SerializeField]
    private Transform controlPointsContainer;

    [SerializeField]
    private Transform[] controlPoints;
    [SerializeField]
    private int controlPointsCount;

    [SerializeField]
    private float magnitude;

    [SerializeField]
    private bool renderLine = false;

    private int maxControlPointsCount = 4;
    
    private Vector3[] bezierPoints;
    
    private float t = 0.02f;
    
    int loopCount;

    public void CreateBezierCurve() {

    }
    
    public void ResetBezier() {
        maxControlPointsCount = 4;
        controlPointsCount = 0;
        magnitude = 0;
        t = 0.02f;
        loopCount = (int) (1 / t);

        controlPoints = new Transform[maxControlPointsCount];

        int childs = controlPointsContainer.transform.childCount;
        
        for (int i = childs - 1; i >= 0; i--) {
            if (Application.isEditor)
                GameObject.DestroyImmediate(controlPointsContainer.transform.GetChild(i).gameObject);
            else
                GameObject.Destroy(controlPointsContainer.transform.GetChild(i).gameObject);
        }

        bezierPoints = new Vector3[loopCount];

        lineRenderer.positionCount = 0;
    }

    void Start()
    {        
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer == null) {
            lineRenderer = gameObject.GetComponent<LineRenderer>();
        }

        for (int i = 0; i < controlPointsCount; i++) {
            if (controlPoints[i] == null) {
                return;
            }
        }       
        
        /*
        for (int i = 0; i < controlPointsCount; i++) {
            if (controlPoints[i].hasChanged) {
                CalculateBezierCurve();
                break;
            }
        }
        */
        if (renderLine) {
            if (controlPointsCount == 2) {
                lineRenderer.positionCount = controlPointsCount;

                lineRenderer.SetPosition(0, controlPoints[0].position);
                lineRenderer.SetPosition(1, controlPoints[1].position);
            }
            else if (controlPointsCount > 2){
                lineRenderer.positionCount = bezierPoints.Length;

                for (int i = 0; i < bezierPoints.Length - 1; i++) {
                    lineRenderer.SetPosition(i, bezierPoints[i]);
                }
            }            
        }    
        else {
            lineRenderer.positionCount = 0;
        }    
    }


    public void Init() {
        maxControlPointsCount = 4;
        controlPoints = new Transform[maxControlPointsCount];
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        t = 0.02f;
        loopCount = (int) (1 / t);
        bezierPoints = new Vector3[loopCount];
        
        int controlPointsCountInContainerOnStart = controlPointsContainer.transform.childCount;

        if (controlPointsCountInContainerOnStart > 4) {
            for (int i = controlPointsCountInContainerOnStart - 1; i >= 0; i--) {
                GameObject.Destroy(controlPointsContainer.transform.GetChild(i).gameObject);
            }
        }
        else {
            controlPointsCount = controlPointsCountInContainerOnStart;

            for (int i = 0; i < controlPointsCount; i++) {
                controlPoints[i] = controlPointsContainer.transform.GetChild(i).gameObject.transform;
            }
        }

        CalculateBezierCurve(); 
    }

    public int GetControlPointsCount() {
        return controlPointsCount;
    }
    public int GetMaxControlPointsCount() {
        return maxControlPointsCount;
    }

    public Transform[] GetControlPoints() {
        if (controlPoints == null) {
            Debug.Log("Tablica controlPoints nie jest zainicjalizowana");
            return null;
        }

        return controlPoints;
    }

    public Vector3[] GetBezierPoints() {
        return bezierPoints;
    }

    public float GetMagnitude() {
        return magnitude;
    }

    public GameObject CreateControlPoint() {
        GameObject newControlPoint = new GameObject();
        newControlPoint.name = "ControlPoint" + controlPointsCount;       
        newControlPoint.transform.parent = controlPointsContainer.transform;

        return newControlPoint;
    }

    public void AddControlPoint() {
        if (controlPointsCount >= maxControlPointsCount) {
            Debug.Log("Nie mozna dodac nowego punktu kontrolnego");
            return;
        }

        GameObject newControlPoint = CreateControlPoint();

        controlPoints[controlPointsCount] = newControlPoint.transform;
        controlPointsCount++;

        CalculateBezierCurve();        
    }

    public void AddControlPoint(Vector3 newControlPointPosition) {

        if (controlPointsCount >= maxControlPointsCount) {
            Debug.Log("Nie mozna dodac nowego punktu kontrolnego");
            return;            
        }

        GameObject newControlPoint = CreateControlPoint();

        newControlPoint.transform.position = newControlPointPosition;

        controlPoints[controlPointsCount] = newControlPoint.transform;
        controlPointsCount++;

        CalculateBezierCurve(); 
    }

    public void RemoveControlPoint(int index) {
        GameObject.DestroyImmediate(controlPoints[index].gameObject); 
        
        for (int i = index; i < controlPointsCount - 1; i++) {
            controlPoints[i] = controlPoints[i + 1];
        } 
        controlPoints[controlPointsCount - 1] = null;   
        controlPointsCount--;

        CalculateBezierCurve();        
    }

    public Vector3 CalculateBezierCurvePoint(float t) {
        if (controlPointsCount == 3) {
            return Mathf.Pow(1 - t, 2) * controlPoints[0].position + 2 * (1 - t) * t * controlPoints[1].position + Mathf.Pow(t, 2) * controlPoints[2].position; 
        }
        else if (controlPointsCount == 4) {
            return Mathf.Pow(1 - t, 3) * controlPoints[0].position + 3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position + 3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position + Mathf.Pow(t, 3) * controlPoints[3].position;
        }
        else {
            return Vector3.zero;
        }
    }

    public void CalculateBezierCurve() {
        bezierPoints = new Vector3[loopCount];
        
        if (controlPointsCount < 2) {
            return;
        }

        if (controlPointsCount > 2) {
            float ttime = 0;
            int index = 0;

            while (ttime <= 1 && index < loopCount) {
                bezierPoints[index] = CalculateBezierCurvePoint(ttime);
                index++;   
                ttime += t;             
            }
        }

        CalculateBezierCurveMagnitude();
        if (lineRenderer) lineRenderer.SetPositions(bezierPoints);
    }

    public void CalculateBezierCurveMagnitude() {
        if (controlPointsCount == 2) {
            magnitude = Vector3.Magnitude(controlPoints[1].position - controlPoints[0].position);
        }
        else if (controlPointsCount > 2){
            magnitude = 0;
            for (int i = 0; i < bezierPoints.Length - 1; i++) {
                magnitude += Vector3.Magnitude(bezierPoints[i + 1] - bezierPoints[i]);
            }
        }
        else {
            magnitude = 0;
        }
    }

    public int GetIndexOfNearestBezierPoint(float magnitudeToReach) {
        float mag = 0;
        int indexToFind = 0;

        for (int index = 0; index < bezierPoints.Length - 1; index++) {
            mag += Vector3.Magnitude(bezierPoints[index + 1] - bezierPoints[index]);

            if (mag >= magnitudeToReach) {
                indexToFind = index + 1;
                break;
            } 
        }

        return indexToFind;
    }

    private void OnDrawGizmosHandler(bool selected) {
        if (controlPointsCount < 2) {
            return;
        }

        int controlPointsNumber = controlPointsContainer.transform.childCount;
        Vector3[] gizmosBezierPoints = new Vector3[loopCount];

        for (int i = 0; i < controlPointsNumber; i++) {            
            if (controlPointsContainer.transform.GetChild(i).hasChanged) {
                if (controlPointsNumber > 2) {
                    float ttime = 0;
                    int index = 0;

                    while (ttime <= 1 && index < loopCount) {
                        gizmosBezierPoints[index] = CalculateBezierCurvePoint(ttime);
                        index++;   
                        ttime += t;             
                    }
                }

                break;
            }
        }

        CalculateBezierCurveMagnitude();

        Color color = new Color();

        if (selected) {
            Gizmos.color = color = new Color(0.4f, 0, 1f);
        }
        else {
            Gizmos.color = color = new Color(0, 0.4f, 0.8f);
        }

        GameObject selectedObj = Selection.activeGameObject;

        foreach (Transform controlPoint in controlPoints) {
            if (controlPoint != null) {
                if (selectedObj != null && selectedObj.Equals(controlPoint.gameObject)) {
                    Gizmos.color = new Color(0.4f, 0, 1f);
                }

                Gizmos.DrawSphere(controlPoint.transform.position, 0.5f);
                
                Gizmos.color = color;
            }
        }        

        Gizmos.color = new Color(0, 0.4f, 0.8f);

        if (controlPointsCount == 2) {
            Gizmos.DrawLine(controlPoints[0].position, controlPoints[1].position);
        }
        else {
            for (int i = 1; i < loopCount; i++) {
                Gizmos.DrawLine(gizmosBezierPoints[i - 1], gizmosBezierPoints[i]);
            }
        }
    }

    private void OnDrawGizmos() {
        OnDrawGizmosHandler(false);      
    }

    private void OnDrawGizmosSelected() {
        OnDrawGizmosHandler(true);      
    }
}
#endif