using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple script that sets the attached GameObject to spin along its Z axis (used for stones and some pickups) - virtually identical to PickupSpin but on a different axis
public class StoneSpin : MonoBehaviour {

    public float speed;

    void Update()
    {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}
