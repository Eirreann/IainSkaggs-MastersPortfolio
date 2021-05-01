using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is triggered when the game ends by the player completing the final puzzle.  It fades the screen to black before quitting the application.
public class EndGame : MonoBehaviour
{
    public UIManager uiManager;         // The UI Manager of the current scene

    private void Start()
    {
        Invoke("FadeOut", 3);           // When this object is activated, invoke the FadeOut() method after three seconds
    }

    // A method that fades the screen to black before triggering the application to close.
    private void FadeOut()
    {
        uiManager.FadeIn();            // Trigger the FadeOut animation from the UI Manager
        Invoke("QuitGame", 2);          // Invoke the QuitGame() method to quit the game after two seconds.
    }

    // A method that triggers the Quit() method in the UI Manager to quit the game
    private void QuitGame()
    {
        uiManager.Quit();
    }
}
