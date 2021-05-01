using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelTransition : MonoBehaviour
{
    public GameObject cam;

    public Collider thisLvl;
    public Collider nextLvl;

    private Collider currentLvl;
    private CinemachineConfiner cineCamSettings;



    // Start is called before the first frame update
    void Start()
    {
        cineCamSettings = cam.GetComponent<CinemachineConfiner>();
    }

    // Transitions the Cinemachine camera bwteen the bounding volumes that represent the current and subsequent levels when the player leaves the collder
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (cineCamSettings.m_BoundingVolume == thisLvl)
            {
                cineCamSettings.m_BoundingVolume = nextLvl;
            }
            else if (cineCamSettings.m_BoundingVolume == nextLvl)
            {
                cineCamSettings.m_BoundingVolume = thisLvl;
            }
        }
    }
}
