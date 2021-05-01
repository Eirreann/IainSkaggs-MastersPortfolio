using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObj : MonoBehaviour
{
    [Range(0.5f, 50f)]
    public float moveSpd = 5;

    [HideInInspector]
    public Vector2 move;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Grabbing the directional axes (e.g. the arrow keys) and declaring a Vector2 movement vector using the axes as inputs.
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        move = new Vector2(h, v);

        //transform.position += move * moveSpd * Time.deltaTime;  // Old movement method; we're using the rigidbody now (see below)

        // Resetting movement along the x / y axis to zero if movement is already happening along the other axis, so you can't move in both directions at once.
        if (h > 0 || h < 0)
        {
            //move.y = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        else if (v > 0 || v < 0)
        {
            //move.x = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }

        // Applying movement to the Obj using the rigidbody in order for collisions with the walls of the maze to register properly.
        rb.MovePosition(new Vector2((transform.position.x + move.x * moveSpd * Time.deltaTime), (transform.position.y + move.y * moveSpd * Time.deltaTime)));

        // Clamping movement to the edges of the maze, so that the Obj can't go out-of-bounds (will refine later).
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -6.3f, 6.3f), Mathf.Clamp(transform.position.y, -4.65f, 4.4f), transform.position.z);

        //print(transform.position);
    }
}
