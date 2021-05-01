using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple script that activates tooltips when the Player enters the trigger collider
public class TriggerTooltip : MonoBehaviour
{
    public GameObject targetTooltip;
    public GameObject uiTracker;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            targetTooltip.SetActive(true);
            uiTracker.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
