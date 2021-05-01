using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// A script to manage the Game Over screen that appears when the player falls off a platform
public class GameOverScreen : MonoBehaviour {

    public GameObject gameOverText;             // The "You Died" message (or whatever it ends up being)
    public GameObject gameOverButtons;          // The panel that appear giving you the option to respawn or quit
    public GameObject placeHolder1;             // Empty GameObject that reserves space for the above GameObjects (for fade-in purposes)
    public GameObject placeHolder2;             // Empty GameObject that reserves space for the above GameObjects (for fade-in purposes)

    
    public void GameOverDelay()                 // Activavtes Game Over elements after a delay
    {
        Invoke("EnableGameOverElements", 2);
    }

    public void EnableGameOverElements()    // Calls the buttons and flavour text of the Game Over screen (replacing the placeholders)
    {
        gameOverText.SetActive(true);
        gameOverButtons.SetActive(true);

        placeHolder1.SetActive(false);
        placeHolder2.SetActive(false);
    }

    public void DisableGameOverElements()   // Disables the buttons and flavour text when the Game Over screen is dismissed
    {
        gameOverText.SetActive(false);
        gameOverButtons.SetActive(false);

        placeHolder1.SetActive(true);
        placeHolder2.SetActive(true);
    }
}
