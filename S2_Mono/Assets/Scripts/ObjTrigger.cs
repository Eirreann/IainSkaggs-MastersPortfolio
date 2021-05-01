using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjTrigger : MonoBehaviour
{
    #region Variables
    public bool isActivateTransition;

    public bool isLevelTransition;
    public Animator fadePanel;
    public string nextScene;
    public float fadeTime = 2;

    public bool isEndTrigger;
    public GameObject endRope;
    public GameObject onLever;
    public GameObject endFallingObj;

    private Material thisMaterial;
    private Color originalColor;
    #endregion

    private void Start()
    {
        thisMaterial = GetComponent<Renderer>().material;
        originalColor = thisMaterial.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Transitions the player to the target level if this object is set to being a Level Transition
        if(other.tag == "Player" && isLevelTransition)
        {
            fadePanel.SetTrigger("FadeIn");
            Invoke("ChangeLevel", fadeTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            thisMaterial.color = Color.red;

            if (Input.GetButtonDown("Activate"))
            {
                // Transitions the player after Activate has been pressed if this object is an ActivateTransition
                if (isActivateTransition)
                {
                    fadePanel.SetTrigger("FadeIn");
                    Invoke("ChangeLevel", fadeTime);
                }

                // Triggers the ending sequence if this object is the End Trigger
                if (isEndTrigger)
                {
                    endRope.SetActive(false);
                    endFallingObj.GetComponent<Animator>().enabled = true;
                    onLever.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Resets the material colour on-exit
        thisMaterial.color = originalColor;
    }

    // A copy+paste of the SceneChange() method from UIManager, seemed easier to have it here too for this script's purposes.
    private void ChangeLevel()
    {
        SceneManager.LoadScene(nextScene);
        print("Changing scene to... " + nextScene);
    }
}
