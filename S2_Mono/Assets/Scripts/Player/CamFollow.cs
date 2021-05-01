using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform camTarget;
    public Player2DController playerScript;

    public float speed = 1.0f;

    private void Start()
    {
        this.transform.position = camTarget.position;

        playerScript.Flip();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(this.transform.position, camTarget.position, speed * Time.deltaTime);
    }
}
