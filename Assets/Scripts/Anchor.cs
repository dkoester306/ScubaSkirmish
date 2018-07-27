using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour {
	
	GameObject localAnchorWarning;

	void Start()
	{
		localAnchorWarning = null;
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Swimmer")
		{
			GameObject.FindGameObjectWithTag("Swimmer").GetComponent<SwimmerCharacter2D> ().PlayerHealth--;
			SpawnTrash.RemoveAnchor(gameObject);
		}
			
	}

	void Update()
	{
		// pop anchor from stack
		// make it visible
		// make warning spawn in the same x coordinate as the anchor
		// make it last for a 'X' amount of time
		// the warning flicker somehow??? NOT PRIORITY
		// end the animation
		// push object back on stack and make invisible


		if (this.transform.position.y < -6f)
			SpawnTrash.RemoveAnchor(gameObject);
	}
		
}
