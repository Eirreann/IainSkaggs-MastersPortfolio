using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script to for the player's max speed to walk speed while in side the opening house
public class ForceWalkSpeed : MonoBehaviour {

    public GameObject player;               // Grabing the player GameObject

    private Animator playerAnimator;        // The player's animator

    private void Start()
    {
        // Assigning the player's animator to the playerAnimator variable
        playerAnimator = player.GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)              // While the player is in the trigger area...
    {
        if (playerAnimator.GetFloat("Speed") > 0.15f)       // ...if the player's speed would be higher the walk speed (0.15f)...
        {
            playerAnimator.SetFloat("Speed", 0.15f);        // ...set the player's speed to walk speed (0.15f).
        }
    }
}
