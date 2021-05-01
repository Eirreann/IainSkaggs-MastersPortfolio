using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A modified RoleScanner script (see RoleScanner.cs) for the defunct Role Scanner in 01SewerLevel
public class DefunctRoleScanner : MonoBehaviour
{
    public GameObject gateComponent;
    public GameObject scannerObj;
    public GameObject indicator;

    private Light scannerLight;
    private SpriteRenderer indicatorScreen;
    private Color startingColor;
    private Color startingIndicator;

    private AudioSource audioSrc;

    private void Start()
    {
        scannerLight = scannerObj.GetComponent<Light>();
        startingColor = scannerLight.color;

        indicatorScreen = indicator.GetComponent<SpriteRenderer>();
        startingIndicator = indicatorScreen.color;

        audioSrc = GetComponent<AudioSource>();

        LockedGate();
    }

    private void OnTriggerStay(Collider other)
    {
        audioSrc.Play();

        Invoke("OpenedGate", Random.Range(0.5f, 3f));
        Invoke("LockedGate", Random.Range(0.5f, 3f));
        
    }

    private void OnTriggerExit(Collider other)
    {
        audioSrc.Stop();
        Invoke("LockedGate", 0.1f);
    }

    private void LockedGate()
    {
        scannerLight.color = Color.red;
        indicatorScreen.color = Color.red;
    }

    private void OpenedGate()
    {
        scannerLight.color = Color.green;
        indicatorScreen.color = Color.green;
    }

    private void GateNeutral()
    {
        scannerLight.color = startingColor;
        indicatorScreen.color = startingIndicator;
    }
}
