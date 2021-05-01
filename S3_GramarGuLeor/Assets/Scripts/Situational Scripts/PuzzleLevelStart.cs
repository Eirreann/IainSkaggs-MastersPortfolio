using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is attached to 2D sprites in the environment that are associated with Puzzle Sequences, and triggers the start of a puzzle sequence (as well as some other visual responses) when clicked on
public class PuzzleLevelStart : MonoBehaviour
{
    #region Variables
    public GameObject preSprite;            // The visible sprite before the object is clicked on
    public GameObject postSprite;           // The visible sprite after the object is clicked on
    public GameObject hoverText;            // The floating text that appears when the cursor is hovered over the object

    public GameObject targetPuzzle;         // The target Puzzle Sequence to trigger when the object is clicked on
    private LevelManager levelManager;      // The LevelManager of the target Puzzle Sequence
    private bool puzzleActive = false;      // A boolean to track whether the puzzle is active or not
    private bool activated = false;         // A boolean to track whether this object has been activated (before the puzzle becomes active)

    private GameObject hiddenObj;           // An additional sprite that may be "hidden" under the preSprite (e.g. the scroll in the vase)
    public GameObject nextLevel;            // The next interactable object that triggers the following Puzzle Sequence
    private AudioSource thisAudio;          // This object's AudioSource
    #endregion

    private void Start()
    {
        if(postSprite.transform.childCount > 0)
            hiddenObj = postSprite.transform.GetChild(0).gameObject;    // If this object has children, assign the first Child object to the hiddenObj variable

        levelManager = 
            targetPuzzle.GetComponentInChildren<LevelManager>();        // Assign the LevelManager of the target puzzle

        thisAudio = GetComponent<AudioSource>();                        // Assgn this object's AudioSource component
    }

    private void Update()
    {
        if (levelManager.levelCleared)                                  // If the level has been cleared...
        {
            if(hiddenObj != null)
                hiddenObj.SetActive(false);                             // ...deactivate the hiddenObj if there is one...

            nextLevel.SetActive(true);                                  // ... and activate the next Puzzle Sequence starter object
        }
    }

    private void OnMouseOver()                                          // When the mouse cursor is over this object's collider...
    {
        if (!puzzleActive)                                              // If the puzzle is not active...
            hoverText.SetActive(true);                                  // ...activate the hover text.
        else if (activated)                                             // However, if this object has been activated...
            hoverText.SetActive(false);                                 // ...deactivate the hover text.
    }

    private void OnMouseExit()                                          // When the mouse cursor exits this object's collider...
    {
        if(hoverText.activeSelf == true)                                // If the hover text is active...
            hoverText.SetActive(false);                                 // ...deactivate the hover text.
    }

    private void OnMouseDown()                                          // When the mouse button is pressed on this object...
    {
        preSprite.SetActive(false);                                     // Deactivate the pre-clicked sprite
        postSprite.SetActive(true);                                     // Activate the post-clicked sprite
        if (!activated)                                                 // If the object hasn't been activated yet....
        {
            Invoke("StartPuzzleSequence", 2);                           // ...invoke the StartPuzzleSequence() method after two seconds...
            thisAudio.Play();                                           // ...play the audio clip...
            activated = true;                                           // ...and set this object's status to activated.
        }
        else                                                            // Otherwise...
        {
            StartPuzzleSequence();                                      // ...invoke the StartPuzzleSequence() method without a delay.
        }
    }

    // A method that starts the target Puzzle Sequence
    private void StartPuzzleSequence()
    {
        targetPuzzle.SetActive(true);                                   // Activate the target Puzzle Sequence GameObject
        hoverText.SetActive(false);                                     // Make sure the hover text is deactivated
        puzzleActive = true;                                            // Set the status of the puzzle to Active (thus preventing the hover text appearing again)
    }
}
