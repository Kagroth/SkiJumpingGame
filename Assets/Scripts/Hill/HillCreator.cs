using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class HillCreator : MonoBehaviour
{
    [SerializeField]
    GameObject hillTarget;

    [SerializeField]
    BezierCurveCreator inrunCreator;

    [SerializeField]
    BezierCurveCreator landingSlopeCreator;
    private float takeOffHeight;
    
    private float Kpoint;
    private float hillSizePoint;

    // Start is called before the first frame update
    void Start()
    {
        if (inrunCreator.GetControlPointsCount() < 3 && landingSlopeCreator.GetControlPointsCount() < 3) {
            return;
        }
        
        Vector3 inrunLastControlPointPosition = inrunCreator.GetControlPoints()[inrunCreator.GetControlPointsCount() - 1].position;
        Vector3 landingSlopeFirstControlPointPosition = landingSlopeCreator.GetControlPoints()[0].position;
        takeOffHeight = (inrunLastControlPointPosition - landingSlopeFirstControlPointPosition).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public float GetHillSizePoint() {
        return hillSizePoint;
    }

    public void SetHillSizePoint(float newHillSize) {
        hillSizePoint = newHillSize;
    }

    public void CreateHill() {
        GameObject newHill = new GameObject();
        GameObject newHillInrun = new GameObject();
        GameObject newHillLandingSlope = new GameObject();

        newHill.name = "Hill";
        newHillInrun.name = "Inrun";
        newHillLandingSlope.name = "LandingSlope";
        newHillLandingSlope.tag = "LandingSlope";

        newHillLandingSlope.transform.parent = newHillInrun.transform.parent = newHill.transform;

        ConstructBezierCurveComponent(newHillInrun, inrunCreator);
        ConstructBezierCurveComponent(newHillLandingSlope, landingSlopeCreator);
        ConstructStartingPoint(newHillInrun);
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
        Debug.Log(startingPointPosition);
    }
}
