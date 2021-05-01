using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A very small script to make the target GameObject activate when the player hits the assigned collider
public class OnImpact : MonoBehaviour {

    public GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target.SetActive(true);
        }
    }
}
