using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    #region Variables
    public int maxDistance = 3;
    public Transform pickupPosition;
    public GameObject crosshair;
    public GameObject goggles;
    public GameObject uiTracker;
    private Camera cam;

    [HideInInspector]
    public Transform pickupObj;

    private Transform pickupParent;
    private AudioSource pickupAudio;
    private GameObject interactObj;
    private AudioSource playerAudio;
    private AudioSource interactAudio;
    private Light interactLight;

    private bool isCasting = true;
    private bool isPickedUp = false;

    [HideInInspector]
    public bool recordPlayerisOn = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();

        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Bit shift the index of the layer (9) to get a bit mask
        int layerMask = 1 << 9;

        // Send out a raycast from the centre of the screen
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance, layerMask) && isCasting)
        {
            // If the looked-at object is a pickup, assign it and grab its parent object
            if (hit.transform.gameObject.CompareTag("PickUp"))
            {
                pickupObj = hit.transform;

                pickupParent = pickupObj.parent;
            }
            else
            {
                // On the other hand if its an interactable object only, assign it and grab its audio source
                interactObj = hit.transform.gameObject;

                interactAudio = interactObj.GetComponent<AudioSource>();
            }

            // If its a light, also grab its light source
            if (hit.transform.gameObject.CompareTag("Light"))
            {
                interactLight = interactObj.GetComponent<Light>();
            }

            // If its the "Time Goggles", let the player pick them up by pressing the Interact button
            if (hit.transform.gameObject.CompareTag("Goggles") && Input.GetButtonDown("Interact"))
            {
                playerAudio.Play();
                hit.transform.gameObject.SetActive(false);
                goggles.SetActive(true);
            }

            // If its a stare-triggered object, play the audio source while the player is looking at it
            if (hit.transform.gameObject.CompareTag("StareTrigger"))
            {
                interactObj = hit.transform.gameObject;

                interactAudio = interactObj.GetComponent<AudioSource>();

                if (!interactAudio.isPlaying)
                {
                    interactAudio.Play();
                }
            }

            print("I'm looking at " + hit.transform.name);
        }
        else
        {
            // Stop the stare-triggered sound from playing on look-away
            if(interactObj != null && interactObj.tag == "StareTrigger")
            {
                interactAudio.Stop();
            }

            // Reset the pickup object variable if the object isn't currently in the player's hand
            if (!isPickedUp)
            {
                pickupObj = null;
            }

            // Reset the interact object variable
            interactObj = null;
        }

        if (Input.GetButtonDown("Interact"))
        {
            // If the player isn't already holding a pickup, pick up the object and play the sound
            if (pickupPosition.gameObject.activeSelf == false && pickupObj != null)
            {
                playerAudio.Play();
                PickUpObj();
            }
            // otherwise, drop the held object
            else if(pickupPosition.gameObject.activeSelf == true)
            {
                DropObj();
            }

            // If there is an interact object...
            if(interactObj != null)
            {
                // If it's a generic object, just play the sound
                if (interactObj.CompareTag("PlayAudio"))
                {
                    interactAudio.Play();
                }
                // If it's the Harp, make sure the player can't trigger the sound to restart until its played out
                else if (interactObj.CompareTag("Harp") && !interactAudio.isPlaying)
                {
                    interactAudio.Play();
                }
                // If its a light, grab the Audio Clip Register from the light and play the appropriate on/off sound
                else if (interactObj.CompareTag("Light"))
                {
                    var audioClips = interactObj.GetComponent<AudioClipRegister>().clipList;
                    if (interactLight.enabled)
                    {
                        interactAudio.clip = audioClips[0];
                        interactAudio.Play();
                    }
                    else if (!interactLight.enabled)
                    {
                        interactAudio.clip = audioClips[1];
                        interactAudio.Play();
                    }

                    interactLight.enabled = !interactLight.enabled;
                }
                // If its a record player (or gramaphone?) switch on the audio source if it isn't already on, 
                // and tell the RecordPlayerTrigger script that it is active and ready to receive a vinyl record
                else if(interactObj.CompareTag("Record Player"))
                {
                    if (!interactAudio.isPlaying)
                    {
                        interactAudio.Play();
                        recordPlayerisOn = true;
                    }
                    else if (interactAudio.isPlaying)
                    {
                        interactAudio.Stop();
                        recordPlayerisOn = false;
                    }
                }
                // If its the old lamp in Zone 3, enable a separate light component that has the "buzz" audio clip attached
                // in addition to basic on/off functionality
                else if (interactObj.CompareTag("Old Lamp"))
                {
                    Transform lightObj;
                    lightObj = interactObj.transform.GetChild(0);
                    var audioClips = interactObj.GetComponent<AudioClipRegister>().clipList;
                    if (lightObj.gameObject.activeSelf == false)
                    {
                        interactAudio.clip = audioClips[0];
                        interactAudio.Play();

                        lightObj.gameObject.SetActive(true);
                    }
                    else
                    {
                        interactAudio.clip = audioClips[1];
                        interactAudio.Play();

                        lightObj.gameObject.SetActive(false);
                        lightObj = null;
                    }
                }
                // If its a handwritten note, play the pickup audio, activate the relevant UI panel for the note, and deactivate player movement
                else if(interactObj.tag == "ReadNote")
                {
                    interactAudio.Play();
                    interactObj.GetComponent<NotePickup>().targetNote.SetActive(true);
                    uiTracker.SetActive(true);                  
                }
            }
        }
    }

    // A method that turns off the interaction raycast and sets the pickup object to the player's "Held" position
    void PickUpObj()
    {
        isCasting = false;
        crosshair.SetActive(false);
        pickupPosition.gameObject.SetActive(true);
        pickupObj.GetComponent<Rigidbody>().isKinematic = true;
        pickupObj.parent = pickupPosition;
        pickupObj.localPosition = Vector3.zero;
        pickupObj.localRotation = Quaternion.Euler(0, 0, 90);
        isPickedUp = true;
    }

    // A method that disassociates the pickup from the player when placing it in the record player, before RecordPlayerTrigger takes over
    public void PlayRecord()
    {
        pickupObj.transform.parent = null;
        pickupPosition.gameObject.SetActive(false);
        crosshair.SetActive(true);
        isCasting = true;
        pickupObj = null;
        isPickedUp = false;
    }

    // A method that resets the pickup to its original parent object and releases it from the held position
    public void DropObj()
    {
        pickupObj.transform.parent = pickupParent;
        pickupObj.GetComponent<Rigidbody>().isKinematic = false;
        pickupPosition.gameObject.SetActive(false);
        crosshair.SetActive(true);
        isCasting = true;
        pickupObj = null;
        isPickedUp = false;
    }
}
