using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    #region Variables
    private GameObject thisObj;                                             // This GameObject (i.e. the pickup)
    public Transform heldObjPos;                                            // The "Held Object" position on the Player
    public Transform heldRolePos;                                           // The "Held Role" position (a child obj of HeldObjPos) on the Player
    public Transform wornObjPos;                                            // The "worn object" position on the Player
    public bool isRole;                                                     // Is the pickup in this prefab a Role or not?
    public bool isStartingRole;                                             // Is this the Role pickup that the player starts the level wearing?

    public string searchTag;                                                // The Role tag to be found in the Player's heldRolePos, to determine targetRole
    private GameObject targetRole;                                          // The pre-made Role GameObject that will active in the Player's heldRolePos when this obj is picked up
    private SpriteRenderer thisSprite;
    private Color defaultColor;
    private AudioSource audioSrc;


    //private bool isPickedUp;                                              // Has this obj been picked up?
    private Transform pickupParent;                                         // The Pickup prefab, to hold the pickup's original position in the scene
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Grabbing all the dependencies...
        thisObj = gameObject;
        pickupParent = transform.parent;
        thisSprite = GetComponent<SpriteRenderer>();
        defaultColor = thisSprite.color;
        audioSrc = GetComponent<AudioSource>();
        

        if (searchTag != null)                                              // If there has been a Role searchTag defined, trigger the appropriate 
        {                                                                   // method(s) that will find the correspondingly tagged GameObject
            if (isRole)
            {
                FindObjectwithTag(searchTag);
            }
            else if (!isRole)
            {
                this.tag = searchTag;
            }
        }
    }

    public void FindObjectwithTag(string _tag)                              // A method to simplify the GetChildRoles() parameters
    {
        GetChildRoles(heldRolePos, _tag);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Drop") && 
            thisObj.transform.parent == heldObjPos && 
            heldObjPos.gameObject.activeSelf == true)                       // If the the 'Drop' button is pressed, this obj is the child obj of the Held Object position, 
                                                                            // and the Held Object position on the player is active...
        {
            audioSrc.Play(); // ..play the sound effect...

            if (isRole)                                                     // ...and if the held object is a Role...
            {
                DroppedRole();                                              // ...use the Dropped Role parameters.
            }
            else if (!isRole)                                               // Otherwise...
            {
                Dropped();                                                  // ...use the normal Dropped parameters.
            }
        }

        // A method that allows the defining of a Starting Role for each level, so that the player doesn't start nekked when she isn't supposed to.
        if (isStartingRole)
        {
            PickedUpRole();

            isStartingRole = false;
        }
    }

    // The OnTrigger event that manages picking up a pickup-able object:
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && 
            heldObjPos.gameObject.activeSelf == false)                      // If the player is in the trigger area and isn't already holding something...
        {
            thisSprite.color = Color.red;                                   // ..change the sprite's colour to Red.

            if (Input.GetButtonDown("Activate"))                            // If the Activate button is pressed...
            {
                audioSrc.Play();

                if (isRole)                                                 // ...and if this object is a role...
                {
                    PickedUpRole();                                         // ...use the Picked Up Role parameters.
                }
                else if(!isRole)                                            // Otherwise...
                {
                    PickedUp();                                             // ...use the normal Picked Up parameters.
                }
            }
        }
    }

    // Deactivates the floating Pickup prompt text.
    private void OnTriggerExit(Collider other)
    {
        thisSprite.color = defaultColor;
    }

    // The method for picking up non-Role pickups:
    void PickedUp()
    {
        heldObjPos.gameObject.SetActive(true);                              // Activate the heldObj GameObject on the player

        thisObj.transform.parent = heldObjPos;                              // Make the held object position GameObject the parent of the Pickup
        thisObj.transform.localPosition = Vector3.zero;                     // Set the Pickup's position to the heldObj position on the Player
    }

    // The method for picking up Role pickups:
    void PickedUpRole()
    {
        if(wornObjPos.childCount == 0)
        {
            thisSprite.color = defaultColor;

            thisObj.transform.parent = wornObjPos;                              // Make the worn object position GameObject the parent of the Role
            thisObj.transform.localPosition = Vector3.zero;                     // Set the Role's position to the worn object position on the Player
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;     // Disable the sprite renderer for the Role (the corresponding Role on the Player will take its place)
            this.gameObject.GetComponent<Collider>().enabled = false;           // Disable the collider for the Role
        }
        else
        {
            heldObjPos.gameObject.SetActive(true);                              // Activate the held object position GameObject on the player (to prevent picking up more than one thing at a time)
            heldRolePos.gameObject.SetActive(true);                             // Activate the held Role position GameObject on the player (to make visible the Role)
                                                                                //targetRole.SetActive(true);

            thisSprite.color = defaultColor;

            thisObj.transform.parent = heldObjPos;                              // Make the held object position GameObject the parent of the Role
            thisObj.transform.localPosition = Vector3.zero;                     // Set the Role's position to the held object position on the Player
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;     // Disable the sprite renderer for the Role (the corresponding Role on the Player will take its place)
            this.gameObject.GetComponent<Collider>().enabled = false;           // Disable the collider for the Role
        }        
    }

    // The method for dropping non-Role pickups:
    void Dropped()
    {
        thisObj.transform.parent = pickupParent;                                // Set the Pickup's parent to its original Pickup prefab GameObject
        thisObj.transform.localPosition = Vector3.zero;                         // Set the Pickups's position to its original position in the prefab.

        heldObjPos.gameObject.SetActive(false);                                 // Deactivate the heldObj GameObject on the player
    }

    // The method for dropping Role pickups:
    void DroppedRole()
    {
        thisObj.transform.parent = pickupParent;                                // Set the Role's parent to its original Pickup prefab GameObject
        thisObj.transform.localPosition = Vector3.zero;                         // Set the Role's position to its original position in the prefab
        thisObj.transform.localRotation = Quaternion.identity;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;          // Re-enable the Role's Sprite Renderer
        this.gameObject.GetComponent<Collider>().enabled = true;                // Re-enable the Role's collider

        targetRole.SetActive(false);                                            // Deactivate the corresponding Role object in the held role position on the player
        heldObjPos.gameObject.SetActive(false);                                 // Deactivate the held object position GameObject on the player
        heldRolePos.gameObject.SetActive(false);                                // Deactivate the held role position GameObject on the Player
    }

    // The method for using key objects:
    public void Used()
    {
        thisObj.transform.parent = pickupParent;                                // Set the Pickup's parent to its original Pickup prefab GameObject
        thisObj.transform.localPosition = Vector3.zero;                         // Set the Pickups's position to its original position in the prefab.
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;         // Disable the sprite renderer for the Pickup (the corresponding Role on the Player will take its place)
        this.gameObject.GetComponent<Collider>().enabled = false;               // Disable the collider for the Pickup

        heldObjPos.gameObject.SetActive(false);                                 // Deactivate the heldObj GameObject on the player
    }

    // A method that searches through all child objects of heldRole to find the one corresponding with the defined tag, 
    // and assigns that child object to the targetRole variable.
    // Credit to TBruce on UnityAnswers for inspring this method
    public void GetChildRoles(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                targetRole = child.gameObject;
            }
        }
    }

    
        //                                  ############################################################
        //                                  ###||Old PickedUpRole() and DroppedRole() methods below||###
        //                                  ############################################################
                   
        //void PickedUpRole()
        //{
        //    heldObjPos.gameObject.SetActive(true);                                  // Activate the heldObj GameObject on the player
        //    targetRole.SetActive(true);                                             // Activate the corresponding Role object in the Held Role position on the player
     
        //    pickupText.SetActive(false);                                            // Deactivate the floating Pickup prompt text
        //    this.gameObject.GetComponent<SpriteRenderer>().enabled = false;         // Disable the Sprite Renderer for this obj
        //    this.gameObject.GetComponent<Collider>().enabled = false;               // Disable the collider for this obj
        //}
     
     
        //void DroppedRole()
        //{
        //    targetRole.SetActive(false);                                        // Deactivate the corresponding Role object in the Held Role position on the player
        //    heldObjPos.gameObject.SetActive(false);                             // Deactivate the heldObj GameObject on the player
     
        //    this.gameObject.GetComponent<SpriteRenderer>().enabled = true;      // Re-enable this object's Sprite Renderer
        //    this.gameObject.GetComponent<Collider>().enabled = true;            // Re-enable this object's collider
        //}
    
}
