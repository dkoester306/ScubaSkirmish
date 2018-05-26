using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {

    public Slider musicBar;
    public bool musicplay;
    public float musicVolume;

	// Use this for initialization
	void Awake () {
        musicplay = true;
        musicVolume = musicBar.value;
	}
	
	// Update is called once per frame
	void Update () {
        SliderEqualsVolume();
	}

    public void CheckMute()
    {
        if (musicplay)
        {
            musicplay = !musicplay;
        }
        else
        {
            musicplay = !musicplay;
        }
    }

    private void SliderEqualsVolume()
    {
        musicVolume = musicBar.value;
    }
}
