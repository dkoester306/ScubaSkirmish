﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checks if swimmer collides with 
public class SwimmerAirGravity : MonoBehaviour {

    // needs for script
    // access to swimmers position vector
    // check if the player is colliding with air box collider
    // will need a method to mathmatically send the swimmer down

    // get acces to swimmers position
    private Rigidbody2D swimmersRigidbody;

    private Vector3 airGravityScale;
    private float y;

    // int to change gravity
    private float gravityScale;

    public bool checkInAIr;

    public float GravityScale
    {
        get { return gravityScale; }
        set { gravityScale = value; }
    }

    private void Start()
    {
        // get rigigbodygravity
        swimmersRigidbody = gameObject.GetComponent<Rigidbody2D>();

        // set gravity scale
        GravityScale = y = 20.0f;

        // set checkInAir
        checkInAIr = false;
    }

    // check if player collides with airgravitycollider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if tag is Air
        if (collision.gameObject.tag == "Air")
        {
            // set swimmers rigidbody to gravity scale
            swimmersRigidbody.gravityScale = GravityScale * Time.deltaTime * 5.0f;

            checkInAIr = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // check if tag is Air
        if (collision.gameObject.tag == "Air")
        {
            // set swimmers rigidbody to gravity scale
            swimmersRigidbody.gravityScale = 0.0f;

            checkInAIr = false;
        }
    }
}