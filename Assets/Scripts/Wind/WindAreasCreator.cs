using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WindAreasCreator : MonoBehaviour
{
    [SerializeField]
    GameObject windAreaPrefab;

    [SerializeField]
    Vector3[] windAreasPositions;

    private float xSize;
    private float ySize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject[] CreateWindAreas() {
        GameObject[] windAreas  = new GameObject[windAreasPositions.Length];

        for (int index = 0; index < windAreasPositions.Length; index++) {
            windAreas[index] = Instantiate(windAreaPrefab, windAreasPositions[index] - new Vector3(xSize / 2, -ySize / 4, 0), Quaternion.identity) as GameObject;
            BoxCollider2D bc = windAreas[index].GetComponent<BoxCollider2D>();
            bc.size = new Vector2(xSize, ySize);
        }

        return windAreas;
    }

    public void CalculateWindAreas(BezierCurveCreator landingSlopeCreator) {
        float landingSlopeMagnitude = landingSlopeCreator.GetMagnitude();
        int numberOfAreas = 0;

        if (landingSlopeMagnitude > 250) {
            numberOfAreas = 5;
        }
        else {
            numberOfAreas = 3;
        }

        windAreasPositions = new Vector3[numberOfAreas];

        Vector3[] landingSlopePoints = landingSlopeCreator.GetBezierPoints();
        int currentIndex = 0;
        float targetMagnitude = 0;

        for (int index = 1; index <= numberOfAreas; index++) {
            targetMagnitude = landingSlopeMagnitude / (numberOfAreas + 1) * index;
            currentIndex = landingSlopeCreator.GetIndexOfNearestBezierPoint(targetMagnitude);
            windAreasPositions[index - 1] = landingSlopePoints[currentIndex] + Vector3.up * 5;
        }

        xSize = Vector3.Distance(windAreasPositions[1], windAreasPositions[0]);
        ySize = Mathf.Abs(landingSlopePoints[landingSlopePoints.Length - 1].y - landingSlopePoints[0].y);
    }

    private void OnDrawGizmos() {
        if (windAreasPositions == null) {
            return;
        }

        for (int index = 0; index < windAreasPositions.Length; index++) {
            Gizmos.DrawWireCube(windAreasPositions[index] + new Vector3(-10, -ySize * 0.2f, 0), new Vector3(xSize, ySize, 0));
        }
    }
}
