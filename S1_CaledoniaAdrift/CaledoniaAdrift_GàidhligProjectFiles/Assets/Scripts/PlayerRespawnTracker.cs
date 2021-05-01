using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// A script primarily made to manage the player's respawn position, but now does all sorts of stuff to do with the player, including re/despawning, re/deactivating control, and much else besides
public class PlayerRespawnTracker : MonoBehaviour {

    public GameObject playerCam;                        // The main camera (the one the player controls)
    public GameObject dangerZone;                       // The "Danger Zone" (the trigger area which respawns the player when they fall)
    public GameObject gameOverScreen;                   // The Game Over panel
    public GameObject selectedButtonGameOver;           // The button to be selected for the controller when the Game Over panel is activated
    public GameObject currentCheckpoint;                // The current respawn checkpoint
    public GameObject pauseMenu;                        // The Pause Menu panel
    public GameObject selectedButtonPauseMenu;          // The button to be selected for the controller when the Pause Menu panel is activated
    public GameObject popUpController;                  // Our ever-faithful Popup Controller!
    public Animator playerAnimator;                     // The player animator

    private Vector3 startPosition;                      // Where the respawner starts
    private GameObject player;                          // The player
    private AudioSource soundEffect;                    // The sound efffect that plays when the player hits the Danger Zone

    void Start () {                 // Assigning variables...

        playerAnimator = GetComponent<Animator>();

        player = this.gameObject;

        soundEffect = dangerZone.GetComponent<AudioSource>();

        // A log to keep track of the checkpoint respawn coordinates for debugging purposes
        Debug.Log(currentCheckpoint.transform.position.ToString("F4"));
    }
	
	void Update () {

        if (popUpController.activeSelf == false && Input.GetButtonDown("Start"))                    // If there isn't a menu already active and the player presses the Start button...
        {
            pauseMenu.SetActive(true);                                                              // ...activate the pause menu..

            popUpController.SetActive(true);                                                        // ...tell the rest of the game that a menu is active...

            GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(selectedButtonPauseMenu, null);    // ...set the top button as the active button...

            DeactivatePlayerControl();                                                              // ...and deactivate the player's control of the PC

        }
        else if (popUpController.activeSelf == true && Input.GetButtonDown("Start"))                // Otherwise, if the menu is already active...
        {
            pauseMenu.SetActive(false);                                                             // ...deactivate the menu...

            popUpController.SetActive(false);                                                       // ...let the rest of the game know that the menu is deactivated...

            ReactivatePlayerControl();                                                              // ...and give the player control back.
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DangerZone"))                                              // If the player hits the "Danger Zone" trigger...
        {
            soundEffect.Play(0);                                                                    // ...play the sound effect...

            DespawnPlayer();                                                                        // ...despawn the player...
               
            gameOverScreen.SetActive(true);                                                         // ...activate the Game Over screen...

            GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(selectedButtonGameOver, null);     // ...and set the appropriate menu button active.

        }

    }

    // (Re)Actiave the player and camera GameObject at the last checkpoint, and dismiss the Game Over screen. (i.e. respawn)
    public void RespawnPlayer()
    {
        player.SetActive(true);

        playerCam.GetComponent<ThirdPersonOrbitCamBasic>().enabled = true;

        player.transform.position = currentCheckpoint.transform.position;

        gameOverScreen.SetActive(false);
    }

    // Deactivate the player and camera GameObjects (i.e. despawn)
    private void DespawnPlayer()
    {
        player.SetActive(false);

        playerCam.GetComponent<ThirdPersonOrbitCamBasic>().enabled = false;
    }

    public void DeactivatePlayerControl()                   // Take away the player's control of the PlayerCharacter:
    {
        playerAnimator.SetFloat("Speed", 0);                                // Set player movement speed to 0.

        player.GetComponent<BasicBehaviour>().enabled = false;              // Turn off movement scripts on the player
        player.GetComponent<MoveBehaviour>().enabled = false;               // Turn off movement scripts on the player

        playerCam.GetComponent<ThirdPersonOrbitCamBasic>().enabled = false; // Turn off movement scripts on the camera
    }

    public void ReactivatePlayerControl()                   // Return the player's control of the PlayerCharacter.
    {
        player.GetComponent<BasicBehaviour>().enabled = true;               // Turn on movement scripts on the player
        player.GetComponent<MoveBehaviour>().enabled = true;                // Turn on movement scripts on the player

        playerCam.GetComponent<ThirdPersonOrbitCamBasic>().enabled = true;  // Turn on movement scripts on the camera
    }

}
