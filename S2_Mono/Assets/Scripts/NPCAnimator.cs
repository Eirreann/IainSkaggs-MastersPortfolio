using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple script that helps to keep the NPC Animator manageable, by allowing a simple check-box trigger to enable any idle/simple interactive NPC animation needed.
public class NPCAnimator : MonoBehaviour
{
    public bool isLeaning;
    public bool isStanding;
    public bool isAngry;
    public bool isWalking;
    public bool isSitting;
    public bool isYelling;

    private Animator npcAnim;
    
    void Start()
    {
        npcAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isLeaning)
        {
            npcAnim.SetBool("isLeaning", true);
        }

        if (isStanding)
        {
            npcAnim.SetBool("isStanding", true);
        }

        if (isAngry)
        {
            npcAnim.SetBool("isAngry", true);           
        }

        if (isWalking)
        {
            npcAnim.SetLayerWeight(1, 1);
            npcAnim.SetBool("isWalking", true);
            npcAnim.SetTrigger("WalkL");
            npcAnim.SetTrigger("WalkR");
        }

        if (isSitting)
        {
            npcAnim.SetBool("isSitting", true);
        }

        if (isYelling)
        {
            npcAnim.SetBool("isYelling", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(isLeaning)
            {
                npcAnim.SetTrigger("LeanLookUp");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if (isLeaning)
            {
                npcAnim.SetTrigger("LeanLookDown");
            }
        }
        
    }
}
