using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (SwimmerCharacter2D))]
public class Swimmer2DUserControl : MonoBehaviour {

	private SwimmerCharacter2D m_Character;


	void Awake () {
		m_Character = GetComponent<SwimmerCharacter2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		float h = CrossPlatformInputManager.GetAxis ("Horizontal");
		float v = CrossPlatformInputManager.GetAxis ("Vertical");

		m_Character.Move (h,v);
	}
}
