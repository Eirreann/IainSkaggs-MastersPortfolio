using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script keeps the player on a moving platform by making the player GameObject a child object of the platform
public class KeepPlayerOnPlatforms : MonoBehaviour {

    // Thanks to Jayanam on YouTube for the below method of keeping the player on the platform.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))              // If the player enters the trigger area...
        {
            other.gameObject.transform.parent = transform;      // ...make this platform the parent object of the player.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))              // If the player leaves the trigger area...
        {
            other.gameObject.transform.parent = null;           // ...remove the player from the platform.
        }
    }
}
