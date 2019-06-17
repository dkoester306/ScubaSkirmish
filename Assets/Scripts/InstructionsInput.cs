using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsInput : MonoBehaviour {

    Color32 nonwhite = new Color32(255, 255, 255, 75);
    Color32 highlightwhite = new Color32(255, 255, 255, 180);

    public Image[] buttons;
    public Image enterbutton;
    public List<KeyCode> keys = new List<KeyCode>();

	// Use this for initialization
	void Awake () {
        buttons = GetComponentsInChildren<Image>();
	}
	
	// Update is called once per frame
	void Update () {


	    if (Input.anyKey)
	    {
	        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
	        {
	            buttons[0].color = highlightwhite;
	        }
	        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
	        {
	            buttons[1].color = highlightwhite;
	        }
	        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
	        {
	            buttons[2].color = highlightwhite;
	        }
	        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
	        {
	            buttons[3].color = highlightwhite;
	        }
	        else if ((Input.GetKey(KeyCode.Space)))
	        {
	            enterbutton.color = highlightwhite;
	        }
        }
	    else
	    {
	        foreach (var button in buttons)
	        {
	            button.color = nonwhite;
	            enterbutton.color = nonwhite;
	        }
	    }
    }
}
