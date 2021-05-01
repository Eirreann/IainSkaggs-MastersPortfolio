using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleScanner : MonoBehaviour
{
    #region Variables
    public string roleRestriction;
    public GameObject gateComponent;
    public GameObject scannerObj;
    public GameObject indicator;

    public GameObject playerRole;

    public float neutralLightIntensity = 5;
    public float activatedLightItensity = 15;

    public AudioClip approved;
    public AudioClip rejected;
    private AudioClip originalClip;

    private AudioSource audioSrc;
    private GameObject currentRole;
    private Light scannerLight;
    private SpriteRenderer indicatorScreen;
    private Color startingColor;
    private Color startingIndicator;

    private bool hasAccepted = false;
    private bool hasRejected = false;
    #endregion

    private void Start()
    {
        scannerLight = scannerObj.GetComponent<Light>();
        startingColor = scannerLight.color;

        indicatorScreen = indicator.GetComponent<SpriteRenderer>();
        startingIndicator = indicatorScreen.color;

        audioSrc = GetComponent<AudioSource>();
        originalClip = audioSrc.clip;
        audioSrc.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            // Finding the currently equipped Player Role
            for (int i = 0; i < playerRole.transform.childCount; i++)
            {
                if (playerRole.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    currentRole = playerRole.transform.GetChild(i).gameObject;
                }
            }

            if (currentRole != null && currentRole.tag != roleRestriction)          // Lock the gate if the equipped Role is not the required Role
            {
                Invoke("LockedGate", 0.5f);
            }
            else if (currentRole != null && currentRole.tag == roleRestriction)     // Open the gate if the equipped Role is the required Role
            {
                Invoke("OpenedGate", 0.5f);
            }
            else
            {
                Invoke("LockedGate", 0.5f);                                         // Or return the Scanner gate to a neutral state.
            }
        }        
    }

    // Resets the gate to a neutral state if the Player leaves the detection area.
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Invoke("GateNeutral", 0.5f);
            audioSrc.PlayOneShot(originalClip);
            Invoke("ResetAudio", 3);
        }        
    }

    // Turns the scanner light red, plays the rejected audio clip, and closes the Gate
    private void LockedGate()
    {
        if (!hasAccepted)
        {
            audioSrc.PlayOneShot(rejected);
            hasAccepted = true;
        }

        scannerLight.color = Color.red;
        scannerLight.intensity = activatedLightItensity;
        indicatorScreen.color = Color.red;
        gateComponent.SetActive(true);
    }

    // Turns the scanner light green, plays the accepted audio clip, and pens the Gate
    private void OpenedGate()
    {
        if (!hasAccepted)
        {
            audioSrc.PlayOneShot(approved);
            hasAccepted = true;
        }

        scannerLight.color = Color.green;
        scannerLight.intensity = activatedLightItensity;
        indicatorScreen.color = Color.green;

        gateComponent.SetActive(false);
    }

    // Resets all variables to their neutral state
    private void GateNeutral()
    {
        scannerLight.color = startingColor;
        scannerLight.intensity = neutralLightIntensity;
        indicatorScreen.color = startingIndicator;

        gateComponent.SetActive(true);
    }

    // Resets the Audio clips after a period of time, to prevent a repeated loop of the same noises playing.
    private void ResetAudio()
    {
        hasAccepted = false;
        hasRejected = false;
    }
}
