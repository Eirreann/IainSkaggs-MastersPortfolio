using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFungusFlowchart : MonoBehaviour
{
    #region Variables
    public GameObject playerRole;
    public GameObject uiTracker;
    public GameObject squareFlowchart;
    public GameObject triangleFlowchart;
    public GameObject circleFlowchart;
    public bool isEnvironmental;
    public GameObject envFlowchart;
    public bool isIntro;

    private Material thisMat;
    private Color originalColor;

    private GameObject currentRole;
    private GameObject currentFlowchart;
    private bool hasActivated = false;
    #endregion

    private void Start()
    {
        if (isEnvironmental)                                    // A condition for Fungus flowcharts attached to interactable objects rather than NPCs
        {
            thisMat = GetComponent<Renderer>().material;
            originalColor = thisMat.color;
        }        
    }

    private void Update()
    {
        if(playerRole != null)
        {
            // Check for the current Player role
            for (int i = 0; i < playerRole.transform.childCount; i++)
            {
                if (playerRole.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    currentRole = playerRole.transform.GetChild(i).gameObject;
                }
            }

            // Assigns the relevant flowchart dependent on active Player role
            if (currentRole != null)
            {
                if (currentRole.tag == "Square")
                {
                    currentFlowchart = squareFlowchart;
                }
                else if (currentRole.tag == "Triangle")
                {
                    currentFlowchart = triangleFlowchart;
                }
                else if (currentRole.tag == "Circle")
                {
                    currentFlowchart = circleFlowchart;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && hasActivated == false)      // Activates the appropriate flowchart, and disables re-activation for 5 seconds;
        {
            if (playerRole != null)
            {
                currentFlowchart.SetActive(true);
                uiTracker.SetActive(true);
                hasActivated = true;
                Invoke("ActivateTimer", 5f);
            }
        }
        if (other.tag == "Player" && isIntro && hasActivated == false) // The same as above, but for the specific Intro NPC flowchart
        {
            envFlowchart.SetActive(true);
            uiTracker.SetActive(true);
            hasActivated = true;
            Invoke("ActivateTimer", 5f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && isEnvironmental)        // For interactable flowcharts, highlights the interactable object Red
        {
            thisMat.color = Color.red;

            if (Input.GetButtonDown("Activate"))
            {
                envFlowchart.SetActive(true);
                uiTracker.SetActive(true);
            }
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (isEnvironmental)                // For interactable flowcharts, resets the interactable object's material colour.
        {
            thisMat.color = originalColor;
        }
    }

    // A simple method to prevent re-activation of a flowchart until a set amount of time using Invoke()
    private void ActivateTimer()
    {
        hasActivated = false;
    }
}
