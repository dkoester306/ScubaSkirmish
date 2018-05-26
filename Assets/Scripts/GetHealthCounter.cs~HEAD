using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHealthCounter : MonoBehaviour {

	int currentHealth;
	public List<GameObject> healthIcons;
	
	
	void Awake()
	{
		currentHealth = GameObject.FindGameObjectWithTag("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth;
	}
	// Update is called once per frame
	void Update () {
		currentHealth = GameObject.FindGameObjectWithTag("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth;
		UpdateHealth();
	}

	void UpdateHealth()
	{
		switch(currentHealth)
		{
			case 0:
				healthIcons[0].GetComponent<RawImage>().enabled=false;
				break;
			case 1:
				healthIcons[1].GetComponent<RawImage>().enabled=false;
				break;
			case 2:
				healthIcons[2].GetComponent<RawImage>().enabled=false;
				break;
			default:
				break;
		}
	}

}
