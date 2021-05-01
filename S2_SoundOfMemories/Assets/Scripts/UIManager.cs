using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ModifiedControllers;

// This script manages most UI interactions in the game
public class UIManager : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCrosshair;
    public GameObject uiChecker;
    public GameObject pauseMenu;
    public GameObject fadePanel;
    public GameObject finalNote;

    private Animator fadeAnim;
    private AudioSource pauseAudio;
    private int noteCounter;

    private bool isTransitioned;

    // Start is called before the first frame update
    void Start()
    {
        fadeAnim = fadePanel.GetComponent<Animator>();
        pauseAudio = GetComponent<AudioSource>();
        fadeAnim.SetTrigger("FadeOut");
    }

    // Update is called once per frame
    void Update()
    {
        // If there is a Player in this scene (i.e. the game proper)
        if(player != null)
        {
            // If the UI checker is active, turn on the mouse cursor and disable player movement
            if (uiChecker != null && uiChecker.activeSelf == true)
            {
                ActiveCursor(true);
                ActivePlayer(false);
            }
            // If the UI checker is inactive, return control to the player and hide the mouse
            else if (uiChecker != null && uiChecker.activeSelf == false)
            {
                ActiveCursor(false);
                ActivePlayer(true);
            }

            // If the ESC button is pressed, activate the pause menu and sound and deactivate player movement
            if (Input.GetButton("Cancel") && uiChecker.activeSelf == false)
            {
                pauseAudio.Play();
                pauseMenu.SetActive(true);
                uiChecker.SetActive(true);
            }
        }        
    }

    // A method for setting the mouse cursor's active state
    public void ActiveCursor(bool cursorActive)
    {
        if (cursorActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (!cursorActive)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }
    }

    // A method for setting the player's active state
    public void ActivePlayer(bool playerActive)
    {
        if (playerActive)
        {
            player.GetComponent<FirstPersonController>().enabled = true;
            playerCrosshair.SetActive(true);
        }
        else if (!playerActive)
        {
            player.GetComponent<FirstPersonController>().enabled = false;
            playerCrosshair.SetActive(false);
        }
    }

    // A method to keep track of how many notes the player has read, triggers level exit upon 3 notes
    public void IncreaseNoteCount()
    {
        noteCounter++;
        if(noteCounter == 3)
        {
            finalNote.SetActive(true);
        }
    }

    // A method for fading into the game scene
    public void FadeToGame()
    {
        fadeAnim.SetTrigger("FadeIn");
        Invoke("TargetGameScene", 2);
    }

    // A method for fading into the menu scene
    public void FadeToMenu()
    {
        fadeAnim.SetTrigger("FadeIn");
        Invoke("TargetMenuScene", 2);
    }

    // A method for invoking the game scene
    private void TargetGameScene()
    {
        ChangeScene("01_Main");
    }

    // A method for invoking the menu scene
    private void TargetMenuScene()
    {
        ChangeScene("00_Menu");
    }

    // A method that changes the scene to a defined target
    public void ChangeScene(string nextScene)
    {
        Debug.Log("Changing to scene: " + nextScene);
        SceneManager.LoadScene(nextScene);
    }

    // A method for fading out before quitting the application
    public void FadeToQuit()
    {
        fadeAnim.SetTrigger("FadeIn");
        Invoke("Quit", 2);
    }

    // A method that quits the application
    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
