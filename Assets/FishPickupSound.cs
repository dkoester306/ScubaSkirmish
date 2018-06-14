using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class FishPickupSound : MonoBehaviour {
	AudioSource itemPickup;
	
	void Awake()
	{
		AudioSource[] audios = gameObject.GetComponents<AudioSource>();
		itemPickup = audios[1];
	}
	
void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag=="Fish")
		{
			itemPickup.Play();
		}
	}
}
