﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SwimmerCharacter2D : MonoBehaviour {
	[SerializeField] private float m_MaxSpeed = 6f;				// The fastest the player can travel in any directions
	[SerializeField] private float m_MinX=-8.21f;						// The smallest x value the player can be in
	[SerializeField] private float m_MaxX = 8.4f;						// The smallest x value the player can be in
	[SerializeField] private float m_MinY=-4.88f;						// The smallest x value the player can be in
	[SerializeField] private float m_MaxY=4.85f;						// The smallest x value the player can be in

	private Rigidbody2D m_RigidBody;
	private bool m_FacingRight = true;
	private Vector3 m_Position;

	private int fishCount;
	private int m_PlayerHealth;
    private bool attack;

	public int FishCount{get{return fishCount;}}
	public int PlayerHealth{get{return m_PlayerHealth;}set{m_PlayerHealth = value;}}

    public Vector3 PlayerPosition
    {
        get { return m_Position; }
    }

    void Awake()
	{
		// setting up the references
		m_RigidBody =  GetComponent<Rigidbody2D>(); 

	}

	// Use this for initialization
	void Start () {
		m_Position = this.transform.position;
		fishCount = 0;
		m_PlayerHealth = 10;
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
		theScale.x *= -1;
		transform.localScale = theScale;


	}
	// Update is called once per frame
	void Update () {
		UpdatePosition ();
        BoundaryCollision ();
        if (m_PlayerHealth <= 0)
            CheckZeroHealth();
    }


	void BoundaryCollision()
	{
		Vector3 newPos = m_Position;
		newPos.x = Mathf.Clamp (m_Position.x, m_MinX, m_MaxX);
		newPos.y = Mathf.Clamp (m_Position.y, m_MinY, m_MaxY);

        if (newPos.x <= m_MinX || newPos.x >= m_MaxX)
        {
		    this.transform.position = newPos;
        }

        if (newPos.y <= m_MinY || newPos.y >= m_MaxY)
        {
            this.transform.position = newPos;
        }

    }

	void UpdatePosition()
	{
		m_Position = this.transform.position;
	}

	public void IncreaseFishCount()
	{
		fishCount++;
	}

	void CheckZeroHealth()
	{
		if(m_PlayerHealth<=0)
		{
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(2);  // Game Over Scene
        }
	}
		
		
}
