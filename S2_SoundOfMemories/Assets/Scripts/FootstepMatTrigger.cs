using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using ModifiedControllers;

public class FootstepMatTrigger : MonoBehaviour
{
    public GameObject player;
    public AudioClip[] m_CarpetFootsteps;

    private FirstPersonController characterController;
    private AudioClip[] m_originalFootsteps;
    private AudioSource playerAudio;

    private AudioMixerGroup originalMixer;
    public AudioMixerGroup newMixer;

    // Start is called before the first frame update
    void Start()
    {
        characterController = player.GetComponent<FirstPersonController>();

        playerAudio = player.GetComponent<AudioSource>();
    }

    // Code written based on discussions with Níall Tracey
    private void OnTriggerEnter(Collider other)
    {
        //  When the player enters a surface's trigger area, record the original footstep sounds 
        // and then swap them out with the sounds for this area and assign the appropriate mixer.
        if (other.gameObject == player)
        {
            m_originalFootsteps = characterController.m_FootstepSounds;
            originalMixer = playerAudio.outputAudioMixerGroup;

            playerAudio.outputAudioMixerGroup = newMixer;
            characterController.m_FootstepSounds = m_CarpetFootsteps;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player leaves the trigger area, re-assign original footstep sounds.
        if(other.gameObject == player)
        {
            playerAudio.outputAudioMixerGroup = originalMixer;
            characterController.m_FootstepSounds = m_originalFootsteps;
        }        
    }
}
