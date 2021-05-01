using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class manages the completion status of and the transitions between puzzle levels in a puzzle sequence.
public class LevelManager : MonoBehaviour
{
    #region Variables
    public GameObject[] levelPuzzles;       // An array that contains all of the puzzles in this LevelManager's puzzle sequence (assigned in the Inspector)
    public Button levelBack;                // The UI button that allows the player to cycle backward through the level list
    public Button levelForward;             // The UI button that allows the player to cycle forward through the level list
    private GameObject activeLevel;         // A variable to story the currently active level GameObject
    private int activeLevelPos;             // An integer to track the position of the currently active level in the levelPuzzles array

    public GameObject puzzleSequence;       // The GameObject that is the parent object of the current puzzle sequence
    [HideInInspector]
    public bool levelCleared = false;       // A boolean to track the completion status of the current puzzle sequence
    #endregion

    void Start()
    {
        FindActivePuzzle();                                             // Find the currently active puzzle in the sequence (see method below)
    }

    void Update()
    {
        if (levelPuzzles[activeLevelPos] == levelPuzzles[0])            // If the active puzzle is the first puzzle in the array...
            levelBack.interactable = false;                             // ...disable to back cycle button.
        else                                                            // Otherwise...
            levelBack.interactable = true;                              // ...enable the back cycle button.

        if (activeLevel.GetComponent<PuzzleTracker>().isCompleted)      // If the current level has been completed...
            levelForward.interactable = true;                           // ...enable the forward cycle button.
        else                                                            // Otherwise...
            levelForward.interactable = false;                          // ...keep the forward cycle button disabled.
    }

    // A method that transitions from the currently active puzzle level to the next puzzle level in the Sequence array (called from FinishLine.cs)
    public void NextLevel()
    {
        if(activeLevelPos + 2 <= levelPuzzles.Length)                   // If the next puzzle is within the max number of puzzles in the Sequence array (+2 to compensate for array starting at 0)...
        {
            activeLevel.SetActive(false);                               // ...de-activate the current level...
            levelPuzzles[activeLevelPos + 1].SetActive(true);           // ...activate the next level in the array...
            FindActivePuzzle();                                         // ...and call the FindActivePuzzle() method to set the next level as the current level.
        }
        else if (activeLevelPos + 2 > levelPuzzles.Length)              // Otherwise, if there is no next puzzle in the array to switch to...
        {
            levelCleared = true;                                        // Set this puzzle sequence as being cleared...
            puzzleSequence.SetActive(false);                            // ...and de-activate the current puzzle sequence.
        }
        
    }

    // A method that transitions from the currently active puzzle level to the previous puzzle level in the Sequence array (called from FinishLine.cs)
    public void PreviousLevel()                     
    {
        activeLevel.SetActive(false);                                   // De-activate the current level...
        levelPuzzles[activeLevelPos - 1].SetActive(true);               // ...activate the previous level in the array...
        FindActivePuzzle();                                             // ...and call the FindActivePuzzle() method to set the previous level as the current level.
    }

    // A method to locate the currently active puzzle in the active puzzle sequence
    private void FindActivePuzzle()
    {
        for (var i = 0; i < levelPuzzles.Length; i++)                   // A for-loop that searches through the puzzle sequence array
        {
            if (levelPuzzles[i].activeSelf == true)                     // If a puzzle in the array is active...
            {
                activeLevel = levelPuzzles[i];                          // ...assign that puzzle to the activeLevel variable...
                activeLevelPos = i;                                     // ... and record its position in the array.
            }
        }
    }
}
