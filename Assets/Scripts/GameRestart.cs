using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameRestart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKeyDown)
	    {
	        CheckRestart();
	    }
    }

    private void CheckRestart()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void ButtonRestart()
    {
       SceneManager.LoadScene(0);
    }
}
