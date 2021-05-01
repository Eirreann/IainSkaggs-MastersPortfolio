using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class manages the behaviour of the "Conditions" (i.e. Verb Form Pick-ups), which the player needs to collect to activate the correct Verb Form to complete each puzzle.
public class ConditionBehaviour : MonoBehaviour
{
    #region Variables
    [HideInInspector]
    public bool activated = false;                  // A bool to track whether this Condition has been activated
    private bool playerActive = false;              // A boolean to track whether this is the currently active Form collected by the Player
    private Collider2D col;                         // This Condition's box collider
    private SpriteRenderer rend;                    // This Condition's sprite renderer

    [HideInInspector]
    public Color rendCol;                           // A Color variable used to change the colour of this Condition's sprite renderer
    private PuzzleManager puzzleManager;            // The PuzzleManager script for the currently-active puzzle
    private AudioSource thisAudio;                  // This object's AudioSource component

    private Vector2 positionEntered;                // A Vector2 to record the position at which the "Player" object entered the collider at
    private Vector2 positionExited;                 // A Vector2 to record the position at which the "Player" object exited the collider at

    private FollowMouse playerScript;               // The FollowMouse script on the currently-active "Player" object
    private string directionEntered;                // A string to record the direction the "Player" object entered the collider from
    private string directionExited;                 // A string to record the direction the "Player" object entered the collider from
    private float centralOffset= 0.1f;              // A small offset value to compensate for any potential variance in the x/y position of positionEntered or positionExited

    [Header("What form am I?")]                     // Booleans to set the Form property of this Condition object:
    public bool isPositiveStatement = false;
    public bool isNegativeStatement = false;
    public bool isPositiveQuestion = false;
    public bool isNegativeQuestion = false;

    private string form;                            // A string that stores the current Form value
    #endregion

    private void Start()
    {
        col = GetComponent<Collider2D>();                                                       // Assign the Collider component
        puzzleManager = 
            GameObject.FindGameObjectWithTag("PuzzleManager").GetComponent<PuzzleManager>();    // Find and assign the currently-active PuzzleManager script

        thisAudio = GetComponent<AudioSource>();
        rend = GetComponent<SpriteRenderer>();                                                  // Assign the SpriteRenderer component
        rendCol = rend.color;                                                                   // Assign the initial colour of the SpriteRenderer

        // An if-statement check to record the Form of this Condition based on the checked boolean in the Inspector.
        if (isPositiveStatement)
            form = "PositiveStatement";
        else if (isNegativeStatement)
            form = "NegativeStatement";
        else if (isPositiveQuestion)
            form = "PositiveQuestion";
        else if (isNegativeQuestion)
            form = "NegativeQuestion";
    }

    private void Update()
    {
        if (!activated  && puzzleManager.formActive == true)   // If this object is *not* active and a Form is active elsewhere in the puzzle...
            col.isTrigger = false;                             // ...disable this object's Trigger, making it a solid collider.
        else                                                   // Otherwise...
            col.isTrigger = true;                              // ...make sure the trigger is enabled.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !activated)                         // If the Player object enters this Condition object's trigger are and this object hasn't been activated...
        {
            collision.transform.parent = transform;                         // ...make this Condition the Player's parent object...
            positionEntered = collision.transform.localPosition;            // ...record the local position of the Player as the Player object's entry position...
            FindSide(collision, "Entry");                                   // ...determine which direction the trigger area was entered from (see method below)...
            puzzleManager.SendFormInfo(form);                               // ...set this Condition's Form to be the active form in the PuzzleManager...
            puzzleManager.activateAudio = true;                             // ...play the Puzzle Manager's AudioSource (which should be the relevant verb form - see PuzzleManager)...
            ActivatePoint(true);                                            // ...and activate this Condition.
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !activated)
            thisAudio.Play();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")                              // If the Player object exits this Condition object's trigger area...
        {
            collision.transform.parent = transform;                 // ...make sure this Condition is the Player's parent object (otherwise won't re-activate with some obstacle behaviours)...
            positionExited = collision.transform.localPosition;     // ...record the local position of the player as the Player object's exit position...
            FindSide(collision, "Exit");                            // ...determine which direction the trigger area was exited from (see method below).

            if (directionExited == directionEntered)                // If the line exits the same direction it entered, deactivate this Condition object.
                ActivatePoint(false);

            if (!activated)                                         // If the point hasn't been activated (or is being *un*activated)...
                puzzleManager.SendFormInfo(form);                   // ...reset the active Form in the PuzzleManager.
        }
    }

    // This method compares the Player's local position as a child object of this GameObject to a small central offset value to determine which direction the trigger was entered/exited from.
    // (A modified version of the FindSide() method from the IntersectionBehaviour.cs script)
    private void FindSide(Collider2D collision, string direction)
    {
        if (direction == "Entry")
        {
            if (positionEntered.x < -centralOffset)
            {
                // Pointer's exiting from the LEFT side...
                directionEntered = "Left";
            }
            else if (positionEntered.x > centralOffset)
            {
                // Pointer's coming from the RIGHT side...
                directionEntered = "Right";
            }
            else if (positionEntered.y > centralOffset)
            {
                // Pointer's coming from the TOP side...
                directionEntered = "Top";
            }
            else if (positionEntered.y < -centralOffset)
            {
                // Pointer's coming from the BOTTOM side...
                directionEntered = "Bottom";
            }

            //print("Entered: " + directionEntered + "; " + positionEntered + ", " + transform.position);
        }
        else if (direction == "Exit")
        {
            if (positionExited.x < -centralOffset)
            {
                // Pointer's exiting from the LEFT side...
                directionExited = "Left";
            }
            else if (positionExited.x > centralOffset)
            {
                // Pointer's coming from the RIGHT side...
                directionExited = "Right";
            }
            else if (positionExited.y > centralOffset)
            {
                // Pointer's coming from the TOP side...
                directionExited = "Top";
            }
            else if (positionExited.y < -centralOffset)
            {
                // Pointer's coming from the BOTTOM side...
                directionExited = "Bottom";
            }

            //print("Exited: " + directionExited + "; " + positionExited + ", " + transform.position);
        }
    }

    // A method used to activate/deactivate this Condition's visual appearance and behaviours
    public void ActivatePoint(bool active)
    {
        if (active)                                 // If this Condition has been activated...
        {
            var activeCol = Color.gray;             // ...create a temporary dulled colour...
            activeCol.a = 0.5f;                     // ...that is semi-transparent...

            activated = true;                       // ...set the active status of this Conditon to true...
            rend.color = activeCol;                 // ...set the colour of the SpriteRenderer to the dulled colour...
            puzzleManager.formActive = true;        // ...tell the Puzzle Manager that a Form is currently active...
        }
        else if (!active)                           // If this Condition has be deactivated...
        {
            activated = false;                      // ...set the active status of this condition to false...
            rend.color = rendCol;                   // ...reset the colour of the SpriteRenderer to its original colour...
            puzzleManager.formActive = false;       // ...and tell the Puzzle Manager that there is not a Form currently active.
        }
    }
}
