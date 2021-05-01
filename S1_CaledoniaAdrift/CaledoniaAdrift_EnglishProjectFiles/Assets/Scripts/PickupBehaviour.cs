using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupBehaviour : MonoBehaviour {

    public string pickUpName;               // The name of the pickup (for debugging purposes primarily)
    public GameObject pickUpUI;             // The in-menu UI element of the target pickup
    public GameObject emptyUI;              // The empty placeholder UI that will be replaced
    public GameObject pickupConfirm;        // The confirmation UI element that shows up when the player collects a pickup
    public GameObject parentObject;         // The parent object of the pickup (I realise I could probably make this private and grab it in the Start method, if I have time I will)

    private AudioSource soundEffect;        // The sound effect that plays when the pickup is collected
    private Image confirmImage;             // The Image component of the pickup confirmation object

    private void Start()                    // Assigning variables to components...
    {
        soundEffect = parentObject.GetComponent<AudioSource>();

        confirmImage = pickupConfirm.GetComponent<Image>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))              // If the player collects the pickup...
        {
            soundEffect.Play(0);                                // ...play the pickup audio...

            this.gameObject.SetActive(false);                  // ...deactivate the pickup GameObject...

            UIMenuHandler();                                    // ...make the pickup visible in the 'Discoveries' menu and activate the confirmation display...

            Invoke("PickupConfirmHandler", 3);                  // ...kill the confirmation UI after 3 seconds...

            Debug.Log("Pickup " + pickUpName + " Acquired");    // ...and tell the log that the pickup's been collected.
        }
    }

    private void UIMenuHandler()                                // A method that activates the pickup UI in the 'Discoveries' menu and activates the confirmation UI
    {
        pickUpUI.SetActive(true);

        emptyUI.SetActive(false);

        pickupConfirm.SetActive(true);
    }

    private void PickupConfirmHandler()                         // A method that fades out and kills the confirmation UI when called
    {
        confirmImage.CrossFadeAlpha(0, 1, false);

        Invoke("KillUI", 1);
    }

    private void KillUI()                                       // The mehod that deactivates the the confirmation UI, used in the Invoke() above.
    {
        pickupConfirm.SetActive(false);
    }
}
