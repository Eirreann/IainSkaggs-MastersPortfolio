using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class IntroZoneFlipPath : MonoBehaviour
{
    public GameObject cam;
    public Transform player;
    public Animator fadePanel;

    public Transform nextPlayerPos;

    public GameObject currentTransitions;
    public GameObject targetTransitions;

    private CinemachineConfiner cineCamSettings;

    void Start()
    {
        cineCamSettings = cam.GetComponent<CinemachineConfiner>();
    }

    // Flips the player around and resets all level transition points in the Intro after the player enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            fadePanel.SetTrigger("FadeIn");
            Invoke("FlipPath", 2);
        }
    }

    private void FlipPath()
    {
        player.position = nextPlayerPos.position;
        player.GetComponent<Player2DController>().Flip();
        fadePanel.SetTrigger("FadeOut");
        targetTransitions.SetActive(true);
        currentTransitions.SetActive(false);
    }
}
