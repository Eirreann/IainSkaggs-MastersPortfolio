using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This class manages additional behaviours associated with Conditions in the final level of Puzzle Sequence; it dynamically modifies the TextMesh Pro component 
// over the Condition sprites that represent the morphological changes that the Condition's Form will effect on the final Verb Output.
public class ConditionText : MonoBehaviour
{
    #region Variables
    private ConditionBehaviour thisCondition;       // The behaviour script for the Condition object that this script is attached to
    private TextMeshPro thisText;                   // The TextMesh Pro component this script effects
    private string startingText;                    // The initial text of the TextMesh Pro component
    private PuzzleManager puzzleManager;            // The PuzzleManager of the currently-active puzzle

    [Header("Root Forms")]                          // Strings to display that reflect the morphological changes to the root of a Verb
    [SerializeField]
    private string positiveRoot;
    [SerializeField]
    private string negativeRoot;

    [Header("Past Tense Forms")]                    // Strings to display that reflect the morphological changes to the Past Tense of a Verb
    [SerializeField]
    private string positiveStatementPT;
    [SerializeField]
    private string negativeStatementPT;
    [SerializeField]
    private string positiveQuestionPT;
    [SerializeField]
    private string negativeQuestionPT;

    [Header("Future Tense Forms")]                  // Strings to display that reflect the morphological changes to the Future Tense of a Verb
    [SerializeField]
    private string positiveStatementFT;
    [SerializeField]
    private string negativeStatementFT;
    [SerializeField]
    private string positiveQuestionFT;
    [SerializeField]
    private string negativeQuestionFT;

    [Header("Conditional Tense Forms")]             // Strings to display that reflect the morphological changes to the Conditional Tense of a Verb
    [SerializeField]
    private string positiveStatementCT;
    [SerializeField]
    private string negativeStatementCT;
    [SerializeField]
    private string positiveQuestionCT;
    [SerializeField]
    private string negativeQuestionCT;

    [Header("Sprites")]
    [SerializeField]
    private Sprite symbolSprite;                    // The static symbol sprite that displays before a Line has been drawn
    [SerializeField]
    private Sprite blankSprite;                     // The blank sprite background that displays when a line starts

    private SpriteRenderer thisSprite;              // This Condition's SpriteRenderer
    #endregion

    void Start()
    {
        puzzleManager = 
            GameObject.FindGameObjectWithTag("PuzzleManager").GetComponent<PuzzleManager>();    // Finding and assigning the currently active PuzzleManager script
        thisCondition = GetComponent<ConditionBehaviour>();                                     // Assigning the ConditionBehaviour script component on this object
        thisSprite = GetComponent<SpriteRenderer>();                                            // Assigning the SpriteRenderer component
        thisText = GetComponentInChildren<TextMeshPro>();                                       // Finding and assigning the TextMesh Pro component on the Child object
        startingText = thisText.text;                                                           // Setting the initial value of startingText
    }

    void Update()
    {
        DisplayVerbForm();                                  // Calling the DisplayVerbForm method to dynamically alter the text on this Condition to reflect the currently active Tense (see below)
    }

    // A method that changes the TextMesh Pro component associated with this Condition to reflect the morphological impact of the currently active Tense (as input into the Inspector)
    private void DisplayVerbForm()
    {
        if (puzzleManager.rootVerb)
        {
            thisSprite.sprite = blankSprite;

            if (thisCondition.isPositiveStatement)
            {
                thisText.text = positiveRoot;
            }
            else if (thisCondition.isNegativeStatement)
            {
                thisText.text = negativeRoot;
            }
            else if (thisCondition.isPositiveQuestion)
            {
                thisText.text = "x";
            }
            else if (thisCondition.isNegativeQuestion)
            {
                thisText.text = "x";
            }
            else
            {
                thisText.text = startingText;
            }
        }
        else if (puzzleManager.pastTense)
        {
            thisSprite.sprite = blankSprite;

            if (thisCondition.isPositiveStatement)
            {
                thisText.text = positiveStatementPT;
            }
            else if (thisCondition.isNegativeStatement)
            {
                thisText.text = negativeStatementPT;
            }
            else if (thisCondition.isPositiveQuestion)
            {
                thisText.text = positiveQuestionPT;
            }
            else if (thisCondition.isNegativeQuestion)
            {
                thisText.text = negativeQuestionPT;
            }
            else
            {
                thisText.text = startingText;
            }
        }
        else if (puzzleManager.futureTense)
        {
            thisSprite.sprite = blankSprite;

            if (thisCondition.isPositiveStatement)
            {
                thisText.text = positiveStatementFT;
            }
            else if (thisCondition.isNegativeStatement)
            {
                thisText.text = negativeStatementFT;
            }
            else if (thisCondition.isPositiveQuestion)
            {
                thisText.text = positiveQuestionFT;
            }
            else if (thisCondition.isNegativeQuestion)
            {
                thisText.text = negativeQuestionFT;
            }
            else
            {
                thisText.text = startingText;
            }
        }
        else if (puzzleManager.conditionalTense)
        {
            thisSprite.sprite = blankSprite;

            if (thisCondition.isPositiveStatement)
            {
                thisText.text = positiveStatementCT;
            }
            else if (thisCondition.isNegativeStatement)
            {
                thisText.text = negativeStatementCT;
            }
            else if (thisCondition.isPositiveQuestion)
            {
                thisText.text = positiveQuestionCT;
            }
            else if (thisCondition.isNegativeQuestion)
            {
                thisText.text = negativeQuestionCT;
            }
            else
            {
                thisText.text = startingText;
            }
        }
        else
        {
            thisSprite.sprite = symbolSprite;
            thisText.text = "";
        }
    }

    // A method to reset the content of the TextMesh Pro's text to its initial value
    public void ResetText()
    {
        thisText.text = startingText;
    }


    // An attempt to make inapplicable Conditions disappear (e.g. Interrogative forms when drawing a line from the Root)
    //private void HideThisCondition(bool hide)
    //{
    //    if (hide)
    //    {
    //        thisCondition.activated = true;
    //        thisCondition.ActivatePoint(true);
    //    }
    //    else if (!hide)
    //    {
    //        thisCondition.activated = false;
    //        thisCondition.ActivatePoint(false);
    //    }
    //}
}
