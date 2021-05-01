using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This class keeps track of all variables pertaining to the puzzle, tracks activation status of Tenses and Forms, and controls the Verb Output that represents the currently-active Tense and Form
public class PuzzleManager : MonoBehaviour
{
    #region Variables
    [Header("Tenses")]                  // Booleans to keep track of the currently active Tense
    public bool rootVerb;
    public bool pastTense;
    public bool futureTense;
    public bool conditionalTense;
    public string currentTense;         // A string to represent the currently-active Tense (referenced by other scripts)

    [Header("Forms")]                   // Booleans to keep track of the currently active Verb Form
    public bool positiveStatement;
    public bool negativeStatement;
    public bool positiveQuestion;
    public bool negativeQuestion;
    public string currentForm;          // A string to represent the currently-active Verb Form (referenced by other scripts)

    [Header("Conditions")]
    public bool tenseActive = false;    // A boolean to track whether a Tense (e.g. a LineRenderer drawn from a Starting Point) is currently active
    public bool formActive = false;     // A boolean to track whether a Verb Form is currently active

    [Header("Root Forms")]              // Strings to record the current puzzle's Verb Root Forms for the dynamic text output and their corresponding Audio Clips.
    [SerializeField]
    private string positiveRoot;
    [SerializeField]
    private AudioClip positiveRootAudio;
    [SerializeField]
    private string negativeRoot;
    [SerializeField]
    private AudioClip negativeRootAudio;

    [Header("Past Tense Forms")]        // Strings to record the current puzzle's Past Tense Verb Forms for the dynamic text output and their corresponding Audio Clips.
    [SerializeField]
    private string positiveStatementPT;
    [SerializeField]
    private AudioClip positiveStatementPTAudio;
    [SerializeField]
    private string negativeStatementPT;
    [SerializeField]
    private AudioClip negativeStatementPTAudio;
    [SerializeField]
    private string positiveQuestionPT;
    [SerializeField]
    private AudioClip positiveQuestionPTAudio;
    [SerializeField]
    private string negativeQuestionPT;
    [SerializeField]
    private AudioClip negativeQuestionPTAudio;

    [Header("Future Tense Forms")]      // Strings to record the current puzzle's Future Tense Verb Forms for the dynamic text output and their corresponding Audio Clips.
    [SerializeField]
    private string positiveStatementFT;
    [SerializeField]
    private AudioClip positiveStatementFTAudio;
    [SerializeField]
    private string negativeStatementFT;
    [SerializeField]
    private AudioClip negativeStatementFTAudio;
    [SerializeField]
    private string positiveQuestionFT;
    [SerializeField]
    private AudioClip positiveQuestionFTAudio;
    [SerializeField]
    private string negativeQuestionFT;
    [SerializeField]
    private AudioClip negativeQuestionFTAudio;

    [Header("Conditional Tense Forms")] // Strings to record the current puzzle's Conditional Tense Verb Forms for the dynamic text output and their corresponding Audio Clips.
    [SerializeField]
    private string positiveStatementCT;
    [SerializeField]
    private AudioClip positiveStatementCTAudio;
    [SerializeField]
    private string negativeStatementCT;
    [SerializeField]
    private AudioClip negativeStatementCTAudio;
    [SerializeField]
    private string positiveQuestionCT;
    [SerializeField]
    private AudioClip positiveQuestionCTAudio;
    [SerializeField]
    private string negativeQuestionCT;
    [SerializeField]
    private AudioClip negativeQuestionCTAudio;

    [HideInInspector]
    public bool puzzleEnded = false;    // A bool to track whether or not the current puzzle has been completed or not (for either a win or a loss)

    public TextMeshPro finishTextMesh;  // The TextMesh Pro component on the Verb Output point, that displays the currently active verb form and, on completion of the puzzle, indicates success.
    private string startingText;        // The initial text of the Verb Output ("..." by default)
    private AudioSource thisAudio;      // This object's AudioSource
    [HideInInspector]
    public bool activateAudio;          // A bool to activate the AudioSource to play the verb audio when it changes.

    //private DrawPuzzleLine lineStart;
    #endregion

    private void Start()
    {
        startingText = finishTextMesh.text;                     // Acquire the starting text of the Verb Output
        thisAudio = GetComponent<AudioSource>();                // Assign this Puzzle Manager's AudioSouce component
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !puzzleEnded)        // If the Reset Puzzle button (RMB) has been pressed and if the puzzle hasn't already ended...
        {
                ResetPuzzle();                                  // ...call the ResetPuzzle() method to completely reset the puzzle (see below)
        }

        DisplayCurrentVerbForm();                               // Call a method to display the currently-active Verb Form and Tense in the Verb Output/Finish Line.        

        // An if-statement that updates the currently active Verb Form in the currentForm string for reference from other scripts.
        if (positiveStatement)
            currentForm = "PositiveStatement";
        else if (negativeStatement)
            currentForm = "NegativeStatement";
        else if (positiveQuestion)
            currentForm = "PositiveQuestion";
        else if (negativeQuestion)
            currentForm = "NegativeQuestion";
        else
            currentForm = "";
    }

    // A method that changes the Verb Output's text to reflect the currently active Tense and Form
    private void DisplayCurrentVerbForm()
    {
        if (rootVerb)                                               // If the Root (Imperative Tense) of the Verb is currently active...
        {
            currentTense = "Root";                                  // ...update the currentTense string to reflect the currently active Tense (referenced in other scripts).

            if (positiveStatement)                                  // If the Positive Statement form is currently active...
            {
                finishTextMesh.text = positiveRoot;                 // ...display the correct text...
                thisAudio.clip = positiveRootAudio;                 // ...set the Positive Root audio clip in the AudioSource.

            }
            else if (negativeStatement)                             // Or if the Negative Statement form is currently active...
            {
                finishTextMesh.text = negativeRoot;                 // ...display the correct text...
                thisAudio.clip = negativeRootAudio;                 // ...and set the Negative Root audio clip in the AudioSource.
            }
            else                                                    // Otherwise, if any other Form or no Form is currently active...
            {
                finishTextMesh.text = startingText;                 // ...display the starting text...
                thisAudio.clip = null;                              // ...and null the Audio Clip.
            }
        }
        else if (pastTense)                                         // If the Past Tense of the Verb is currently active...
        {
            currentTense = "Past";                                  // ...update the currentTense string to reflect the currently active Tense (referenced in other scripts).

            if (positiveStatement)                                  // If the Positive Statement form is currently active...
            {
                finishTextMesh.text = positiveStatementPT;          // ...display the correct text...
                thisAudio.clip = positiveStatementPTAudio;          // ...and set the audio clip in the AudioSource.
            }
            else if (negativeStatement)                             // Or if the Negative Statement form is currently active...
            {
                finishTextMesh.text = negativeStatementPT;          // ...display the correct text.
                thisAudio.clip = negativeStatementPTAudio;          // ...and set the audio clip in the AudioSource.
            }
            else if (positiveQuestion)                              // Or if the Positive Question form is currently active...
            {
                finishTextMesh.text = positiveQuestionPT;           // ...display the correct text.
                thisAudio.clip = positiveQuestionPTAudio;           // ...and set the audio clip in the AudioSource.
            }
            else if (negativeQuestion)                              // Or if the Negative Question form is currently active...
            {
                finishTextMesh.text = negativeQuestionPT;           // ...display the correct text.
                thisAudio.clip = negativeQuestionPTAudio;           // ...and set the audio clip in the AudioSource.
            }
            else                                                    // Otherwise, if no Form is currently active...
            {
                finishTextMesh.text = startingText;                 // ... display the starting text.
                thisAudio.clip = null;                              // ...and null the Audio Clip.
            }
        }
        else if (futureTense)                                       // If the Future Tense of the Verb is currently active...
        {
            currentTense = "Future";                                // ...update the currentTense string to reflect the currently active Tense (referenced in other scripts).

            if (positiveStatement)                                  // If the Positive Statement form is currently active...
            {
                finishTextMesh.text = positiveStatementFT;          // ...display the correct text.
                thisAudio.clip = positiveStatementFTAudio;          // ...and set the audio clip in the AudioSource.
            }
            else if (negativeStatement)                             // Or if the Negative Statement form is currently active...
            {
                finishTextMesh.text = negativeStatementFT;          // ...display the correct text.
                thisAudio.clip = negativeStatementFTAudio;           // ...and set the audio clip in the AudioSource.
            }
            else if (positiveQuestion)                              // Or if the Positive Question form is currently active...
            {
                finishTextMesh.text = positiveQuestionFT;           // ...display the correct text.
                thisAudio.clip = positiveQuestionFTAudio;           // ...and set the audio clip in the AudioSource.
            }
            else if (negativeQuestion)                              // Or if the Negative Question form is currently active...
            {
                finishTextMesh.text = negativeQuestionFT;           // ...display the correct text.
                thisAudio.clip = negativeQuestionFTAudio;           // ...and set the audio clip in the AudioSource.
            }
            else                                                    // Otherwise, if no Form is currently active...
            {
                finishTextMesh.text = startingText;                 // ... display the starting text.
                thisAudio.clip = null;                              // ...and null the Audio Clip.
            }
        }
        else if (conditionalTense)                                  // If the Conditional Tense of the Verb is currently active...
        {
            currentTense = "Conditional";                           // ...update the currentTense string to reflect the currently active Tense (referenced in other scripts).

            if (positiveStatement)                                  // If the Positive Statement form is currently active...
            {
                finishTextMesh.text = positiveStatementCT;          // ...display the correct text.
                thisAudio.clip = positiveStatementCTAudio;           // ...and set the audio clip in the AudioSource.
            }
            else if (negativeStatement)                             // Or if the Negative Statement form is currently active...
            {
                finishTextMesh.text = negativeStatementCT;          // ...display the correct text.
                thisAudio.clip = negativeStatementCTAudio;           // ...and set the audio clip in the AudioSource.
            }
            else if (positiveQuestion)                              // Or if the Positive Question form is currently active...
            {
                finishTextMesh.text = positiveQuestionCT;           // ...display the correct text.
                thisAudio.clip = positiveQuestionCTAudio;           // ...and set the audio clip in the AudioSource.
            }
            else if (negativeQuestion)                              // Or if the Negative Question form is currently active...
            {
                finishTextMesh.text = negativeQuestionCT;           // ...display the correct text.
                thisAudio.clip = negativeQuestionCTAudio;           // ...and set the audio clip in the AudioSource.
            }
            else                                                    // Otherwise, if no Form is currently active...
            {
                finishTextMesh.text = startingText;                 // ... display the starting text.
                thisAudio.clip = null;                              // ...and null the Audio Clip.
            }
        }
        else                                                        // If no tense is currently active...
        {
            currentTense = "";                                      // ...blank out the currentTense string.
        }

        PlayAudio(activateAudio);                                   // Call the PlayAudio() method to play the audio of the activated Verb form (if activated).
    }

    // A method called to play the audio clip JUST ONCE for a Verb Form (Condition) when it is activated
    private void PlayAudio(bool activate)
    {
        if (!thisAudio.isPlaying)                                   // If the AudioSource isn't already playing...
        {
            if (activate)                                           // ...and if the activate audio boolean is true...
                thisAudio.Play();                                   // ...play the audio clip.
            else if (!activate)                                     // Otherwise if the activate audio boolean is false...
                return;                                             // ...do nothing.
        }

        activateAudio = false;                                      // Reset the activate audio boolean to false when done so the audio clip doesn't play on repeat while the form is active.
    }

    // A method called from other scripts (mainline DrawPuzzleLine.cs) to update the active Tense boolean (serves to both active or deactivate the current Tense).
    public void SendTenseInfo(string tense)
    {
        if (tense == "Root")
            rootVerb = !rootVerb;
        else if (tense == "Past")
            pastTense = !pastTense;
        else if (tense == "Future")
            futureTense = !futureTense;
        else if (tense == "Conditional")
            conditionalTense = !conditionalTense;
    }

    // A method called from other scripts (mainly ConditionBehaviour.cs) to update the active Form boolean (serves to both active or deactivate the current Form).
    public void SendFormInfo(string form)
    {
        if (form == "PositiveStatement")
            positiveStatement = !positiveStatement;
        else if (form == "NegativeStatement")
            negativeStatement = !negativeStatement;
        else if (form == "PositiveQuestion")
            positiveQuestion = !positiveQuestion;
        else if (form == "NegativeQuestion")
            negativeQuestion = !negativeQuestion;
    }

    // A method that completely resets the currently active Puzzle; called primarily when the "Reset Puzzle" button is pressed (see above)
    public void ResetPuzzle()
    {
        finishTextMesh.text = startingText;

        // Reset all of the tenses
        GameObject[] tenses = GameObject.FindGameObjectsWithTag("Tense");                   // Fill an array with all active Tense (Starting Point) GameObjects...
        for(var i = 0; i < tenses.Length; i++)
        {
            tenses[i].GetComponent<DrawPuzzleLine>().ResetLine();                           // ...and trigger the ResetLine() method in each of them to reset all active lines.
        }

        // Reset all of the intersections
        GameObject[] intersections = GameObject.FindGameObjectsWithTag("Intersection");     // Fill an array with all active Intersection GameObjects...
        for(var i = 0; i < intersections.Length; i++)
        {
            intersections[i].GetComponent<IntersectionBehaviour>().ResetBehaviour();        // ...and trigger the ResetBehaviour() method in each of them to reset all intersections to their original state.
        }

        // Reset all of the Pickups
        GameObject[] forms = GameObject.FindGameObjectsWithTag("Form");                     // Fill an array with all active Form (pickup) GameObjects...
        for(var i = 0; i < forms.Length; i++)
        {
            forms[i].GetComponent<ConditionBehaviour>().ActivatePoint(false);               // ...and deactivate them using the ActivatePoint() method to reset them to their default state.
            if (forms[i].GetComponent<ConditionText>() != null)                             // If any active Form GameObjects have the ConditionText script attached (e.g. it's currently the last level in a sequence)...
            {
                forms[i].GetComponent<ConditionText>().ResetText();                         // ...trigger the ResetText() method in each of them to reset the text to its initial value.
            }
        }

        // Reset all of the Obstacles
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");             // Fill an array with all active Obstacle GameObjects...
        for (var i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].GetComponent<ObstacleBehaviour>().ActivatePoint(false);            // ... and deactivate them using the ActivatePoint() method to reset them to their default state.
            if (obstacles[i].GetComponent<ObstacleBehaviour>().contentBlock)                // If any of the active Obstacle GameObjects are Content Blocks...
            {
                obstacles[i].GetComponent<ObstacleBehaviour>().BreakBlock(false);           // ...also call the BreakBlock() method to reset the path block's behaviours.
            }
        }

        FullReset();                                                                        // Reset all of the Tense and Form conditions (see method below)
    }

    // A method that resets all Tense booleans and sets the Tense active status to false, used primarily by the Time Vortex obstacle.
    public void TenseReset()
    {
        rootVerb = false;
        pastTense = false;
        futureTense = false;
        conditionalTense = false;
        tenseActive = false;
    }

    // A method that resets all Form booleans and sets the Form active status to false, used primarily by the Confusifier obstacle.
    public void FormReset()
    {
        positiveStatement = false;
        negativeStatement = false;
        positiveQuestion = false;
        negativeQuestion = false;
        formActive = false;
    }

    // A method that consolidates the TenseReset() and FormReset() methods into a single method for ease-of-access, and resets the output text
    public void FullReset()
    {
        TenseReset();
        FormReset();
        finishTextMesh.text = startingText;
    }
}
