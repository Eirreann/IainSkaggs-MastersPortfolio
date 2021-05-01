using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

// This class manages the "Verb Output" that represents the effective finish line of each puzzle; the "Player" object must be drawn to this point with the correct Tense and Form active.  Tense and Form requirements
// for each puzzle are stored in this class, which are compared to the active Tense and Form in the PuzzleManager to determine whether the player has solved the puzzle or not.
public class FinishLine : MonoBehaviour
{
    #region Variables
    [Header("Solution Tense")]                  // Booleans to set the current puzzle's Tense requirements (set in the Inspector)
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

    [Header("Solution Form")]                   // Booleans to set the current puzzle's Form requirements (set in the Inspector)
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

    [Header("Status")]                          // Booleans to track whether the currently active Tense and Form are the required Tense and Form to solve the puzzle
    [SerializeField]
    private bool correctTense = false;
    [SerializeField]
    private bool correctForm = false;

    private PuzzleManager puzzleManager;        // The PuzzleManager script of the currently-active puzzle
    private FollowMouse playerScript;           // The FollowMouse script on the "Player" object

    private SpriteRenderer rend;                // This Verb Output object's SpriteRenderer component
    private Color rendCol;                      // The colour of this object's SpriteRenderer

    [Header("Audio Clips")]
    private AudioSource thisAudio;              // This object's AudioSource component
    public AudioClip solutionSentence;          // The audio clip of this puzzle's solution sentence, played when the puzzle is solved
    public AudioClip wrongSolution;             // The audio clip that plays if the puzzle is completed incorrectly
    #endregion

    private void Start()
    {
        thisAudio = GetComponent<AudioSource>();                                                    // Assigning the AudioSource component
        rend = GetComponent<SpriteRenderer>();                                                      // Assigning the SpriteRenderer component
        rendCol = rend.color;                                                                       // Setting the starting/default colour of the SpriteRenderer

        puzzleManager = 
            GameObject.FindGameObjectWithTag("PuzzleManager").GetComponent<PuzzleManager>();        // Finding and assigning the PuzzleManager of the currently-active puzzle.
    }

    private void Update()
    {
        if (root && puzzleManager.rootVerb ||                                                       // If the required Tense is active in the PuzzleManager...
            pastTense && puzzleManager.pastTense ||
            futureTense && puzzleManager.futureTense ||
            conditionalTense && puzzleManager.conditionalTense)
            correctTense = true;                                                                    // ...the active Tense is the correct tense.
        else                                                                                        // Otherwise...
            correctTense = false;                                                                   // ...the active Tense is not the correct tense.

        if (positiveStatement && puzzleManager.positiveStatement ||                                 // If the required Form is active in the PuzzleManager...
            positiveQuestion && puzzleManager.positiveQuestion ||
            negativeStatement && puzzleManager.negativeStatement ||
            negativeQuestion && puzzleManager.negativeQuestion ||
            noForm)
            correctForm = true;                                                                     // ...the active Form is the correct form.
        else                                                                                        // Otherwise...
            correctForm = false;                                                                    // ...the active Form is not the correct form.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")                                                               // If the "Player" object enters the Verb Output object's trigger area...
        {
            playerScript = collision.GetComponent<FollowMouse>();                                   // ...grab and assign the FollowMouse script from the "Player" object.

            if (correctTense && correctForm)                                                         // If both the correct Tense and the correct Form are active when the player enters the trigger area...
                WinPuzzle();                                                                        // ...trigger the WinPuzzle() method to move on to the next puzzle (or complete the puzzle sequence).
            else                                                                                    // Otherwise...
                LosePuzzle();                                                                       // ...trigger the LosePuzzle() method to reset the puzzle.
        }
    }

    // A method to trigger the win conditions for when the Player reaches the end of the puzzle with the correct Tense and Form active.
    private void WinPuzzle()
    {
        if (puzzleManager.puzzleEnded)                                                              // If the puzzle has already ended (e.g. if the method has already been triggered)...
        {
            return;                                                                                 // ...do nothing (you've already won once, don't get greedy).
        }
        else                                                                                        // Otherwise...
        {
            if (noForm)                                                                             // If no Form was required to complete this puzzle (only true for puzzle 1.01)...
                puzzleManager.positiveStatement = true;                                             // ...set the PositiveStatement in the PuzzleManager to be true.

            playerScript.StopPuzzle(true);                                                          // Activate the StopPuzzle() method process in the currently active FollowMouse script to stop the "Player" object's movement

            thisAudio.clip = solutionSentence;                                                      // Set the solution sentence audio clip as the active clip in the AudioSource
            thisAudio.Play();                                                                       // Play the solution sentence audio clip

            LineRenderer lineRend = 
                GameObject.FindGameObjectWithTag("LineHolder").GetComponent<LineRenderer>();        // Find and assign the currently active LineRenderer to a local variable
            lineRend.material.color = Color.green;                                                  // Change the colour of the LineRenderer to green
            rend.color = Color.green;                                                               // Change the colour of this object's SpriteRenderer to green


            Invoke("NextPuzzle", thisAudio.clip.length + 1);                                        // And invoke the next puzzle to start after two seconds
        }        
    }

    // A method to trigger the win conditions for when the Player reaches the end of the puzzle without the correct Tense and Form active.
    private void LosePuzzle()
    {
        playerScript.StopPuzzle(true);                                                              // Activate the StopPuzzle() method process in the currently active FollowMouse script to stop the "Player" object's movement

        thisAudio.clip = wrongSolution;                                                             // Set the wrong solution audio clip as the active clip in the AudioSource
        thisAudio.Play();                                                                           // Play the wrong solution audio clip

        LineRenderer lineRend = 
            GameObject.FindGameObjectWithTag("LineHolder").GetComponent<LineRenderer>();            // Find and assign the currently active LineRenderer to a local variable
        lineRend.material.color = Color.red;                                                        // Change the colour of the LineRenderer to red
        rend.color = Color.red;                                                                     // Change the colour of this object's SpriteRenderer to red

        Invoke("ResetPuzzle", 2);                                                                   // And invoke a complete reset of the puzzle after two seconds
    }

    // A method to reset the puzzle in the event of the current puzzle being completed unsuccessfully
    private void ResetPuzzle()
    {
        rend.color = rendCol;                                                                       // Reset the SpriteRenderer's colour to its original colour
        puzzleManager.ResetPuzzle();                                                                // Trigger the ResetPuzzle() method in the PuzzleManager to completely reset all of the puzzle elements.
    }

    // A method to trigger the start of the next puzzle in the sequence in the event of the current puzzle being completed successfully
    private void NextPuzzle()
    {
        LevelManager levelManager = 
            GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();          // Find and assign the LevelManager for the current Puzzle Sequence to a local variable
        transform.parent.GetComponent<PuzzleTracker>().isCompleted = true;                          // Mark the current puzzle as being completed successfully
        levelManager.NextLevel();                                                                   // And call the NextLevel() method from the LevelManager to trigger the next level to start (or to complete the puzzle sequence)
    }

}
