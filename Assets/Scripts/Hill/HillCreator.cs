#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class HillCreator : MonoBehaviour
{
    [SerializeField]
    BezierCurveCreator inrunCreator;

    [SerializeField]
    BezierCurveCreator landingSlopeCreator;

    [SerializeField]
    WindAreasCreator windAreasCreator;

    private float takeOffHeight;
    
    private float Kpoint;
    private Vector3 KpointPos;
    private float hillSizePoint;
    private Vector3 hillSizePointPos;


    // Start is called before the first frame update
    private void Awake() {
        SetNewLandingSlopePosition();  
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Transform[] inrunControlPoints = inrunCreator.GetControlPoints();

        for (int i = 0; i < inrunCreator.GetControlPointsCount(); i++) {
            if (inrunControlPoints[i].hasChanged) {
                inrunCreator.CalculateBezierCurve();
                SetNewLandingSlopePosition();
                break;
            }
        }

        Transform[] landingSlopeControlPoints = landingSlopeCreator.GetControlPoints();

        for (int i = 0; i < landingSlopeCreator.GetControlPointsCount(); i++) {
            if (landingSlopeControlPoints[i].hasChanged) {
                landingSlopeCreator.CalculateBezierCurve();
                windAreasCreator.CalculateWindAreas(landingSlopeCreator);
                break;
            }
        }
    }

    public float GetTakeOffHeight() {
        return takeOffHeight;
    }

    public void SetTakeOffHeight(float newTakeOffHeight) {
        takeOffHeight = newTakeOffHeight;
    }

    public float GetKPoint() {
        return Kpoint;
    }

    public void SetKPoint(float newKPoint) {
        Kpoint = newKPoint;
        int index = landingSlopeCreator.GetIndexOfNearestBezierPoint(Kpoint);
        KpointPos = landingSlopeCreator.GetBezierPoints()[index];
    }

    public float GetHillSizePoint() {
        return hillSizePoint;
    }

    public void SetHillSizePoint(float newHillSize) {
        hillSizePoint = newHillSize;
        int index = landingSlopeCreator.GetIndexOfNearestBezierPoint(hillSizePoint);
        hillSizePointPos = landingSlopeCreator.GetBezierPoints()[index];
    }

    public void CreateHill() {
        GameObject newHill = new GameObject();
        GameObject newHillInrun = new GameObject();
        GameObject newHillLandingSlope = new GameObject();
        GameObject newWindAreas = new GameObject();

        newHill.name = "Hill";
        newHillInrun.name = "Inrun";
        newHillLandingSlope.name = "LandingSlope";
        newHillLandingSlope.tag = "LandingSlope";
        newWindAreas.name = "WindAreas";

        newHillLandingSlope.transform.parent = newHillInrun.transform.parent = newWindAreas.transform.parent = newHill.transform;

        ConstructBezierCurveComponent(newHillInrun, inrunCreator);
        ConstructBezierCurveComponent(newHillLandingSlope, landingSlopeCreator);
        ConstructStartingPoint(newHillInrun);
        ConstructWindAreas(newWindAreas, windAreasCreator);
    }

    public BezierCurveCreator GetInrunCreator() {
        return inrunCreator;
    }

    public BezierCurveCreator GetLandingSlopeCreator() {
        return landingSlopeCreator;
    }

    public void SetNewLandingSlopePosition() {
        if (inrunCreator.GetControlPointsCount() == 0 || landingSlopeCreator.GetControlPointsCount() == 0) {
            return;
        }

        Vector3 inrunLastControlPointPosition = inrunCreator.GetControlPoints()[inrunCreator.GetControlPointsCount() - 1].position;
        Transform landingSlopeFirstControlPoint = landingSlopeCreator.GetControlPoints()[0];
        landingSlopeFirstControlPoint.position = inrunLastControlPointPosition - new Vector3(0, takeOffHeight, 0);
    }

    private void ConstructBezierCurveComponent(GameObject hillPart, BezierCurveCreator bezierCurveCreator) {
        BezierCurve bezierCurve = hillPart.AddComponent<BezierCurve>();

        bezierCurve.SetControlPoints(bezierCurveCreator.GetControlPoints());
        bezierCurve.SetBezierPoints(bezierCurveCreator.GetBezierPoints());
        bezierCurve.SetMagnitude(bezierCurveCreator.GetMagnitude());

        MeshRenderer meshRenderer = hillPart.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = hillPart.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        int upperVerticesLength = bezierCurve.GetBezierPoints().Length;  

        Vector3[] upperVertices = new Vector3[upperVerticesLength];
        Vector3[] downVertices = new Vector3[upperVerticesLength];
        Vector3[] vertices = new Vector3[upperVerticesLength * 2];

        float minYpos = bezierCurve.GetBezierPoints()
                                   .OrderBy(bezierPoint => bezierPoint.y)
                                   .First().y;
        minYpos -= takeOffHeight;

        upperVertices = bezierCurve.GetBezierPoints();

        for (int i = 0; i < upperVertices.Length; i++) {
            downVertices[i] = new Vector3(upperVertices[i].x, minYpos, 0);
            vertices[i] = upperVertices[i];
            vertices[i + upperVerticesLength] = downVertices[i];
        }

        mesh.vertices = vertices;

        int numOfTriangles = vertices.Length - 2;

        int[] triangles = new int[numOfTriangles * 3];

        int vertexIndexIterator = 0;

        for (int i = 0; i < triangles.Length / 2; i += 3) {
            triangles[i] = vertexIndexIterator;
            triangles[i + 1] = vertexIndexIterator + 1;
            triangles[i + 2] = vertexIndexIterator + upperVerticesLength;
            vertexIndexIterator++;
        }

        vertexIndexIterator = upperVerticesLength;

        for (int i = triangles.Length / 2; i < numOfTriangles * 3; i += 3) {
            triangles[i] = vertexIndexIterator;
            triangles[i + 1] = vertexIndexIterator - upperVerticesLength + 1;
            triangles[i + 2] = vertexIndexIterator + 1;
            vertexIndexIterator++;
        }

        mesh.triangles = triangles;

        mesh.uv = vertices.Select(vertice => new Vector2(vertice.x, vertice.y)).ToArray();
        meshFilter.mesh = mesh;

        ConstructComponentCollider(hillPart);
    }    

    private void ConstructComponentCollider(GameObject hillPart) {
        EdgeCollider2D collider = hillPart.AddComponent<EdgeCollider2D>();

        collider.points = hillPart.GetComponent<BezierCurve>()
                                  .GetBezierPoints()
                                  .Select(bezierPoint => new Vector2(bezierPoint.x, bezierPoint.y))
                                  .ToArray();
    
    }

    private void ConstructStartingPoint(GameObject inrun) {
        Vector3[] inrunBezierPoints = inrun.GetComponent<BezierCurve>().GetBezierPoints();
    
        int startingPointPositionIndex = inrunBezierPoints.Length / 15;

        Vector3 startingPointPosition = inrunBezierPoints[startingPointPositionIndex];

        GameObject startingPoint = new GameObject();
        
        startingPoint.name = "StartingPoint";
        startingPoint.tag = "StartingPoint";
        startingPoint.transform.parent = inrun.transform;
        startingPoint.transform.position = startingPointPosition + new Vector3(0, 0.5f, 0);
    }

    private void ConstructWindAreas(GameObject windAreasContainer, WindAreasCreator windAreasCreator) {
        GameObject[] windAreas = windAreasCreator.CreateWindAreas();

        for (int index = 0; index < windAreas.Length; index++) {
            windAreas[index].transform.parent = windAreasContainer.transform;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(KpointPos, 0.5f);
        Gizmos.DrawSphere(hillSizePointPos, 0.5f);                
    }
}
#endif