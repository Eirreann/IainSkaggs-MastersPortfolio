using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script manages fading in/out between UI elements in the Main Menu and Ending Credits
public class AnimationTransition : MonoBehaviour
{
    public GameObject menuSplashScreen;
    public Animator fadePanel;
    public bool isEnding;

    private void Start()
    {
        if (isEnding)
        {
            Invoke("FadeInAndDeactivate", 6);
        }
    }

    public void FadeInAndDeactivate()
    {
        fadePanel.SetTrigger("FadeIn");
        Invoke("MainMenuTransition", 2);
    }

    public void MainMenuTransition()
    {
        menuSplashScreen.SetActive(true);
        this.gameObject.SetActive(false);

        if (isEnding)
        {
            fadePanel.SetTrigger("FadeOut");
        }
    }
}
