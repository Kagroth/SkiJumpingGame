using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class BezierCurveMesh : MonoBehaviour
{
    [SerializeField]
    GameObject inrun;

    [SerializeField]
    Vector3[] downVertices;


    [SerializeField]
    int[] triangles;


    [SerializeField]
    GameObject landingSlope;

    [SerializeField]
    Vector3[] landingSlopeDownVertices;

    int[] landingSlopeTriangles;
    
    // Start is called before the first frame update
    void Start()
    {
        RenderHill();
    }

    // Update is called once per frame
    void Reset() {
        RenderHill();
    }

    public void RenderHill() {
        RenderInrun();
        RenderLandingSlope();
    }

    public void RenderInrun() {
        MeshRenderer meshRenderer = inrun.GetComponent<MeshRenderer>();
        MeshFilter meshFilter = inrun.GetComponent<MeshFilter>();
        BezierCurve inrunShape = inrun.GetComponentInChildren<BezierCurve>();
        Vector3[] bezierPoints = inrunShape.GetBezierPoints();
        int upperVerticesLength = bezierPoints.Length;
        
        Mesh mesh = new Mesh();

        Vector3[] upperVertices = new Vector3[upperVerticesLength];
        downVertices = new Vector3[upperVerticesLength];
        Vector3[] vertices = new Vector3[upperVerticesLength * 2];

        float minYpos = inrunShape.GetControlPoints()
                                   .Where(controlPoint => controlPoint != null)
                                   .Min(controlPoint => controlPoint.position.y);

        minYpos -= 2;

        for (int i = 0; i < upperVertices.Length; i++) {
            upperVertices[i] = bezierPoints[i];
            downVertices[i] = new Vector3(bezierPoints[i].x, minYpos, 0);
            vertices[i] = upperVertices[i];
            vertices[i + upperVerticesLength] = downVertices[i];
        }

        mesh.vertices = vertices;

        int numOfTriangles = vertices.Length - 2;

        triangles = new int[numOfTriangles * 3];

        int vertexIndexIterator = 0;

        for (int i = 0; i < triangles.Length / 2; i += 3) {
            triangles[i] = vertexIndexIterator;
            triangles[i + 1] = vertexIndexIterator + 1;
            triangles[i + 2] = vertexIndexIterator + upperVerticesLength;
            vertexIndexIterator += 1;
        }

        vertexIndexIterator = upperVerticesLength;

        for (int i = triangles.Length / 2; i < numOfTriangles * 3; i += 3) {
            triangles[i] = vertexIndexIterator;
            triangles[i + 1] = vertexIndexIterator + 1;
            triangles[i + 2] = vertexIndexIterator - upperVerticesLength + 1;
            vertexIndexIterator += 1;
        }

        mesh.triangles = triangles;

        mesh.uv = vertices.Select(vertice => new Vector2(vertice.x, vertice.y)).ToArray();
        meshFilter.mesh = mesh;
    }

    public void RenderLandingSlope() {
        MeshRenderer meshRenderer = landingSlope.GetComponent<MeshRenderer>();
        MeshFilter meshFilter = landingSlope.GetComponent<MeshFilter>();
        BezierCurve landingSlopeShape = landingSlope.GetComponentInChildren<BezierCurve>();
        Vector3[] bezierPoints = landingSlopeShape.GetBezierPoints();
        int upperVerticesLength = bezierPoints.Length;
        
        Mesh mesh = new Mesh();

        Vector3[] upperVertices = new Vector3[upperVerticesLength];
        landingSlopeDownVertices = new Vector3[upperVerticesLength];
        Vector3[] vertices = new Vector3[upperVerticesLength * 2];

        float minYpos = landingSlopeShape.GetControlPoints()
                                   .Where(controlPoint => controlPoint != null)
                                   .Min(controlPoint => controlPoint.position.y);

        minYpos -= 2;

        for (int i = 0; i < upperVertices.Length; i++) {
            upperVertices[i] = bezierPoints[i];
            landingSlopeDownVertices[i] = new Vector3(bezierPoints[i].x, minYpos, 0);
            vertices[i] = upperVertices[i];
            vertices[i + upperVerticesLength] = landingSlopeDownVertices[i];
        }

        mesh.vertices = vertices;

        int numOfTriangles = vertices.Length - 2;

        landingSlopeTriangles = new int[numOfTriangles * 3];

        int vertexIndexIterator = 0;

        for (int i = 0; i < landingSlopeTriangles.Length / 2; i += 3) {
            landingSlopeTriangles[i] = vertexIndexIterator;
            landingSlopeTriangles[i + 1] = vertexIndexIterator + 1;
            landingSlopeTriangles[i + 2] = vertexIndexIterator + upperVerticesLength;
            vertexIndexIterator += 1;
        }

        vertexIndexIterator = upperVerticesLength;

        for (int i = landingSlopeTriangles.Length / 2; i < numOfTriangles * 3; i += 3) {
            landingSlopeTriangles[i] = vertexIndexIterator;
            landingSlopeTriangles[i + 1] = vertexIndexIterator + 1;
            landingSlopeTriangles[i + 2] = vertexIndexIterator - upperVerticesLength + 1;
            vertexIndexIterator += 1;
        }

        mesh.triangles = landingSlopeTriangles;

        mesh.uv = vertices.Select(vertice => new Vector2(vertice.x, vertice.y)).ToArray();
        meshFilter.mesh = mesh;
    }
}
