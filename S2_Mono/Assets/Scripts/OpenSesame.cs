using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSesame : MonoBehaviour
{
    public string keyTag;

    public GameObject activeObj;
    public GameObject inactiveObj;

    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // If the correct Key item enters the collision area, trigger the barrier open and destroy the pickup.
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == keyTag)
        {
            audioSrc.Play();

            activeObj.SetActive(false);
            inactiveObj.SetActive(true);

            other.GetComponent<PickupBehaviour>().Used();
        }
    }
}
