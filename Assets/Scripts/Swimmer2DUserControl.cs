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
    private Animation swimmerAnimaton;
    private bool attack = false;

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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(SwimmerPunchInputPressed());
        }
    }

	void FixedUpdate()
	{
		float h = CrossPlatformInputManager.GetAxis ("Horizontal");
		float v = CrossPlatformInputManager.GetAxis ("Vertical");

        bool inAir = airGravity.checkInAIr;

        if (inAir == false)
        {
            m_Character.Move(h, v);
        }
	}

    // Adding User Control of Attacking the Shark
    // This will be input: Enter
    // This will be activated by a Shark gameobject inside of SpawnAttackState
    // bool SharkAttackInput

    IEnumerator SwimmerPunchInputPressed()
    {
        
        attack = true;
        swimmerAnimator.SetBool("attack",attack);
        Debug.Log("Attack");
        yield return new WaitForSeconds(.2f);
        
        SwimmerPunchInputUnPressed();
    }

    void SwimmerPunchInputUnPressed()
    {
        attack = false;
        swimmerAnimator.SetBool("attack", attack);
        Debug.Log("Swim");
    }
}
