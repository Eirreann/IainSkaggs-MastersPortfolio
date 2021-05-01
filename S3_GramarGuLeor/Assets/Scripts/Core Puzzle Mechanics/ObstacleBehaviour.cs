using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This class manages the behavour of all Obstacle types, with specifically tailored conditions for each Obstacle as well as general features true across the board (such as active states)
public class ObstacleBehaviour : MonoBehaviour
{
    #region Variables
    private bool activated = false;                 // A boolean to track if this obstacle has been activated (passed through) or not
    private Collider2D col;                         // This object's collider
    private SpriteRenderer rend;                    // This object's SpriteRenderer
    private Color rendCol;                          // This object's starting SpriteRenderer colour
    private PuzzleManager puzzleManager;            // The PuzzleManager of the currently-active puzzle level
    private AudioSource thisAudio;                  // This object's Audio Source component
    private AudioClip startingAudio;                // The initial audio clip in the Audio Source

    private Vector2 positionEntered;                // A Vector2 to record the position the player entered the trigger area from
    private Vector2 positionExited;                 // A Vector2 to record the position the player exited the trigger area from

    private FollowMouse playerScript;               // The FollowMouse script attached to the currently active "Player" object
    private DrawPuzzleLine lineScript;              // The DrawPuzzleLine script attached to the currently active Starting Point (Tense)
    private string directionEntered;                // A string that records the direction that the "Player" entered the collider from
    private string directionExited;                 // A string that records the direction that the "Player" exited the collider from
    private float centralOffset = 0.1f;             // A small offset value to compensate for any potential variance in the x/y position of positionEntered or positionExited

    [Header("What kind of obstacle?")]              // Booleans to assign the variety of Obstacle that this object is:
    public bool timeVortex = false;
    public bool contentBlock = false;
    public bool confusifier = false;

    [Header("Content Block Solution Tense")]        // Booleans to set the required Tense to break this obstacle if it is a Content Block:
    [SerializeField]
    private bool root = false;
    [SerializeField]
    private bool pastTense = false;
    [SerializeField]
    private bool futureTense = false;
    [SerializeField]
    private bool conditionalTense = false;
    [SerializeField]
    private bool noTense = false;

    [Header("Content Block Solution Form")]         // Booleans to set the required Form to break this obstacle if it is a Content Block:
    [SerializeField]
    private bool positiveStatement = false;
    [SerializeField]
    private bool positiveQuestion = false;
    [SerializeField]
    private bool negativeStatement = false;
    [SerializeField]
    private bool negativeQuestion = false;
    [SerializeField]
    private bool noForm = false;

    [Header("Content Block Solution Sentence")]
    [SerializeField]
    [TextArea(1, 5)]
    private string solutionSentenceGD;              // The Content Block's solution sentence in Gàidhlig
    [SerializeField]
    [TextArea(1, 5)]
    private string solutionSentenceEN;              // The Content Block's solution sentence in English
    [SerializeField]
    private AudioClip solutionAudio;                // The Content Block's solution sentence audio
    [SerializeField]
    private GameObject popupPrefab;                 // The popup prefab that displays the Content Block's solution sentences
    private GameObject popupPrompt;                 // The instance of the popup prefab generated in the event of an OnMouseOver

    private bool correctTense = false;              // A boolean to track whether or not the correct Tense to break this Content Block is currently active
    private bool correctForm = false;               // A boolean to track whether or not the correct Form to break this Content Block is currently active

    private string initialTense;                    // A string to record the initial tense for the Time Vortex obstacle
    private string initialForm;                     // A string to record the initial form for the Confusifier obstacle
    #endregion

    private void Start()
    {
        col = GetComponent<Collider2D>();                                                       // Assigning this object's collider
        puzzleManager = 
            GameObject.FindGameObjectWithTag("PuzzleManager").GetComponent<PuzzleManager>();    // Assigning the currently active PuzzleManager script

        rend = GetComponent<SpriteRenderer>();                                                  // Assigning the object's SpriteRenderer
        rendCol = rend.color;                                                                   // Assigning the initial colour of the SpriteRenderer
        thisAudio = GetComponent<AudioSource>();                                                // Assigning the AudioSource component
        startingAudio = thisAudio.clip;                                                         // Setting the initial audio clip
    }

    private void Update()
    {
        if (contentBlock && puzzleManager.tenseActive)                                          // If this Obstacle is a Content Block and a Tense is currently active...
        {
            GameObject[] drawLine = GameObject.FindGameObjectsWithTag("Tense");                 // Fill an array with all Tenses (Starting Points) in the level
            for (var i = 0; i < drawLine.Length; i++)
            {
                if (drawLine[i].GetComponent<DrawPuzzleLine>().drawingLine)                     // If a Starting Point is drawing a line...
                {
                    lineScript = drawLine[i].GetComponent<DrawPuzzleLine>();                    // ...assign it to the lineScript variable.
                }
            }

            Destroy(popupPrompt);                                                               // ...and destroy the popup prompt (if one is currently active).
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !activated)                                            // If the Player object enters the Obstacle and this point hasn't been activated yet...
        {
            if (timeVortex || confusifier)                                                      // If this obstacle is a Time Vortex or a Confusifier...
            {
                collision.transform.parent = transform;                                         // ...make this object the parent of the Player object...
                positionEntered = collision.transform.localPosition;                            // ...record the position the Player entered the trigger from...
                FindSide(collision, "Entry");                                                   // ...determine which direction the Obstacle was entered from...
                ActivatePoint(true);                                                            // ...and activate this Obstacle.

                if (activated)                                                                  // If this Obstacle is activated...
                {
                    if (timeVortex)                                                             // If it is a Time Vortex...
                    {
                        initialTense = puzzleManager.currentTense;                              // ...record the currently active Tense...
                        puzzleManager.TenseReset();                                             // ...reset the active Tense from the PuzzleManager...
                        puzzleManager.SendTenseInfo("Root");                                    // ...set the active Tense to Verb Root...
                        if(puzzleManager.formActive && !puzzleManager.positiveQuestion 
                            && !puzzleManager.negativeQuestion)                                 // ...and if there's an active Verb Form that isn't a positive or negative question...
                            puzzleManager.activateAudio = true;                                 // ...activate the audio clip for the currently active Verb Form.
                    }
                    else if (confusifier)                                                       // Otherwise, if it is a Confusifier...
                    {
                        initialForm = puzzleManager.currentForm;                                // ...record the currently active Form...
                        puzzleManager.FormReset();                                              // ...and reset the active Form from the PuzzleManager.
                        thisAudio.Play();
                    }
                }                
            }

            if (contentBlock)                                                                   // If this Obstacle is a Content Block...
            {
                playerScript = collision.GetComponent<FollowMouse>();                           // ...grab the currently active FollowMouse script from the Player object...
                CheckContentBlock();                                                            // ...and call the CheckContentBlock() method to see if the play has broken the block.
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && contentBlock)                               // If the player enters this Collider and it is a ContentBlock
        {
            playerScript = collision.gameObject.GetComponent<FollowMouse>();                    // ...grab the currently active FollowMouse script from the Player object...
            CheckContentBlock();                                                                // ...and call the CheckContentBlock() method to see if the play has broken the block.
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            col.isTrigger = true;                                                               // Reset the trigger on the collider if the player exits the collider.
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")                                                          // If the Player object exits the Obstacle...
        {
            positionExited = collision.transform.localPosition;                                 // ...record the position the Player exited the trigger from...
            FindSide(collision, "Exit");                                                        // ...determine which direction the Obstacle was exited from.

            if (directionExited == directionEntered)                                            // If the Player object exits the same direction that it entered...
            {
                if (timeVortex || confusifier)                                                  // ...and if this Obstacle is a Time Vortex or a Confusifier...
                {
                    ActivatePoint(false);                                                       // ...deactivate this Obstacle.
                }
            }

            if (!activated)                                                                     // If the point has been *un*activated...
            {
                if (timeVortex)                                                                 // ...and if it is a Time Vortex...
                {
                    puzzleManager.TenseReset();                                                 // ...reset the active Tense...
                    puzzleManager.SendTenseInfo(initialTense);                                  // ...and set the initial Tense as the active tense.
                    puzzleManager.activateAudio = true;                                         // ...activate the audio clip for the currently active Verb Form.
                }
                else if (confusifier)                                                           // Else if it is a Confusifier...
                {
                    puzzleManager.FormReset();                                                  // ...reset the active Form...
                    puzzleManager.SendFormInfo(initialForm);                                    // ...and set the initial Form as the active Form.
                    puzzleManager.activateAudio = true;                                         // ...activate the audio clip for the currently active Verb Form.
                }
            }
        }
    }

    private void OnMouseEnter()                                                                 // If the mouse enters this object's collider...
    {
        if (contentBlock && lineScript == null || contentBlock && !lineScript.drawingLine)      // ...and this Obstacle is a Content Block and there is no line being drawn...
        {
            if (popupPrefab != null && popupPrompt == null)                                     // ...*and* if there is a popup prefab assigned that hasn't been spawned yet...
            {
                popupPrompt = Instantiate(popupPrefab, transform);                              // ...instantiate a new popup and assign it to the popupPrompt variable...
                popupPrompt.GetComponentInChildren<SpriteRenderer>().color = rend.color;        // ...set the colour of the popup prompt to this SpriteRenderer's colour...
                popupPrompt.transform.localPosition = new Vector3(4, 3, 0);                     // ...set the popup prompt's position to slightly above and to the right of this object's position...
                popupPrompt.GetComponent<TextMeshPro>().text = solutionSentenceGD;              // ...and display this Content Block's solution sentence.
            }
        }
    }

    private void OnMouseDown()                                                                  // If the mouse button is pressed...
    {
        if (contentBlock)                                                                       // ...and this Obstacle is a Content Block...
        {
            if (popupPrompt != null)                                                            // ...and a popup prompt is currently active...
            {
                if(popupPrompt.GetComponent<TextMeshPro>().text == solutionSentenceGD)          // ...if the solution statement is being displayed in Gaelic...
                    popupPrompt.GetComponent<TextMeshPro>().text = solutionSentenceEN;          // ...display it in English.
                else if(popupPrompt.GetComponent<TextMeshPro>().text == solutionSentenceEN)     // Else, if it's displayed in English...
                    popupPrompt.GetComponent<TextMeshPro>().text = solutionSentenceGD;          // ...display it in Gaelic.
            }
        }
    }

    private void OnMouseExit()                                                                  // If the mouse exits this Obstacle's collider...
    {
        if (contentBlock && popupPrompt != null)                                                // ...and this Obstacle is a Content Block and there is an active popup prompt...
                Destroy(popupPrompt);                                                           // ...destroy the popup prompt.
    }

    // A method to determine which direction the trigger was entered/exited from, by contrasting the entry/exit position with the 
    // centre point of the intersection +/- a small offset to account for any inconsistencies with the player's relative x/y position
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
            //print(directionEntered);
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
            //print(directionExited);
        }
    }

    // A method that checks if the player has met the conditions to break the Content Block, and triggers the appropriate behaviours.
    private void CheckContentBlock()
    {
        CheckSolution();                                                                // ...and call the CheckSolution() method to check if the right Tense and Form are active to break this Block.

        if (correctTense && correctForm)                                                // If the correct Tense and Form are currently active...
        {
            ActivatePoint(true);                                                        // ...activate this Obstacle...
            BreakBlock(true);                                                           // ... and break this Block with the BreakBlock() method.
        }
        else                                                                            // Otherwise...
        {
            col.isTrigger = false;                                                      // ...turn off this Content Block's trigger so it becomes a solid collider...
            thisAudio.clip = startingAudio;                                             // ...make sure that the active AudioSource clip is the starting clip (the "thump" sound)...
            thisAudio.Play();                                                           // ...and play the audio clip.
        }
    }

    // A method used to activate/deactivate this Obstacle's visual appearance and active status
    public void ActivatePoint(bool active)
    {
        if (active)                                                                             // If this Obstacle has been activated...
        {
            var activeCol = Color.gray;                                                         // ...create a temporary dulled colour...
            activeCol.a = 0.5f;                                                                 // ...that is semi-transparent...

            activated = true;                                                                   // ...set the activated status of this Obstacle to true...
            rend.color = activeCol;                                                             // ...and set the colour of the SpriteRenderer to the dulled colour.
        }
        else if (!active)                                                                       // If this Obstacle has been deactivated...
        {
            activated = false;                                                                  // ...set the activated status of this Obstacle to false...
            rend.color = rendCol;                                                               // ...and reset the colour of the SpriteRenderer to its original colour.
        }
    }

    // A method to activate/deactivate the behaviour associated with the Content Block obstacle when it is "broken" (i.e. solved)
    public void BreakBlock(bool active)
    {
        if (active)                                                                             // If the Content Block has been activated...
        {
            col.enabled = false;                                                                // ...disable the Obstacle's collider...
            playerScript.StopPuzzle(false);                                                     // ...call the StopPuzzle() method from the active Player object to stop the line being drawn...
            lineScript.FreezePoints();                                                          // ...call the FreezePoints() method from the active Starting Point to freeze the line in its current position...
            puzzleManager.FullReset();                                                          // ...reset both the active Tense and Form...
            lineScript = null;                                                                  // ...null out the lineScript so that it can be re-assigned.
            thisAudio.clip = solutionAudio;                                                     // ...set the solution sentence audio to the active audio clip...
            thisAudio.Play();                                                                   // ...and play the audio clip.
        }
        else if (!active)                                                                       // Otherwise, if the Content Block has been deactivated...
        {
            col.enabled = true;                                                                 // ...(re)enable the Obstacle's collider...
            lineScript = null;                                                                  // ...and null out the lineScript so that it can be re-assigned.
        }
    }

    // A method that checks whether or not the currently active Tense and Form meet the requirements to solve/break the Content Block (works identically to the FinishLine.cs's Update method)
    private void CheckSolution()
    {
        if (root && puzzleManager.rootVerb ||                                                   // If the required Tense is active in the PuzzleManager...
                pastTense && puzzleManager.pastTense ||
                futureTense && puzzleManager.futureTense ||
                conditionalTense && puzzleManager.conditionalTense)
            correctTense = true;                                                                // ...the active Tense is the correct tense.
        else                                                                                    // Otherwise...
            correctTense = false;                                                               // ...the active Tense is not the correct tense.

        if (positiveStatement && puzzleManager.positiveStatement ||                             // If the required Form is active in the PuzzleManager...
           positiveQuestion && puzzleManager.positiveQuestion ||
           negativeStatement && puzzleManager.negativeStatement ||
           negativeQuestion && puzzleManager.negativeQuestion ||
           noForm)
            correctForm = true;                                                                 // ...the active Form is the correct form.
        else                                                                                    // Otherwise...
            correctForm = false;                                                                // ...the active Form is not the correct form.
    }
}
