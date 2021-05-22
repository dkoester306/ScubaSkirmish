using System.Collections;
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
    private Camera mainCamera;

    private Vector3 airGravityScale;
    private Vector3 OGCamPosition;
    private Vector3 playerPosition;

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
        gravityScale = 30.0f;

        // set checkInAir
        checkInAIr = false;

        // get main camera
        mainCamera = Camera.main;
    }

    // check if player collides with airgravitycollider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if tag is Air
        if (collision.gameObject.tag == "Air")
        {
            // set swimmers rigidbody to gravity scale
            swimmersRigidbody.gravityScale = gravityScale * Time.deltaTime * 5.0f;

            checkInAIr = true;

            // Camera Lerp Movement to Player Medicated
            // 
            // journey distance
            // disOfJoruney = 
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

            GameAudio.PlaySwimmerReEnterWater();

            // Camera Lerp Movement to OGPosition
        }
    }
}
