using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOutUI : MonoBehaviour {

    public string targetScene;                  // The scene to switch to when fade in/out is complete

    public float delay;                         // The delay before changing scenes

    public GameObject tutorialUI;               // The tutorial UI that is activated when the player enters a trigger area
    public GameObject popUpController;          // The ever-important pop-up controller, to tell the game when a panel or menu is active

    private Image tutorialImage;                // The image component of the tutorial UI

    private void Start()
    {
        // Assigning the image component
        tutorialImage = tutorialUI.GetComponent<Image>();
    }

    void OnGameLaunch()                             // When the fade in animation has finished, disable the FadeIn panel GameObject
    {
        this.gameObject.SetActive(false);
    }

    public void OnButtonPress()                     // On the press of a button, enable the FadeOut panel GameObject and invoke the ChangeScene() method after a delay
    {
        this.gameObject.SetActive(true);

        Invoke("ChangeScene", delay);
    }


    public void ChangeScene()                       // Load the next scene
    {
        SceneManager.LoadScene(targetScene);

        Debug.Log("Changing scene...");
    }

    private void OnTriggerEnter(Collider other)
    {
        tutorialUI.SetActive(true);                 // Set the tutorial UI to active when the player enters the trigger area
    }

    private void OnTriggerStay(Collider other)
    {
        if (popUpController.activeSelf == false)    // While the player is in the trigger area and no menus are open...
        {
            tutorialUI.SetActive(true);             // ...set the tutorial UI to active.
        }
        else                                        // Otherwise...
        {
            tutorialUI.SetActive(false);            // ...turn off the tutorial UI when a menu is active.
        }

    }

    private void OnTriggerExit(Collider other)      // Fade out the tutorial UI and deactivate it when the player leaves the trigger area
    {
        tutorialImage.CrossFadeAlpha(0, 1, false);

        Invoke("KillUI", 1);
    }

    private void KillUI()                           // The method that kills the UI panel in the Invoke method
    {
        tutorialUI.SetActive(false);
    }
}
