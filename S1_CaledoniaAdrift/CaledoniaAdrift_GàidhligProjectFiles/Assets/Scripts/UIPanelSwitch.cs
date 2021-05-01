using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// A script to manage switching between many different panels when using a controller
public class UIPanelSwitch : MonoBehaviour {

    public GameObject currentPanel;         // The currently attached UI panel
    public GameObject nextPanel;            // The UI panel to be navigated to
    public GameObject selectedButton;       // The button to be set as active in the next panel


    public void Update()        // Remains of my attempt to get the B button working for navigating backward out of menus; it broke too many things to keep.
    {                           // Would love to figure out if time permitting
        /*
        if (Input.GetButtonDown("Cancel"))
        {
            PanelSwitch();
        } */
    }

    // Deactivate one panel, activate another panel, and set the target button as the currently selected button for the controller.
    public void PanelSwitch()
    {
        currentPanel.SetActive(false);
        nextPanel.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(selectedButton, null);
    }

    // Set the target button as the currently selected button for the controller without activating/deactivating anything.
    public void ActivateButton()
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(selectedButton, null);
    }
}
