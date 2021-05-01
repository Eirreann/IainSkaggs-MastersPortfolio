using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2DController : MonoBehaviour
{
    #region Variables
    [HideInInspector] public bool facingRight = true;                   // Used by the Flip() method to determine which direction the player is facing at a given time
    [HideInInspector] public bool isJumping = false;                    // Is the player jumping?
    [HideInInspector] public bool isPushing = false;                    // Is the player pushing an object? - Referenced by PushObj.cs
    [HideInInspector] public bool isClimbing = false;                   // Is the player climbing a ladder? - Referenced by LadderClimb.cs
    public float moveForce = 365f;                                      // The force applied to the Player when moving horizontally
    public float maxSpeed = 5f;                                         // The maximum speed that the Player can move at
    public float climbSpeed = 0.5f;                                     // The max speed that the Player can climb ladders
    public float jumpForce = 100f;                                      // The force applied to the Player when jumping

    public AudioClip jumpAudio;                                         // The audio clip that plays when the player jumps.
    public AudioClip landAudio;                                         // The audio clip that plays when the player lands on Ground

    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2f;

    private bool isGrounded = false;                                    // Is the player on the Ground?
    private bool isRunning = false;                                     // Is the player running?
    private Rigidbody rb;                                               // Player's Rigidbody
    private Animator playerAnim;                                        // Player's Animator
    private AudioSource audioSource;                                    // Player's audio source

    private float walkSpeed;                                            // Keep track of Player's walk speed
    private float runSpeed;                                             // Speed applied when the Player is running
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        walkSpeed = maxSpeed;
        runSpeed = maxSpeed * 2;                    // Run speed is double base wall speed

        isRunning = false;                          // Make sure that the Player is not running when Player2DController is enabled after a UI popup has disabled it. - Shame I forgot that things don't work that way
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isPushing)        // If the Player presses jump while they are on the ground and not pushing...
        {
            isJumping = true;                                               // ...jump!

            audioSource.clip = jumpAudio;
            audioSource.Play();                                             // ...and play the Jump audio.
        }

        if (Input.GetButtonDown("Sprint") && isGrounded)                    // If the player presses sprint while on the ground...
        {
            isRunning = true;                                               // ...run!

            maxSpeed = runSpeed;                                            // ...and set the appropriate speed.
        }
        else if (Input.GetButtonUp("Sprint"))                               // If the player releases sprint...
        {
            isRunning = false;                                              // ...stop running!

            maxSpeed = walkSpeed;                                           // ...and set the appropriate speed.
        }

        if (isPushing)                                                      // If the player is Pushing (as assigned by PushObj.cs)...
        {
            playerAnim.SetBool("Push", true);                               // ...activate the push animation!

            isRunning = false;                                              // ...and make sure the player isn't running.
        }
        else if (!isPushing)                                                // Otherwise...
        {
            playerAnim.SetBool("Push", false);                              // ...deactivate the push animation
        }

        // Credit to Board To Bits Games on YouTube for this bit of jump optimisation code.
        if (rb.velocity.y < 0 && !isGrounded)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            playerAnim.SetBool("Land", true);
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y >= 0 || isGrounded)
        {
            playerAnim.SetBool("Land", false);
        }
    }

    private void FixedUpdate()
    {
        // Grab the Horizontal and Vertical input axes
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Apply force to the Player object until the Player reaches max speed if the horizontal axes is being pressed down, as long as the player isn't climbing.
        if (h * rb.velocity.x < maxSpeed && !isClimbing)
        {
            rb.AddForce(Vector3.right * h * moveForce);
        }
        if (Mathf.Abs(rb.velocity.x) > maxSpeed && !isClimbing)
            rb.velocity = new Vector3(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y, 0f);

        //  Checks to make sure the appropriate animations play/don't play based on Player movement on the horizontal axis
        if (h == 0 && isGrounded)
        {
            if (!isJumping && !isClimbing)
            {
                StopWalking("Idle");
            }

            if (!isPushing)
            {
                playerAnim.SetTrigger("Idle");
            }
            else if (isPushing)
            {
                playerAnim.SetTrigger("PushIdle");
            }
            
            // Reset animation triggers
            playerAnim.ResetTrigger("Pushing");
            playerAnim.ResetTrigger("Pulling");
            playerAnim.ResetTrigger("Walk");
            playerAnim.ResetTrigger("Run");
        }
        else if (h > 0 || h < 0 && isGrounded)
        {
            if (!isPushing)
            {

                if (!isRunning)
                {
                    playerAnim.SetTrigger("Walk");
                    playerAnim.ResetTrigger("Run");
                    
                }
                else if (isRunning)
                {
                    playerAnim.ResetTrigger("Walk");
                    playerAnim.ResetTrigger("Idle");
                    playerAnim.SetTrigger("Run");
                }

                playerAnim.ResetTrigger("Climbing");            // Attempted to reset the Climbing animation trigger here, to avoid getting stuck on the climbing animation when jumping after reaching the
                                                                // top of a ladder.  Didn't work, and couldn't find a working solution. :(
            }
            else if (isPushing)
            {
                playerAnim.ResetTrigger("PushIdle");

                if(h > 0 && facingRight || h < 0 && !facingRight)
                {
                    playerAnim.SetTrigger("Pushing");
                }
                else if(h < 0 && facingRight || h > 0 && !facingRight)
                {
                    playerAnim.SetTrigger("Pulling");
                }
            }            
        }

        // Animation triggers based on the vertical axis for climbing ladders (referenced in LadderClimb.cs)
        if (v == 0 && isClimbing)
        {
            playerAnim.SetTrigger("ClimbIdle");
            playerAnim.ResetTrigger("Climbing");
        }
        else if(v > 0 || v < 0 && isClimbing)
        {
            playerAnim.SetTrigger("Climbing");
            //playerAnim.ResetTrigger("ClimbIdle");
            playerAnim.ResetTrigger("Idle");
        }
        else if(!isClimbing)
        {
            playerAnim.ResetTrigger("Climbing");
            playerAnim.ResetTrigger("ClimbIdle");
        }

        //  If the player is moving in a direction, make sure the sprite is flipped to face that direction if it isn't already.
        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        // Jump the player, called after the Jump button is pressed.  Applies force, resets isJumping, and triggers the animation.
        if (isJumping)
        {
            playerAnim.SetTrigger("Jump");
            rb.AddForce(new Vector3(0f, jumpForce, 0f));
            isJumping = false;
        }

        HandleLayers();

    }

    // Collision triggers for the Ground, so that the appropriate animations play and the Land audio clip plays
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            audioSource.clip = landAudio;
            audioSource.Play();

            isGrounded = true;
            playerAnim.ResetTrigger("Jump");
            playerAnim.ResetTrigger("ClimbIdle");
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // A method for flipping the Player to face the direction its moving on the horizontal axis
    public void Flip()
    {
        if (!isPushing) { 
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    // A method for stopping the player from moving, and set an idle trigger called from LadderClimb
    public void StopWalking(string animTrigger)
    {
        Vector3 rbVelX = rb.velocity;
        rbVelX.x = 0f;
        rb.velocity = rbVelX;

        playerAnim.SetTrigger(animTrigger);
    }

    // A method that resets the player's vertical velocity, called from LadderClimb
    public void StopFalling()
    {
        Vector3 rbVelY = rb.velocity;
        rbVelY.y = 0f;
        rb.velocity = rbVelY;
    }

    // A method for handling the transition between Animator layers.
    void HandleLayers()
    {
        if (!isGrounded)
        {
            playerAnim.SetLayerWeight(1, 1);
        }
        else
        {
            playerAnim.SetLayerWeight(1, 0);
        }
    }
}
