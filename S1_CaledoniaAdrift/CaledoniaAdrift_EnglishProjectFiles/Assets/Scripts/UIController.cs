using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// A script that governs a few UI specific transitions, most of which is based on a script by Brian Loranger
public class UIController : MonoBehaviour
{

    private void Start()
    {
        // Hide the mouse (because this is a controller-only game, please-and-thank-you.
        Cursor.visible = false;
    }

    // A method that quits the application
    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    // A method that changes the scene
    public void ChangeScene(string nextScene)
    {
        Debug.Log("Changing to scene: " + nextScene);
        SceneManager.LoadScene(nextScene);
    }

}
