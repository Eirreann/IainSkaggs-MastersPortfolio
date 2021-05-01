using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class manages the scene transition between the first level and the rest of the game, attached to the entrance of the cave in 01_ExteriorScene and activated by an OnMouseDown
public class ChangeScene : MonoBehaviour
{
    public UIManager uiManager;                             // The UI Manager of the current scene
    public string targetScene;                              // The target scene to transition to

    private SpriteRenderer lightSprite;                     // The light rays sprite attached to the cave entrance
    private Color originalColor;                            // The original colour of the light rays sprite
    private Color highlightColor;                           // The highlighted colour of the light rays sprite
    private bool isClicked;                                 // A boolean to track whether this collider has been clicked or not

    void Start()
    {
        lightSprite = 
            GetComponentInChildren<SpriteRenderer>();       // Grab the SpriteRenderer component of the light rays sprite (the only child of this GameObject)
        originalColor = lightSprite.color;                  // Assigns the original colour of the light rays sprice
        highlightColor = originalColor;                     // Assigns the initial highlighted colour
        highlightColor.a = 255;                             // Increases the alpha of the highlighted colour to make it seem brighter than the original colour.
    }

    private void OnMouseOver()
    {
        if (!isClicked)                                     // If the spot hasn't been clicked yet...
        {
            lightSprite.color = highlightColor;             // ...change the colour of the sprite to the highlighted colour on mouse over
        }             
    }

    private void OnMouseDown()                              // When the object is clicked on...
    {
        lightSprite.color = originalColor;                  // Change the sprite back to the original colour
        isClicked = true;                                   // Set the clicked status to true
        uiManager.ChangeScene(targetScene);                 // And trigger a scene change to the target scene
    }

    private void OnMouseExit()
    {
        lightSprite.color = originalColor;                  // Return the sprite to its original colour when the mouse is no longer hovering over it
    }
}
