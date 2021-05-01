using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple script that sets the attached GameObject to spin along its Y axis (used for pickups and some stones)
public class PickupSpin : MonoBehaviour {

    public float speed = 10f;       // A speed variable for the Inspector

    // The rotation itself
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);

    }
}
