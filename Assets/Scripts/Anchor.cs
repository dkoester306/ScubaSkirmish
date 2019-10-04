﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			GameObject.Find ("Swimmer").GetComponent<SwimmerCharacter2D> ().PlayerHealth--;
            //DestroyObject (this.gameObject);
            GameAudio.PlayAnchorSound();
		    gameObject.SetActive(false);
        }
			
	}

	void Update()
	{
		if (this.transform.position.y < -6f)
            //DestroyObject (gameObject);
		    gameObject.SetActive(false);

        //! Object Pooling
    }

}
