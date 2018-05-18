using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {
	private Rigidbody2D m_RigidBody;

	[SerializeField] private float m_SpeedX;

	// Use this for initialization
	void Start () {
		m_RigidBody = GetComponent<Rigidbody2D> ();

	}
		

	// Update is called once per frame
	void Update () {
		m_RigidBody.velocity = new Vector2 (m_SpeedX, 0);
		if (this.transform.position.x < -12f)
			GameObject.Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Swimmer")
		{
			GameObject.FindGameObjectWithTag("Swimmer").GetComponent<SwimmerCharacter2D> ().IncreaseFishCount ();
			GameObject.Destroy(gameObject);
		}
			
	}



}
