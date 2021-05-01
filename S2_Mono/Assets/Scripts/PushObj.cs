using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObj : MonoBehaviour
{

    private Player2DController controller;
    private Transform player;
    private Animator playerAnim;
    private bool canPush = false;

    private void Update()
    {
        // Activate pushing/pulling when the Player presses the Activate button, trusting the player's in the trigger area
        if (Input.GetButtonDown("Activate") && player != null)
            {
                Push();
            }
    }

    // Set up the appropriate assignments when the Player is in the trigger area
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            controller = other.GetComponent<Player2DController>();
            player = other.gameObject.transform;
            playerAnim = player.GetComponent<Animator>();
        }
    }

    // Null out the above assignments if the Player leaves the Trigger area *without* starting the push/pull
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" && transform.parent != player)
        {
            NullOut();
        }
    }

    // Switches between pushing and not-pushing based on animation state and informed by the Player2DController script
    void Push()
    {
        if (!controller.isPushing && playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Circle_idle"))
        {
            transform.parent = player;
            controller.isPushing = true;
        }
        else if (controller.isPushing && playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Push_idle"))
        {
            transform.parent = null;
            controller.isPushing = false;      
        }
    }

    // A method to null-out the varaibles assigned when the player entered trigger area.
    void NullOut()
    {
        player = null;
        controller = null;
        playerAnim = null;
    }
}
