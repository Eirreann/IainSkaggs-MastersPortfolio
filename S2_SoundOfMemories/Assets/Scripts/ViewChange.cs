using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewChange : MonoBehaviour
{
    public GameObject timeGoggles;
    public Animator panelAnim;
    public GameObject triggerText;

    public GameObject baseZoneObjs;
    public GameObject targetZoneObjs;

    [HideInInspector]
    public bool isInZone = false;
    private Animator goggleAnim;
    private bool canSwap = true;
    private AudioSource transitionSound;
    private int originalPlayback;

    // Start is called before the first frame update
    void Start()
    {
        goggleAnim = timeGoggles.GetComponent<Animator>();
        transitionSound = GetComponent<AudioSource>();
        originalPlayback = transitionSound.timeSamples;
    }

    private void OnTriggerStay(Collider other)
    {
        // If the player is in the collision area and has the Time Goggles...
        if(other.tag == "Player" && timeGoggles.activeSelf == true)
        {
            // Enable the prompt to use the goggles
            triggerText.SetActive(true);

            // If the player presses the Transition button...
            if (Input.GetButtonDown("Transition"))
            {
                // If the player's not already in the zone, play the transition sound normally
                if (!isInZone)
                {
                    transitionSound.timeSamples = originalPlayback;
                    transitionSound.pitch = 1;
                    transitionSound.Play();
                }
                // Otherwise, play it backwards
                else if (isInZone)
                {
                    transitionSound.timeSamples = transitionSound.clip.samples - 1;
                    transitionSound.pitch = -1;
                    transitionSound.Play();
                }

                // Play the goggles animation
                goggleAnim.SetTrigger("isOn");

                // Play the fade panel animation
                panelAnim.SetTrigger("FadeIn");

                // Send the signal to transition
                canSwap = true;
            }

            // If the screen has faded to white and the signal to swap is true
            if (panelAnim.GetCurrentAnimatorStateInfo(0).IsName("Visible") && canSwap == true)
            {
                // Swap the relevant Zone and Base objects
                SwapObjs();

                // And reset the signal
                canSwap = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Turn off the use goggles prompt when player leaves the collision area
        triggerText.SetActive(false);
    }

    public void SwapObjs()
    {
        // If not already in the zone, deactivate base room objects, activate zone objects, hide the goggles, and play the appropriate animations
        if (!isInZone)
        {
            baseZoneObjs.SetActive(false);
            targetZoneObjs.SetActive(true);            
            timeGoggles.GetComponent<MeshRenderer>().enabled = false;
            goggleAnim.SetTrigger("isOff");
            panelAnim.SetTrigger("FadeOut");
            isInZone = true;
        }
        // If in a zone, do the reverse of the above.
        else if (isInZone)
        {
            timeGoggles.GetComponent<MeshRenderer>().enabled = true;
            baseZoneObjs.SetActive(true);
            targetZoneObjs.SetActive(false);
            goggleAnim.SetTrigger("isOff");
            panelAnim.SetTrigger("FadeOut");
            isInZone = false;
        }
    }
}
