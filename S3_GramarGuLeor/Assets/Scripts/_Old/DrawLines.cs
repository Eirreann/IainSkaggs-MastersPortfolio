using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLines : MonoBehaviour
{

    [SerializeField]
    private GameObject lineGeneratorPrefab;
    [SerializeField]
    private GameObject linePointPrefab;

    private bool newLine = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = 0;

            CreatePointMarker(newPos);
            newLine = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            ClearAllPoints();
        }

        //if (Input.GetKeyDown("e"))
        //{
        //    GenerateNewLine();
        //}

        if (newLine)
        {
            GenerateNewLine();
        }
    }

    private void GenerateNewLine()
    {
        GameObject[] allPoints = GameObject.FindGameObjectsWithTag("PointMarker");
        Vector3[] allPointPositions = new Vector3[allPoints.Length];

        if(allPoints.Length >= 2)
        {
            for (int i = 0; i < allPoints.Length; i++)
            {
                allPointPositions[i] = allPoints[i].transform.position;
            }

            SpawnLineGenerator(allPointPositions);
        }
        else
        {
            print("Not enough points to create a line.");
        }
    }

    private void SpawnLineGenerator(Vector3[] linePoints)
    {
        // Generating line obj from prefab and grabbing renderer
        GameObject newLineGen = Instantiate(lineGeneratorPrefab);
        LineRenderer lRend = newLineGen.GetComponent<LineRenderer>();

        lRend.positionCount = linePoints.Length;
        lRend.loop = false;

        lRend.SetPositions(linePoints);

        newLine = false;

        //Destroy(newLineGen, 5);
    }

    private void CreatePointMarker(Vector3 pointPosition)
    {
        Instantiate(linePointPrefab, pointPosition, Quaternion.identity);
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
