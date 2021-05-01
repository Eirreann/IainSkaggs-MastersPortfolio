using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class was initially written for the added implementation of a "Close" button on each of the puzzles, however it was retired due to time.  Kept in the project for future work.
public class TogglePuzzle : MonoBehaviour
{
    public GameObject targetPuzzleSequence;
    public GameObject inventory;

    private LevelManager levelManager;

    public bool isToggleOn = false;

    private void Start()
    {
        levelManager = targetPuzzleSequence.GetComponentInChildren<LevelManager>();
    }

    private void OnMouseDown()
    {
        ToggleTargetPuzzle(isToggleOn);
    }

    private void ToggleTargetPuzzle(bool toggle)
    {
        if (toggle)
        {
            targetPuzzleSequence.SetActive(true);
        }
        else if (!toggle)
        {
            targetPuzzleSequence.SetActive(false);
        }
    }
}
