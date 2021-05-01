using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class manages some basic UI functions, like fading in/out to black, switching scenes, reloading scenes, and closing the game.
public class UIManager : MonoBehaviour
{
    public Animator fadePanel;                          // The Animator on the UI panel element that fades in/out from black   
    public float fadeTime = 1;                          // The time (in seconds) that it takes to fade fully in/out from black

    private Scene currentScene;                         // The currently active game scene
    private string targetScene;                         // A string to represent the target scene to change to

    void Start()
    {
        Invoke("FadeOut", fadeTime);                    // Fade from black upon the start of a scene
        currentScene = SceneManager.GetActiveScene();   // Grabs the current scene for use later
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))           // If the DELETE key is pressed...
            QuitButton();                               // ...call the QuitButton() method and quit the game.

        if (Input.GetKeyDown(KeyCode.R))                // If the R key is pressed...
            Reload();                                   // ...call the Reload() method to reload the current scene.
    }

    // A method that triggers the FadePanel to fade into black
    public void FadeIn()
    {
        fadePanel.SetTrigger("FadeIn");                 // Trigger the FadeIn animation on the FadePanel animator
        fadePanel.ResetTrigger("FadeOut");              // Reset the FadeOut trigger
    }

    // A method that triggers the FadePanel to fade out from black
    public void FadeOut()
    {
        fadePanel.SetTrigger("FadeOut");                // Trigger the FadeOut animaton on the FadePanel animator
        fadePanel.ResetTrigger("FadeIn");               // Reset the FadeIn trigger
    }

    // A method that loads the specificed target scene after fading to black
    public void ChangeScene(string nextScene)
    {
        targetScene = nextScene;                        // Set the input scene as the target scene to change to
        fadePanel.SetTrigger("FadeIn");                 // Trigger the FadeIn animation on the FadePanel animator
        Invoke("FadeScene", fadeTime);                  // Invoke the FadeScene() method to load the next scene after after fading out
    }

    // A method called in ChangeScene to load a scene after the FadePanel animator plays
    private void FadeScene()
    {
        SceneManager.LoadScene(targetScene);            // Load the target scene
        print("Changing scene to... " + targetScene);   // Print to the console to verify that the scene is being loaded
    }

    // A method that reloads the current scene after fading to black
    public void Reload()
    {
        fadePanel.SetTrigger("FadeIn");                 // Trigger the FadeIn animation on the FadePanel animator
        Invoke("ReloadScene", fadeTime);                // Invoke the ReloadScene() method to reload the current scene after fading out
    }

    // A method called in Reload() to reload the current scene after the FadePanel animator plays
    void ReloadScene()
    {
        SceneManager.LoadScene(currentScene.name);      // Reload the current scene
    }

    // A method that quits the application after fading to black
    public void QuitButton()
    {
        fadePanel.SetTrigger("FadeIn");                 // Trigger the FadeIn animation on the FadePanel animator
        Invoke("Quit", fadeTime);                       // Invoke the Quit() method to quit the game after fading out
    }

    // A method that quits the application
    public void Quit()
    {
        print("Quitting...");                           // Print to the console that the application is quitting
        Application.Quit();                             // Quit the application
    }
}
