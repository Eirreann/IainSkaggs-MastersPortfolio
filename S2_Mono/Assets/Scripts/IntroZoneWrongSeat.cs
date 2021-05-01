using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroZoneWrongSeat : MonoBehaviour
{
    #region Variables
    public GameObject cam;
    public Transform player;
    public Transform lvlBoundary;
    public GameObject uiTracker;

    public GameObject finalSetup;

    public GameObject oldTo;
    public GameObject oldFrom;

    public GameObject oldNpcs;
    public GameObject newNpcs;

    private Animator playerAnim;
    private Material thisMaterial;
    private Color originalColor;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        thisMaterial = GetComponent<Renderer>().material;
        originalColor = thisMaterial.color;
        playerAnim = player.GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            thisMaterial.color = Color.red;

            // If the player activates the seat, trigger the sitting animation and zoom-in, and invoke the Jolt()
            if (Input.GetButtonDown("Activate"))
            {
                uiTracker.SetActive(true);
                playerAnim.SetBool("Sit", true);
                lvlBoundary.gameObject.GetComponent<Animator>().enabled = true;
                Invoke("Jolt", 5);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            thisMaterial.color = originalColor;
        }        
    }

    // Deactivate old and activate new NPCs and transition points, and make the player stand up.
    public void Jolt()
    {
        finalSetup.SetActive(true) ;
        oldTo.SetActive(false);
        oldFrom.SetActive(false);
        oldNpcs.SetActive(false);
        newNpcs.SetActive(true);
        playerAnim.SetBool("Sit", false);
        uiTracker.SetActive(false);
    }
}
