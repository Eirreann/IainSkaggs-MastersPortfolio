using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A quick 'n' simple script to switch the FollowCam from following the Player to moving along the X-axis to the Animator in order to trigger the End Credits.
public class EndCameraTrigger : MonoBehaviour
{
    public GameObject uiTracker;
    public GameObject followCam;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            followCam.GetComponent<CamFollow>().enabled = false;
            uiTracker.SetActive(true);
            followCam.GetComponent<Animator>().enabled = true;
        }


    }
}
