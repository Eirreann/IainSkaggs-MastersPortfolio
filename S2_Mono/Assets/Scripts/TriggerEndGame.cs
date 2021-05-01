using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEndGame : MonoBehaviour
{
    public GameObject wholeMonomachine;
    public GameObject brokenMonomachine;

    public GameObject workingScanner;
    public GameObject brokenScanner;

    public GameObject oldDoor;
    public GameObject newDoor;

    public GameObject playerRoleObj;
    public GameObject ambientSound;

    public AudioClip fallingFixture;
    public AudioClip crash;
    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public void FallingFixture()
    {
        audioSrc.clip = fallingFixture;
        audioSrc.Play();
    }
    
    // When the EndGame is triggered, deactivate and activate the appropriate objects after playing the crash noise
    public void EndGame()
    {
        GetComponent<Animator>().enabled = false;

        audioSrc.clip = crash;
        audioSrc.Play();

        wholeMonomachine.SetActive(false);
        workingScanner.SetActive(false);
        oldDoor.SetActive(false);
        ambientSound.SetActive(false);

        brokenMonomachine.SetActive(true);
        brokenScanner.SetActive(true);
        newDoor.SetActive(true);

        Invoke("Deactivate", 2f);       // Invoked method to deactivate this object and the Player's Role object, because it was cutting the audio source short otherwise.
    }

    private void Deactivate()
    {
        playerRoleObj.SetActive(false);
        gameObject.SetActive(false);
    }
}
