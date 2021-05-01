using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

// A very simple class attached to each Puzzle Level that keeps track of whether or not it has been completed (referened by FinishLine.cs)
public class PuzzleTracker : MonoBehaviour
{
    [HideInInspector]
    public bool isCompleted;                // A boolean to store level completion status

    private void Start()
    {
        isCompleted = false;                // When the level spawns for the first time, it is not completed (duh).
    }
}
