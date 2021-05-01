using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Prints the name of the object camera is directly looking at.  Thanks to Brian Loranger for helping me find this script!
public class LookAt : MonoBehaviour
{
    public GameObject aimScript;            // The GameObject that has the AimBehaviour script, to check if the player is currently aiming
    public GameObject infoPopUp;            // The info pop-up that appears when the player examines an examine-able object

    private Camera cam;                     // The main game camera

    void Start()
    {
        cam = GetComponent<Camera>();
    }
    
    // The below code (with the exception of some changed parameters) is from Unity's Scripting API website
    void Update()
    {
         // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if (aimScript.GetComponent<AimBehaviourBasic>().aim == true)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                infoPopUp.SetActive(true);

                hit.transform.GetComponent<InfoText>().IfLookedAt();

                print("I'm looking at " + hit.transform.name);
            } else {
                infoPopUp.SetActive(false);
                print("I'm looking at nothing!");
            }
        } else if (aimScript.GetComponent<AimBehaviourBasic>().aim == false)
        {
            infoPopUp.SetActive(false);
        }
        
    }
}