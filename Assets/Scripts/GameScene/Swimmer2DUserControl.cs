using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (SwimmerCharacter2D))]
public class Swimmer2DUserControl : MonoBehaviour {

	private SwimmerCharacter2D m_Character;

    private SwimmerAirGravity airGravity;
    private Animator swimmerAnimator;
    private bool attack = false;
    private bool damaged = false;

    public bool Attack
    {
        get { return attack; }
        // tenative
        set { attack = value; }
    }

    void Awake () {
		m_Character = GetComponent<SwimmerCharacter2D> ();
        airGravity = GetComponent<SwimmerAirGravity>();
        swimmerAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKey)
	    {
	        if (Input.GetKeyDown(KeyCode.Space) && !attack && !damaged)
	        {
	            StartCoroutine(ISwimmerPunchInputPressed());
	        }
	        if (Input.GetKeyUp(KeyCode.Space) && attack && !damaged)
	        {
	            StartCoroutine(ISwimmerPunchInputUnPressed());
	        }
	    }


    }

    void FixedUpdate()
	{
	    float h = CrossPlatformInputManager.GetAxis("Horizontal");
	    float v = CrossPlatformInputManager.GetAxis("Vertical");
	    bool inAir = airGravity.checkInAIr;

	    if (!inAir)
	    {
	        m_Character.Move(h, v);
	    }


    }

    // Adding User Control of Attacking the Shark
    // This will be input: Enter
    // This will be activated by a Shark gameobject inside of SpawnAttackState
    // bool SharkAttackInput

    IEnumerator ISwimmerDamaged()
    {
        damaged = true;
        swimmerAnimator.SetBool("damaged", damaged);
        yield return new WaitForSeconds(.3f);
        damaged = false;
        swimmerAnimator.SetBool("damaged", damaged);
    }

    public void SwimmerDamaged()
    {
        StartCoroutine(ISwimmerDamaged());
    }

    IEnumerator ISwimmerPunchInputPressed()
    {
        
        attack = true;
        swimmerAnimator.SetBool("attack",attack);
        GameAudio.PlaySwimmerPunched();
        yield return new WaitForSeconds(.7f);
        SwimmerPunchInputUnPressed();
    }

    IEnumerator ISwimmerPunchInputUnPressed()
    {

        yield return new WaitForSeconds(.3f);
        SwimmerPunchInputUnPressed();
    }

    void SwimmerPunchInputUnPressed()
    {
        attack = false;
        swimmerAnimator.SetBool("attack", attack);
    }
}
