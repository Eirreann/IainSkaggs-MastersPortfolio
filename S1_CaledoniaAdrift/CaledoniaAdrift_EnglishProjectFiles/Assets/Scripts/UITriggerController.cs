using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITriggerController : MonoBehaviour {

    public GameObject player;                               // Grabbing the player
    public GameObject currentCheckpoint;                    // Grabbing the current respawn location
    public GameObject uiElement;                            // The UI prompt that appears when the player enters the trigger area
    public GameObject infoPanel;                            // The information pop-up when a player activates a Stone
    public GameObject popUpController;                      // The pop-up controller (so that multiple menus aren't active at once)
    public GameObject menuUI;                               // The UI element in the collectables menu
    public GameObject emptyUI;                              // The placeholder UI element to disable upon activation
    public GameObject stoneLight;                           // The blue light that indicates that the stone has been activated

    private string activateButton;                          // The controller/keyboard button that the player presses to activate the Stone
    private Vector3 spawnerOffset;                          // A minor offset so that the player doesn't spawn inside the Stone
    private AudioSource soundEffect;                        // Grabbing the sound effect that plays when the Stone is activated
    private Light lightSource;                              // The light source on the Stone
    private Image uiImage;                                  // The Image component in the UI element that appears prompting the player to activate the stone
    private bool hasActivated = false;

    void Start () {
        activateButton = "Activate";                        // Assigning the activate button

        spawnerOffset = new Vector3(0, 0, -2);              // Defining the spawner offset

        uiImage = uiElement.GetComponent<Image>();          // Assigning the Image component from the UI pop-up to the image variable

        soundEffect = GetComponent<AudioSource>();          // Grabbing the sound component

        lightSource = stoneLight.GetComponent<Light>();     // Grabbing the light component from the stone
    }


    private void OnTriggerEnter(Collider other)
    {
        // Activate the hover text when the player walks into the collision trigger area
        if(popUpController.activeSelf == false && hasActivated == false)
        {
            uiElement.SetActive(true);
        }
        else
        {
            return;
        }
            
    }

    private void OnTriggerStay(Collider other)                                                      // While the player is in the collision area...
    {
        if (infoPanel.activeSelf == false && hasActivated == false)                                 // ...if the infoPanel isn't currently active...
        {
            if (popUpController.activeSelf == false && Input.GetButtonDown(activateButton))         // ...and if there is no other menu active and the player presses the activate button...
            {
                currentCheckpoint.transform.position = this.transform.position + spawnerOffset;     // ...set the respawn point to the activated Stone...

                uiElement.SetActive(false);                                                         // ...deactivate the hover text...

                ActivePopup();                                                                      // ...activate the pop-up...

                ActivateMenuItem();                                                                 // ...and add the stone to the 'Discoveries' menu.

                // Send the new spawn location to the console for debugging. ^_^
                Debug.Log(currentCheckpoint.transform.position.ToString("F4"));
            }

        } else if (infoPanel.activeSelf == true)                                                    // Otherwise, if the infoPanel *is* active...
        {
            if (Input.GetButtonDown(activateButton))                                                // ...if the player presses the activate button...
            {
                InactivePopup();                                                                    // ...disable the pop-up window...

                popUpController.SetActive(false);                                                   // ...and turn off the pop-up controller to let the game know that no menu is now active.

                hasActivated = true;
            }
            
        }

        // Be sure to turn off the hover text if the pause menu is activated
        if (popUpController.activeSelf == true)
        {
            uiElement.SetActive(false);
        }
        else if (popUpController.activeSelf == false && hasActivated == false)
        {
            uiElement.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(uiElement.activeSelf == true)
        {
            // Deactivates the hover text when the player walks out of the collision trigger area
            uiImage.CrossFadeAlpha(0, 1, false);

            Invoke("KillUI", 1);
        }
    }


    // The method that activates the pop-up window.
    private void ActivePopup()
    {
        infoPanel.SetActive(true);                                                  // Enable the pop-up window

        popUpController.SetActive(true);                                            // Enables the popup controller

        soundEffect.Play(0);                                                        // Play the audio

        player.GetComponent<PlayerRespawnTracker>().DeactivatePlayerControl();      // Disable player movement
    }


    // the method that deactivates the pop-up window.
    private void InactivePopup()
    {
        infoPanel.SetActive(false);                                                 // Disable the pop-up window

        player.GetComponent<PlayerRespawnTracker>().ReactivatePlayerControl();      // Enable player movement
    }

    private void ActivateMenuItem()
    {
        lightSource.enabled = true;                                                 // Turn on the light...

        menuUI.SetActive(true);                                                     // ...activate the space in the 'Discoveries' menu...

        emptyUI.SetActive(false);                                                   // ...and deactivate the placeholder element.
    }

    private void KillUI() // The method by which the UI element is murderised
    {
        uiElement.SetActive(false);
    }
}
