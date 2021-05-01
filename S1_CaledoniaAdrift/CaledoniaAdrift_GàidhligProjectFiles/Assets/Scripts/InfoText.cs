using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour {

    public GameObject title;        // Title of the item that the player is examining
    public string titleCont;        // A string that changes content of the above title
    public GameObject body;         // The body text of an item that the player is examining
    [TextArea(10, 15)]  // (for easy Inspector editing)
    public string bodyCont;         // A string that changes the content of the above body

    private Text titleText;         // The Text element of the title GameObject
    private Text bodyText;          // The Text element of the body GameObject


    void Start () {                 // Assigning the text elements to their variables
        titleText = title.GetComponent<Text>();

        bodyText = body.GetComponent<Text>();
    }
	
	public void IfLookedAt () {     // Display the appropriate title and body text when an object is examined by the player
        titleText.text = titleCont;

        bodyText.text = bodyCont;
    }
}
