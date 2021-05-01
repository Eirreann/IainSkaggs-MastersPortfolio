using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class manages the behaviour of the very first puzzle of the game (solved in the title screen).
public class StartPuzzle : MonoBehaviour
{
    private GameObject startPuzzle;                         // The GameObject that contains the starting puzzle
    public GameObject welcomePrompt;                        // The "welcome" UI that appears after the starting puzzle is completed
    private SpriteRenderer rend;                            // This object's SpriteRenderer component
    private Color rendCol;                                  // The SpriteRenderer's colour
    private AudioSource thisAudio;                          // This object's AudioSource component
    public AudioClip finishAudio;                           // The audio clip to play when the puzzle is completed

    private void Start()
    {
        thisAudio = GetComponent<AudioSource>();            // Assign the AudioSource component
        rend = GetComponent<SpriteRenderer>();              // Assign the SpriteRenderer component
        rendCol = rend.color;                               // Set the initial colour of the Sprite Renderer
        startPuzzle = transform.parent.gameObject;          // Assign the starting puzzle GameObject (this object's parent) to the variable
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")                      // If the Player object enters the trigger area...
        {
            FollowMouse playerScript = 
                collision.GetComponent<FollowMouse>();      // ...grab the FollowMouse component on the player...
            playerScript.StopPuzzle(true);                  // ...trigger the StopPuzzle method from the FollowMouse script to stop the line...
            CompleteStartPuzzle();                          // ...and trigger the CompleteStartPuzzle() method to close the starting puzzle and start the game proper.
        }
    }

    // A method that visually indicates that the Starting Puzzle has been completed and triggers the start of the game proper.
    private void CompleteStartPuzzle()
    {
        thisAudio.clip = finishAudio;                                                               // Make the solution sentence audio the active clip on the AudioSource
        PuzzleManager puzzleManager = 
            GameObject.FindGameObjectWithTag("PuzzleManager").GetComponent<PuzzleManager>();        // Grab the currently active PuzzleManager

        LineRenderer lineRend = 
            GameObject.FindGameObjectWithTag("LineHolder").GetComponent<LineRenderer>();            // Grab the currently active LineRenderer
        lineRend.material.color = Color.green;                                                      // Change the LineRenderer colour to green
        rend.color = Color.green;                                                                   // Change this object's SpriteRenderer to green
        thisAudio.Play();                                                                           // Play the solution sentence audio clip

        puzzleManager.positiveStatement = true;                                                     // Tell the PuzzleManager to display the PositiveStatement form
        Invoke("StartGame", 2);                                                                     // Invoke the StartGame() method after two seconds.
    }

    // A method that deactivates the Starting Puzzle object, thus starting the game proper.
    private void StartGame()
    {
        startPuzzle.SetActive(false);
        welcomePrompt.SetActive(true);
    }
}
