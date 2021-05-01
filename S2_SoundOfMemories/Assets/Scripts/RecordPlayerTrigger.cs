using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPlayerTrigger : MonoBehaviour
{
    public GameObject interactionManager;
    public Transform recordPosition;

    private Interaction interactionScript;
    private AudioSource thisAudio;
    private AudioClip originalAudio;
    private AudioSource incomingAudio;
    private bool isFull = false;

    // Start is called before the first frame update
    void Start()
    {
        interactionScript = interactionManager.GetComponent<Interaction>();
        thisAudio = this.GetComponent<AudioSource>();
        originalAudio = thisAudio.clip;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the record player is on and there isn't already a record in it, and a pickup is placed in the collision area...
        if (other.gameObject.CompareTag("PickUp") && interactionScript.recordPlayerisOn && isFull == false)
        {
            // Grab the audio clip and mixer from the record...
            incomingAudio = other.GetComponent<AudioSource>();
            thisAudio.clip = incomingAudio.clip;
            thisAudio.outputAudioMixerGroup = incomingAudio.outputAudioMixerGroup;

            // Disassociate the record from the player...
            interactionScript.PlayRecord();

            // Set the record's position in the record player...
            other.gameObject.transform.parent = recordPosition;
            other.gameObject.transform.position = recordPosition.position;
            other.gameObject.transform.localPosition = Vector3.zero;
            other.gameObject.transform.localRotation = Quaternion.identity;

            // And play the record track.
            thisAudio.Play();

            // Also set the layer of the record player to 0 so it can't be turned off until the record is taken out.
            gameObject.layer = 0;

            // And set the record player to "full" so other records can't be put into it
            isFull = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the record is taken out of the player...
        if (other.gameObject.CompareTag("PickUp") && isFull == true)
        {
            // Stop the record's audio
            thisAudio.Stop();

            // Reset the original idle audio clip for the record player and play it
            thisAudio.clip = originalAudio;
            thisAudio.Play();

            // Set the record player to empty again
            isFull = false;

            // And put it back in the Interaction layer
            gameObject.layer = 9;
        }
    }
}
