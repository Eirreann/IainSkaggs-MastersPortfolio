using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// This script transitions the Player and active Level boundary after fading to black when the player enters the trigger collider
public class IntroZoneTransition : MonoBehaviour
{
    public GameObject cam;
    public Transform player;
    public Animator fadePanel;

    public Collider nextLvl;
    public Transform nextPlayerPos;

    private CinemachineConfiner cineCamSettings;

    void Start()
    {
        cineCamSettings = cam.GetComponent<CinemachineConfiner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            fadePanel.SetTrigger("FadeIn");
            Invoke("TransitionZone", 2);            
        }
    }

    private void TransitionZone()
    {
        cineCamSettings.m_BoundingVolume = nextLvl;
        player.position = nextPlayerPos.position;
        fadePanel.SetTrigger("FadeOut");
    }
}
