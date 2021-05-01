using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleSwapper : MonoBehaviour
{
    #region Variables
    private GameObject heldRole;                                        // The currently held Role
    private GameObject heldRoleObj;                                     // The currently held object
    private GameObject wornRole;                                        // The currently worn Role
    private GameObject wornRoleObj;                                     // The currently worn object

    public Transform wornRolePos;                                       // The worn Role position, in the Player's head pisition
    public Transform wornRoleObjPos;                                    // The worn Role's object position, in the Player's head pisition
    public Transform heldRolePos;                                       // The held Role position, in the Player's arm position
    public Transform heldRoleObjPos;                                    // The held Role's object position, in the Player's arm position

    private AudioSource audioSrc;
    public AudioClip swapRoleSound;

    private GameObject heldRoleTarget;                                  // The Role that matches the corresponding worn Role tag, in the Player's hand position
    private GameObject wornRoleTarget;                                  // The Role that matches the corresponding held Role tag, in the Player's head position
    #endregion

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // A check to make sure that the heldRoleObj is a Role before checking via for-loop, to avoid conflict with pickups.
        if(heldRoleObjPos.gameObject.activeSelf == true)
        {
            heldRoleObj = heldRoleObjPos.GetChild(0).gameObject;
        }
        if(wornRoleObjPos.childCount != 0)
        {
            wornRoleObj = wornRoleObjPos.GetChild(0).gameObject;
        }


        // Finding the currently active worn "Role", and assigning it to the wornRole variable
        if (wornRoleObjPos.childCount != 0)
        {
            for (int i = 0; i < wornRolePos.transform.childCount; i++)
            {
                if (wornRolePos.transform.GetChild(i).gameObject.tag == wornRoleObj.tag)
                {
                    wornRole = wornRolePos.transform.GetChild(i).gameObject;

                    wornRole.SetActive(true);
                }
            }
        }

        // Finding the currently active held "Role", and assigning it to the heldRole variable
        if (heldRoleObjPos.gameObject.activeSelf == true)
        {
            for (int i = 0; i < heldRolePos.transform.childCount; i++)
            {
                if (heldRolePos.transform.GetChild(i).gameObject.tag == heldRoleObj.tag)
                {
                    heldRole = heldRolePos.transform.GetChild(i).gameObject;

                    heldRole.SetActive(true);
                }
            }
        }

        if (Input.GetButtonDown("Swap") && heldRolePos.gameObject.activeSelf == true)            // If the "Swap" button is pressed and a role is currently being held...
        {
            audioSrc.clip = swapRoleSound;
            audioSrc.Play();                                                                     // ...play the "swap" sound effect...

            SwapRoles();                                                                         // ...trigger the SwapRole method.
        }
    }

    // A method that swaps the active Worn role with the active Held role.
    // Use for-loops to compare the heldRoleObj and wornRoleObj tags against the Role tags, activating the matching Role in the head/hand position.
    void SwapRoles()
    {
        if (heldRoleObjPos.gameObject.activeSelf == true)
        {
            heldRole.SetActive(false);
            wornRole.SetActive(false);

            wornRoleObj.transform.parent = heldRoleObjPos;
            heldRoleObj.transform.parent = wornRoleObjPos;
        }
    }

    // Searches through all child objects of the defined Transform object for the defined tag, 
    // and returns the resulting GameObject assigned to the wornRoleTarget variable.
    public void GetWornRole(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                wornRoleTarget = child.gameObject;
            }
        }
    }

    // Searches through all child objects of the defined Transform object for the defined tag, 
    // and returns the resulting GameObject assigned to the heldRoleTarget variable.
    public void GetHeldRole(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                heldRoleTarget = child.gameObject;
            }
        }
    }


                    //############################################################//
                    //#####||Old RoleSwap code and SwapRoles() method below||#####//
                    //############################################################//

    /*
     * void Update()
     * {
            // Finding the currently active worn "Role", and assigning it to the wornRole variable
            for (int i = 0; i < wornRolePos.transform.childCount; i++)
            {
                if (wornRolePos.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    wornRole = wornRolePos.transform.GetChild(i).gameObject;

                    //Debug.Log("Current Role: " + wornRole.tag);
                }
            }

            // Finding and assigning the current worn Role object
            for (int i = 0; i < wornRoleObjPos.transform.childCount; i++)
            {
                if (wornRoleObjPos.transform.GetChild(i).gameObject.tag == "Pickup")
                {
                    wornRoleObj = wornRoleObjPos.transform.GetChild(i).gameObject;

                    //Debug.Log("Current Role's object: " + wornRoleObj);
                }
            }

            // Finding the currently active held "Role", and assigning it to the heldRole variable
            for (int i = 0; i < heldRolePos.transform.childCount; i++)
            {
                if (heldRolePos.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    heldRole = heldRolePos.transform.GetChild(i).gameObject;

                    //Debug.Log("Currently held role: " + heldRole.tag);
                }
            }

            Finding and assigning the current held Role object
            for (int i = 0; i < heldRoleObjPos.transform.childCount; i++)
            {
                if (heldRoleObjPos.transform.GetChild(i).gameObject.tag == "Pickup")
                {
                    heldRoleObj = heldRoleObjPos.transform.GetChild(i).gameObject;

                    //Debug.Log("Currently held Role's object: " + heldRoleObj);
                }
            }
     * }
     * 
     * void SwapRoles()
     * {
            GetWornRole(wornRolePos, heldRole.tag);                         // Finds and assigns the worn Role object matching the held Role's tag
            GetHeldRole(heldRolePos, wornRole.tag);                         // Finds and assigns the held Role object matching the worn Role's tag

            wornRole.SetActive(false);                                      // Deactivates the current worn Role object
            heldRole.SetActive(false);                                      // Deactivates the current held Role object

            wornRoleTarget.SetActive(true);                                 // Activates the target worn Role object
            heldRoleTarget.SetActive(true);                                 // Activates the target held Role object

            wornRoleObj.transform.parent = heldRoleObjPos;
            heldRoleObj.transform.parent = wornRoleObjPos; 
        

            //Debug.Log("Swapping Roles...");
     * }
     * 
     */
}
