using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwimmerCharacter2D : MonoBehaviour {
	[SerializeField] private float m_MaxSpeed = 6f;			// The fastest the player can travel in any direction
	private Rigidbody2D m_RigidBody;
	private bool m_FacingRight = true;

	void Awake()
	{
		// setting up the references
		m_RigidBody =  GetComponent<Rigidbody2D>();
	}

	// Use this for initialization
	void Start () {
		
	}

	/// <summary>
	/// Move this instance.
	/// </summary>
	/// <param name="moveH">Value that moves the object horizontally</param>
	/// <param name="moveV">Value that moves the object veritcally</param>
	public void Move(float moveH, float moveV)
	{
		// move the character
		m_RigidBody.velocity = new Vector2 (moveH * m_MaxSpeed, moveV * m_MaxSpeed);

		// if the player is moving right, then make the player face left
		if(moveH>0&& !m_FacingRight )
		{
			Flip ();
		}
		// otherwise the player if moving right, face them right
		else if(moveH<0&& m_FacingRight)
		{
			Flip ();
		}
	}

	public void Flip()
	{
		// switch which way the player if facing
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;


	}
	// Update is called once per frame
	void Update () {
		
	}
}
