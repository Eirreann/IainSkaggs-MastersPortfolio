using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameCredits : MonoBehaviour {

    public GameObject creditsMusic;         // The GameObjec that has the music that plays when the end credits role
    public GameObject endCreditsPanel;      // The panel that holds all of the end credits
    public GameObject nextCreditScene;      // The next end credits screen to fade-in
    public GameObject closingPanel;         // The final panel that will prompt the player to restart from last checkpoint or quit to main menu

    public float fadeOutTime;               // The time over which the CrossFadeAlpha method will fade out the credits scenes
    public float switchTime = 3;            // The delay timer on the Invoke method before the next scene is called

    private Image thisCreditScene;          // The Image component of the current credits scene, in order to chang ealpha

	// Use this for initialization
	void Start () {
        
        // Grabs the image component from the currently-assigned credits scene
        thisCreditScene = this.gameObject.GetComponent<Image>();

	}

    public void FirstSwitch()               // This method is called when the credits panel's fade-in animations completes, to kickstart the credits by activating the music and first scene
    {
        nextCreditScene.SetActive(true);

        creditsMusic.SetActive(true);
    }

    public void ImageSwitch()               // This method fades out the current credits scene and calls the next scene to after a short delay.
    {
        thisCreditScene.CrossFadeAlpha(0, fadeOutTime, false);

        Invoke("RollCredits", switchTime);
    }

    public void RollCredits()               // This method activates the next credits scene and deactivates the current credits scene, to be used with a delay from the Invoke()
    {
        nextCreditScene.SetActive(true);

        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))                                              // When the player enters the trigger area (Broch door)...
        {
            other.gameObject.GetComponent<PlayerRespawnTracker>().DeactivatePlayerControl();    // ...deactivate the player's control...

            endCreditsPanel.SetActive(true);                                                    // ...and activate the credits panel.
        }
    }

    public void LastPanelSwitch()           // This function is specifically for the last credits scene, which will switch to a menu prompt to either return to menu or restart from checkpoint
    {
        thisCreditScene.CrossFadeAlpha(0, fadeOutTime, false);

        Invoke("ReturnToMenu", switchTime);
    }

    public void ReturnToMenu()              // This is the function that does the menu panel activation for LastPanelSwitch(), after a delay from Invoke()
    {
        closingPanel.SetActive(true);

        this.GetComponent<UIPanelSwitch>().ActivateButton();                                    // Activates a button on the panel, important for controller support
    }
}
