using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Swimmer")
		{
			GameObject.FindGameObjectWithTag("Swimmer").GetComponent<SwimmerCharacter2D> ().PlayerHealth--;
			GameObject.Destroy(gameObject);
		}
			
	}

	void Update()
	{
		if (this.transform.position.y < -6f)
			GameObject.Destroy(gameObject);
	}
		
}
