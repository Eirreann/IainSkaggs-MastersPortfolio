using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class is attached to the object that follows the mouse cursor and represents the final point in the LineRenderer; it is considered the "Player" object because it is the object that 
// the player controls to complete each puzzle.
public class FollowMouse : MonoBehaviour
{
    #region Variables
    [Range(0.5f, 50f)]
    public float moveSpd = 0.5f;                        // The speed at which the point follows the mouse's position

    [SerializeField]
    private GameObject pauseScreen;                     // The UI panel that represents the Pause Screen
    [SerializeField]
    private PuzzleManager puzzleManager;                // The puzzle manager of the current puzzle

    [HideInInspector]
    public DrawPuzzleLine lineScript;                   // The DrawPuzzleLine script from this instance's Starting Point
    [HideInInspector]
    public bool frozenPoint = false;                    // A boolean to record whether this point has been frozen or not

    private Vector3 mousePos;                           // A Vector3 to store the mouse's position on the screen (in world space)
    private Vector2 lastPos;                            // A Vector2 to record the obj's last position in certan circumstances
    private Rigidbody2D rb;                             // The obj's RigidBody2D component

    private bool isPaused = false;                      // A boolean to record whether the current puzzle has been paused or not.
    #endregion


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();                                   // Assigning the RigidBody2D component
        lineScript = transform.parent.GetComponent<DrawPuzzleLine>();       // Acquiring the DrawPuzzleLine component from the Obj's parent object (the current line's Starting Point)
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !puzzleManager.puzzleEnded)    // If the Pause button is pressed (currently ESC) and the puzzle has not been completed...
        {
            lastPos = transform.position;                                   // ...grab the last position of the Obj...
            isPaused = !isPaused;                                           // and trigger the isPaused boolean.
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Translates the mouse position into worldspace values and zeroes out the Z-axis (cuz 2D game)   
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (!puzzleManager.puzzleEnded && !frozenPoint)             // Check to make sure the puzzle is still active and the current line has not been frozen.
        {
            if (!isPaused)                                          // If the game isn't paused and the puzzle hasn't ended...
            {
                rb.MovePosition(Vector2.Lerp(transform.position,
                    mousePos, moveSpd * Time.deltaTime));           // ...lerp the Obj's rigidbody toward the mouse's position...
                pauseScreen.SetActive(false);                       // ... deactivate the Pause screen (if it's active)...
                //Cursor.visible = false;                           // ...and hide the cursor.
            }
            else if (isPaused)                                      // If the game *is* paused...
            {
                rb.MovePosition(lastPos);                           // ...stop the Obj at the last reported position...
                pauseScreen.SetActive(true);                        // ...activate the Pause screen...
                //Cursor.visible = true;                            // ...and reveal the cursor.
            }
        }
        else if (frozenPoint)                                       // If this Obj *has* been frozen...
        {
            this.tag = "oldPlayer";                                 // ...change the tag so that other lines don't confuse this Obj for the active Player.
            //this.gameObject.SetActive(false);
        }
    }

    // A method that stops the line from moving either when the puzzle's been completed or when a Content Block's been broken
    public void StopPuzzle(bool ended)
    {
        lastPos = transform.position;                               // Grab the last reported position...
        rb.MovePosition(lastPos);                                   // ...stop the Obj at this position...
        Cursor.visible = true;                                      // ...reveal the cursor...

        if (ended)                                                  // ...and, if the puzzle has ended...
            puzzleManager.puzzleEnded = true;                       // ...tell the PuzzleManager to stop the puzzle.
        else if (!ended)                                            // Or, if the line has been frozen (see Content Block behaviour in ObstacleBehaviour.cs)...
            frozenPoint = true;                                     // ...freeze this Obj.
    }
}
