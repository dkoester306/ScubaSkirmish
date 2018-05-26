using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSoundBehavior : MonoBehaviour {

	AudioSource damageSound;

	void Awake()
	{
		damageSound = GetComponent<AudioSource>();
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag=="Anchor"||other.tag=="Mine")
		{
			damageSound.Play();
		}
	}
}
