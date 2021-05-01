using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LadderClimb : MonoBehaviour
{
    private float ladderCentrePos;
    private Player2DController moveController;

    private void Start()
    {
        ladderCentrePos = this.transform.position.x;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!(other.tag == "Player"))
            return;

        moveController = other.GetComponent<Player2DController>();

        // Triggers the Climbing state when the Player is in the ladder's trigger area and presses Up or Down, centers the Player on the ladder
        if (other.tag == "Player" && Input.GetButtonDown("Up") || Input.GetButtonDown("Down"))
        {
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;                      // Disables gravity for the player while on the ladder

            Vector3 ladderCentre = new Vector3(ladderCentrePos, other.transform.position.y, other.transform.position.z);
            other.transform.position = ladderCentre;

            moveController.isClimbing = true;
        }

        // If the player is in the Climbing state, make the appropriate animation triggers and allow vertical axis input to climb up and down
        if (moveController.isClimbing)
        {
            var rb = other.GetComponent<Rigidbody>();
            var anim = other.GetComponent<Animator>();
            moveController.StopWalking("ClimbIdle");
            moveController.StopFalling();

            float y = Input.GetAxis("Vertical");
            other.transform.Translate(new Vector3(0, y * moveController.climbSpeed, 0));

            if(y == 0)
            {
                moveController.StopWalking("ClimbIdle");
            }

            // Press Jump to disengage from the ladder
            if (other.tag == "Player" && Input.GetButtonDown("Jump"))
            {
                other.gameObject.GetComponent<Rigidbody>().useGravity = true;
                moveController.isClimbing = false;
            }
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        // De-triggers the Climb state and turns gravity back on for the Player
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().useGravity = true;

            moveController.isClimbing = false;
        }
            
    }
}
