using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineFromPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject lineGeneratorPrefab;
    [SerializeField]
    private GameObject linePointPrefab;
    [SerializeField]
    private GameObject followPoint;

    private Vector3 thisPos;
    private Vector3 followPointPos;
    private bool generateLine = false;
    private bool drawingLine = false;

    private GameObject newLineGen;
    private LineRenderer lRend;

    private void Start()
    {
        thisPos = transform.position;
    }

    private void Update()
    {
        followPointPos = followPoint.transform.position;

        // Create a new point in the line when the mouse is clicked after the puzzle has been started.
        if (Input.GetMouseButtonDown(0) && drawingLine)
        {
            CreatePointMarker(lRend.GetPosition(1));
        }

        // Initiates the generation of the line when the starting point has been clicked.
        if (generateLine == true)
        {
            newLineGen = Instantiate(lineGeneratorPrefab);
            lRend = newLineGen.GetComponent<LineRenderer>();

            drawingLine = true;
            generateLine = false;
        }

        // After a line is generated, follow the mouse position on the X axis
        if(lRend != null)
        {
            lRend.SetPosition(0, thisPos);
            lRend.SetPosition(1, new Vector3(followPointPos.x, followPointPos.y, 0));
        }

        //print(followPointPos);
    }

    private void OnMouseDown()
    {
        // If a line hasn't already been generated, generate a line on mouse click
        if (generateLine == false)
        {
            generateLine = true;

            followPoint.SetActive(true);
        }
    }

    // Not currently being used, but may come back to?
    private void GenerateNewLine()
    {
        GameObject[] allPoints = GameObject.FindGameObjectsWithTag("PointMarker");
        Vector3[] allPointPositions = new Vector3[allPoints.Length];

        if (allPoints.Length >= 2)
        {
            for (int i = 0; i < allPoints.Length; i++)
            {
                allPointPositions[i] = allPoints[i].transform.position;
            }

            //SpawnLineGenerator(allPointPositions);
        }
        else
        {
            print("Not enough points to create a line.");
        }
    }

    // Not currently being used, but may come back to?
    private void SpawnLineGenerator()
    {
        // Generating line obj from prefab and grabbing renderer
        GameObject newLineGen = Instantiate(lineGeneratorPrefab);
        LineRenderer lRend = newLineGen.GetComponent<LineRenderer>();

        //lRend.positionCount = linePoints.Length;

        //lRend.SetPositions(linePoints);
    }

    private void CreatePointMarker(Vector3 pointPosition)
    {
        if (drawingLine == true)
        {
            Instantiate(linePointPrefab, pointPosition, Quaternion.identity);
        }
    }

    private void ClearAllPoints()
    {
        GameObject[] allPoints = GameObject.FindGameObjectsWithTag("PointMarker");

        foreach (GameObject p in allPoints)
        {
            Destroy(p);
        }
    }
}
