using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Variables
    public GameObject player;
    public GameObject uiTracker;
    public Animator fadePanel;
    public float fadeTime = 1;
    public GameObject pauseMenu;
    public string targetScene;
    public bool mainMenu = false;

    private bool fadeToScene = false;
    private Scene currentScene;

    [HideInInspector] public bool isReadyToStart = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Invoke("FadeOut", fadeTime);                                // Fade from black upon the start of a scene
        currentScene = SceneManager.GetActiveScene();               // Grabs the current scene for use later
    }

    // Update is called once per frame
    void Update()
    {
        // Disable the mouse if the Main Menu is active, in addition to setting up the Spacebar to start the game, and the Escape key to exit
        if (mainMenu)
        {
            ActiveCursor(false);

            if (Input.GetButtonDown("Jump"))
            {
                fadePanel.SetTrigger("FadeIn");
                fadeToScene = true;
            }

            if (Input.GetButtonDown("Cancel"))
            {
                Quit();
            }

            if (fadePanel.GetCurrentAnimatorStateInfo(0).IsName("Visible") && fadeToScene == true)
            {
                fadeToScene = false;
                ChangeScene(targetScene);
            }
        }

        // Activate the pause menu (if eligible) 
        if (Input.GetButtonDown("Pause") && pauseMenu != null)
        {
            PauseMenu();
        }

        // Deactivates the Player controller if the UI Tracker is active
        if(player != null)
        {
            if (uiTracker.activeSelf == true)
            {
                player.GetComponent<Player2DController>().enabled = false;
                player.GetComponent<Player2DController>().StopWalking("Idle");
                ActiveCursor(true);
            }
            else if (uiTracker.activeSelf == false)
            {
                player.GetComponent<Player2DController>().enabled = true;
                ActiveCursor(false);
            }
        }        
    }

    // A script to activate the Pause menu
    void PauseMenu()
    {
        if(uiTracker.activeSelf == false)
        {
            uiTracker.SetActive(true);
            pauseMenu.SetActive(true);
        }
        else if (uiTracker.activeSelf == true)
        {
            uiTracker.SetActive(false);
            pauseMenu.SetActive(false);
        }
    }

    // A simmplified method for (de)activating the hardware cursor where needed
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

    // A method that triggers the FadePanel to fade into black
    public void FadeIn()
    {
        fadePanel.SetTrigger("FadeIn");
        fadePanel.ResetTrigger("FadeOut");
    }

    // A method that triggers the FadePanel to fade out from black
    public void FadeOut()
    {
        fadePanel.SetTrigger("FadeOut");
        fadePanel.ResetTrigger("FadeIn");
    }

    // A method that loads the specificed target scene
    public void ChangeScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
        print("Changing scene to... " + nextScene);
    }

    // A method that reloads the current level
    public void Reload()
    {
        fadePanel.SetTrigger("FadeIn");
        Invoke("ReloadScene", 2);
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(currentScene.name);
    }
    
    // A method that quits the application after fading to black
    public void QuitButton()
    {
        fadePanel.SetTrigger("FadeIn");

        Invoke("Quit", fadeTime);        
    }

    // A method that quits the application
    public void Quit()
    {
        print("Quitting...");
        Application.Quit();
    }


}
