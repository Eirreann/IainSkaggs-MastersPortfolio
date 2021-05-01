using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is attached to the Starting Point from which the LineRenderer is drawn in each puzzle; each Staring Point represents a different Tense.
public class DrawPuzzleLine : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject lineGeneratorPrefab;             // The prefab object that generates the line (via LineRenderer component)
    [SerializeField]
    private GameObject linePointPrefab;                 // The prefab (LineHolder) object that represents points in the line.
    [SerializeField]
    private Transform followPoint;                      // The GameObject that the line follows (this follows the position of the mouse cursor, see FollowMouse.cs
    [SerializeField]
    private PuzzleManager puzzleManager;                // The script that manages global variables for the puzzle, e.g. win conditions, text form outputs, etc.
    private Vector3 followPointOrigin;                  // The origin position of the FollowPoint gameObject

    private Vector3 thisPos;                            // A variable to store the position of this Starting Point object
    private bool generateLine = false;                  // A boolean to trigger the generation of a new Line
    [HideInInspector]
    public bool drawingLine = false;                    // A boolean to track whether or not a line is currently being drawn
    private bool freezeLine = false;                    // A boolean to track whether or not the line drawn from this Starting Point is frozen or not (see Content Block behavious in ObstacleBehaviour.cs)

    [HideInInspector]
    public GameObject lineGen;                          // The instantiated LineHolder object
    [HideInInspector]
    public LineRenderer lRend;                          // The LineRenderer component on the lineGen object
    [HideInInspector]
    public GameObject[] allPoints;                      // An array of GameObjects to store all generated line points
    private Vector3[] allPointPositions;                // A Vector3 array to store the positions of all generated line points

    [Header("What tense am I?")]                        // Booleans to set the Tense property of this Starting Point object:
    public bool isRoot = false;
    public bool isPast = false;
    public bool isFuture = false;
    public bool isConditional = false;

    private string tense;                               // A string that stores the current Tense value
    private Color conditionalColor;                     // The colour used by the Conditional Tense
    private AudioSource thisAudio;                      // This object's AudioSource component
    #endregion

    private void Start()
    {
        thisPos = transform.position;                   // Records this object's position
        followPointOrigin = followPoint.position;       // Records the origin point of the FollowPoint object
        thisAudio = GetComponent<AudioSource>();

        conditionalColor = 
            new Color32(71, 195, 58, 255);              // Assigning the colour to match the Conditional Tense Starting Point

        // An if-statement check to record the tense of this Starting Point based on the checked boolean in the Inspector.
        if (isRoot)
            tense = "Root";
        else if (isPast)
            tense = "Past";
        else if (isFuture)
            tense = "Future";
        else if (isConditional)
            tense = "Conditional";
    }

    private void Update()
    {
        // Initiates the generation of the line when the starting point has been clicked (see OnMouseDown below).
        if (generateLine)
        {
            lineGen = Instantiate(lineGeneratorPrefab);     // Creates a new instance of the Line Generator
            lineGen.transform.parent = transform;           // Makes the Line Generator a child of this object
            lRend = lineGen.GetComponent<LineRenderer>();   // Assigns the LineRenderer compoent of the Line Generator to the lRend variable
            lRend.material.color = Color.white;             // (re)Sets the line colour to white
            puzzleManager.SendTenseInfo(tense);             // Sends the current Tense to the Puzzle Manager script
            puzzleManager.tenseActive = true;               // And tells the Puzzle Manager that there is currently an active Tense line being drawn

            drawingLine = true;                             // A line is currently being drawn
            generateLine = false;                           // A line has been successfully generated.
        }

        // After a line is generated, keep track of points as they are added to the line (through passing through intersections) as long as the line isn't being frozen by a Content Block obstacle
        if (lRend != null && !freezeLine)
        {
            // Populate an array of GameObjects with all PointMarkers in the game, then populate a Vector3 array with the positions of those points.
            allPoints = GameObject.FindGameObjectsWithTag("PointMarker");
            allPointPositions = new Vector3[allPoints.Length];

            // Go through the array of PointMarker GameObjects, make them children of this object, and add their positions to the allPointPoisitions array
            for (int i = 0; i < allPoints.Length; i++)
            {
                allPoints[i].transform.parent = transform;
                allPointPositions[i] = allPoints[i].transform.position;
            }

            // Setting the total count of points in the line; currently +1 of an array of objects that represent the points in the world to account for the FollowPoint (see below).
            lRend.positionCount = allPointPositions.Length + 1;

            // For every point in the array, set its position in the LineGenerator's Position Count.
            for(int i = 0; i < allPointPositions.Length; i++)
            {
                lRend.SetPosition(i, allPointPositions[i]);
            }

            // Then, add a final position that is the position of the FollowPoint so the line is always following the cursor
            lRend.SetPosition(lRend.positionCount-1, new Vector3(followPoint.position.x, followPoint.position.y, 0));

            // And finally, set the initial "starting point" in the FollowMouse script for reference by IntersectionBehaviour.cs
            //followPoint.GetComponent<FollowMouse>().startingPoint = transform.TransformPoint(allPointPositions[0]);

            // Based on the Tense of the line, change the line colour to match the Staring Point colour.
            if (!puzzleManager.puzzleEnded)                             //If the puzzle hasn't ended...
            {
                if (puzzleManager.rootVerb)                             // ...and if the active Tense is Root Verb...
                {
                    lRend.material.color = Color.white;                 // ...change the line colour to white.
                }
                else if (puzzleManager.pastTense)                       // If the active Tense is the Past Tense...
                {
                    lRend.material.color = Color.yellow;                // ...change the line colour to yellow.
                }
                else if (puzzleManager.futureTense)                     // If the active Tense is the Future Tense...
                {
                    lRend.material.color = Color.blue;                  // ...change the line colour to blue.
                }
                else if (puzzleManager.conditionalTense)                // If the active Tense is the Conditional Tense...
                {
                    lRend.material.color = conditionalColor;            // ...change the line colour to green.
                }
            }
        }
    }

    private void OnMouseDown()
    {
        // If a line hasn't already been generated, generate a line when the starting point is clicked
        if (!generateLine && !drawingLine && !puzzleManager.tenseActive && !freezeLine)
        {
            followPoint.gameObject.SetActive(true);                     // Activate the FollowPoint object
            Instantiate(linePointPrefab, thisPos,                       // Create a LinePoint at the Starting Point's position
                Quaternion.identity);              
            generateLine = true;                                        // Trigger the generation of a line.
            thisAudio.Play();                                           // Play the attached AudioSource clip
        }
    }

    // Called from other scripts; Creates a GameObject that represents a point on the line while the line is currently being drawn.
    public void CreatePointMarker(Vector3 pointPosition)
    {
        if (drawingLine == true)    // Check to make sure that a line is currently being drawn befor instantiating a Point Marker.
        {
            Instantiate(linePointPrefab, pointPosition, Quaternion.identity);
        }
    }

    // A method to reset the line to its starting conditions, in the event of the puzzle being restarted
    public void ResetLine()
    {
        followPoint.transform.parent = transform;                       // Reset the followPoint's parent to the starting point.
        followPoint.gameObject.SetActive(false);                        // Deactivate the FollowPoint GameObject
        followPoint.position = followPointOrigin;                       // Reset the FollowPoint's position to its origin point
        followPoint.tag = "Player";                                     // Reset the FollowPoint's tag (in the event of it being frozen - see Content Block conditions in ObstacleBehaviour.cs)
        puzzleManager.puzzleEnded = false;                              // Signal the Puzzle Manager that the puzzle has been reset
        puzzleManager.SendTenseInfo(tense);                             // Reset the tense information in the Puzzle Manager.
        ClearAllPoints();                                               // Trigger the method to delete all current points in the line
        Cursor.visible = true;                                          // Make the cursor visible again
        drawingLine = false;                                            // Indicate that a line is no longer being drawn
        freezeLine = false;                                             // Un-freeze the line (if the line is currently frozen - see Content Block conditions in ObstacleBehaviour.cs)
        followPoint.GetComponent<FollowMouse>().frozenPoint = false;    // And un-freeze the Player (if the player is currently frozen - see Content Block conditions in ObstacleBehaviour.cs)
    }

    // A method to delete all active points in the LineRenderer and delete the Line itself
    private void ClearAllPoints()
    {
        // A for-loop that goes through and destroys all Point Marker GameObjects in the allPoints array
        foreach (GameObject p in allPoints)
        {
            Destroy(p);
        }

        Destroy(lineGen);       // And finally, destroy the Line Generator object that has the LineRenderer on it
    }

    // A method called by the Content Block obstacle (see ObstacleBehaviour.cs) to freeze the currently-drawn line in place.
    public void FreezePoints()
    {
        freezeLine = true;                          // Indicate that the current line has been frozen
        drawingLine = false;                        // Indicate that a line is no longer being drawn from the Starting Point

        lineGen.tag = "oldLineHolder";              // Change the tag of the LineGenerator so that it will not be recognised by any other Starting Point

        foreach (GameObject p in allPoints)         // Destroy all Point Marker objects for the frozen line in the allPoints array (so they aren't recognised by any other lines)
        {
            Destroy(p);
        }
    }
}
