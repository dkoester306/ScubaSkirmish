using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsInput : MonoBehaviour {

    public Image[] buttons;
    public List<KeyCode> keys = new List<KeyCode>();

	// Use this for initialization
	void Awake () {
        buttons = GetComponentsInChildren<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        Color32 nonwhite = new Color32(255, 255, 255, 75);
        Color32 highlightwhite = new Color32(255, 255, 255, 180);
        if (Input.GetKey(KeyCode.W))
        {
            buttons[0].color = highlightwhite;
        }else if (Input.GetKey(KeyCode.A))
        {
            buttons[1].color = highlightwhite;
        }else if (Input.GetKey(KeyCode.S))
        {
            buttons[2].color = highlightwhite;
        }else if (Input.GetKey(KeyCode.D))
        {
            buttons[3].color = highlightwhite;
        }
        else
        {
            foreach (var button in buttons)
            {
                button.color = nonwhite;
            }
        }
	}
}
