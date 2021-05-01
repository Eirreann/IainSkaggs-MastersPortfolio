using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class governs the behaviour of the intersection points in the puzzle grid, so that the LineRenderer is drawn and connected between intersections and can be un-drawn if the cursor is moved back the way it came.
public class IntersectionBehaviour : MonoBehaviour
{
    #region Variables
    private bool activated = false;                 // A bool to track if this intersection has been activated (passed through) or not
    private DrawPuzzleLine lineScript;              // The DrawPuzzleLine script from the starting point
    private PuzzleManager puzzleManager;            // The PuzzleManager script for the currently-active puzzle

    private int currentPoint;                       // An int value to track which point in the LineRenderer array the current point is
    //private Vector2 previousPoint;                // A Vector2 to track the previous intersection point (obsolete)
    private BoxCollider2D col;                      // This intersection point's box collider

    private Vector2 positionEntered;                // A Vector2 to record the position at which the "Player" object entered the collider at
    private Vector2 positionExited;                 // A Vector2 to record the position at which the "Player" object exited the collider at

    private string directionEntered;                // A string to record the direction the "Player" entered the collider from
    private string directionExited;                 // A string to record the direction the "Player" exited the collider from
    private float centralOffset = 0.2f;             // A small offset value to compensate for any potential variance in the x/y position of positionEntered or positionExited
    #endregion

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();                                                    // Assigning the Box Collider component
        puzzleManager = 
            GameObject.FindGameObjectWithTag("PuzzleManager").GetComponent<PuzzleManager>();    // Finding and assigning the currently active PuzzleManager script
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !activated)                         // If the player enters the intersection and this point hasn't been activated yet...
        {
            lineScript = collision.GetComponent<FollowMouse>().lineScript;  // ...grab the LineScript component from the player...

            positionEntered = 
                transform.TransformPoint(collision.transform.position);     // ...record the position the player entered at...

            lineScript.CreatePointMarker(transform.position);               // ...create a line point at the intersection (see DrawPuzzleLine.cs)...
            currentPoint = lineScript.lRend.positionCount - 1;              // ...acquire the index of the current point in the LineRenderer's positions array (-1 to account for the position currently following the mouse)...

            FindSide(collision, "Entry");                                   // ...determine which direction the intersection was entered from...
            activated = true;                                               // ...and activate this intersection.
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")                                       // If the player leaves the intersection...
        {
            positionExited = 
                transform.TransformPoint(collision.transform.position);     // ...record the position the player exited at...
            FindSide(collision, "Exit");                                    // ...and determine which direction the intersection was exited from.

            if (directionExited == directionEntered)                        // ...if the direction exited is the same as the direction entered...
            {
                activated = false;                                          // ...de-activate this point.
            }

            if (activated)                                                  // If the player exits the intersection after it has been activated...
            {
                col.isTrigger = false;                                      // ...turn off the trigger so that the intersection becomes a collider.
            }
            else if (!activated)                                            // Otherwise, if the point has been deactivated (e.g. exited the same direction entered)...
            {
                Destroy(lineScript.allPoints[currentPoint]);                // ...destroy the point marker, and by extention, this point in the LineRenderer's positions array.
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            if (lineScript.lRend.positionCount > currentPoint + 2)          // If the player collides with this intersection and it is *not* the last intersection passed through...
            {
                return;                                                     // ...keep being a collider.
            }
            else if (lineScript.lRend.positionCount <= currentPoint + 2)    // But if it *is* the last intersection point passed through...
            {
                col.isTrigger = true;                                       // ...re-enable the trigger so the player can pass through.
            }            
        }        
    }

    // A method to determine which direction the trigger was entered/exited from, by contrasting the entry/exit position with the 
    // centre point of the intersection +/- a small offset to account for any inconsistencies with the player's relative x/y position
    private void FindSide(Collider2D collision, string direction)
    {
        Vector2 thisPoint = transform.TransformPoint(transform.position);   // First, grab the intersection's position in world space...

        // Then, compare that position +/- central offset with the position the player entered the trigger from.
        if (direction == "Entry")
        {
            // To determine direction entered from:
            if (positionEntered.x < (thisPoint.x - centralOffset))
            {
                // Pointer's entering from the LEFT side...
                directionEntered = "Left";
            }
            else if (positionEntered.x > (thisPoint.x + centralOffset))
            {
                // Pointer's entering from the RIGHT side...
                directionEntered = "Right";
            }
            else if (positionEntered.y > (thisPoint.y + centralOffset))
            {
                // Pointer's entering from the TOP side...
                directionEntered = "Top";
            }
            else if (positionEntered.y < (thisPoint.y - centralOffset))
            {
                // Pointer's entering from the BOTTOM side...
                directionEntered = "Bottom";
            }

            //print("Entered: " + directionEntered);
        }
        else if (direction == "Exit")
        {
            if (positionExited.x < (thisPoint.x - centralOffset))
            {
                // Pointer's exiting from the LEFT side...
                directionExited = "Left";
            }
            else if (positionExited.x > (thisPoint.x + centralOffset))
            {
                // Pointer's exiting from the RIGHT side...
                directionExited = "Right";
            }
            else if (positionExited.y > (thisPoint.y + centralOffset))
            {
                // Pointer's exiting from the TOP side...
                directionExited = "Top";
            }
            else if (positionExited.y < (thisPoint.y - centralOffset))
            {
                // Pointer's exiting from the BOTTOM side...
                directionExited = "Bottom";
            }

            //print("Exited: " + directionExited);
        }
    }

    // A method to reset the collider trigger and activated status (also used by PuzzleManager's ResetPuzzle() method)
    public void ResetBehaviour()
    {
        col.isTrigger = true;
        activated = false;
    }



    // Old code that I wasted waaaaaay too much time trying to bugfix. >.<
    //playerScript = collision.GetComponent<FollowMouse>();
    //// To determine direction entered from:
    //if (playerScript.previousPoint.x < thisPoint.x)
    //{
    //    // Pointer's coming from the LEFT side...
    //    directionEntered = "Left";
    //}
    //else if (playerScript.previousPoint.x > thisPoint.x)
    //{
    //    // Pointer's coming from the RIGHT side...
    //    directionEntered = "Right";
    //}
    //else if (playerScript.previousPoint.y > thisPoint.y)
    //{
    //    // Pointer's coming from the TOP side...
    //    directionEntered = "Top";
    //}
    //else if (playerScript.previousPoint.y < thisPoint.y)
    //{
    //    // Pointer's coming from the BOTTOM side...
    //    directionEntered = "Bottom";
    //}
}
