using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class IntroZoneActivateTransition : MonoBehaviour
{
    public GameObject cam;
    public Transform player;
    public Animator fadePanel;

    public Collider nextLvl;
    public Transform nextPlayerPos;

    private CinemachineConfiner cineCamSettings;
    private Material thisMaterial;
    private Color originalColor;

    void Start()
    {
        cineCamSettings = cam.GetComponent<CinemachineConfiner>();
        thisMaterial = GetComponent<Renderer>().material;
        originalColor = thisMaterial.color;
    }

    // If the player enters the trigger area and presses Activate, trigger the zone transition.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            thisMaterial.color = Color.red;
            if (Input.GetButtonDown("Activate"))
            {
                fadePanel.SetTrigger("FadeIn");
                Invoke("TransitionZone", 2);
            }            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            thisMaterial.color = originalColor;
        }
    }

    // Transitions the player and Level Boundary from the current level to the target level.
    private void TransitionZone()
    {
        cineCamSettings.m_BoundingVolume = nextLvl;
        player.position = nextPlayerPos.position;
        fadePanel.SetTrigger("FadeOut");
    }
}
