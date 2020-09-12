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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        minYpos -= 2;

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
