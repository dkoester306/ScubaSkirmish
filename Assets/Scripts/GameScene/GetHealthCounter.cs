﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHealthCounter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Text> ().text = GameObject.Find ("Swimmer").GetComponent<SwimmerCharacter2D> ().PlayerHealth+"";
	}
}
