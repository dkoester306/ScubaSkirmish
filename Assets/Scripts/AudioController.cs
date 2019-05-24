using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {

    public Slider musicBar;
    public Image muteButtomImage;
    public bool musicplay;
    public float musicVolume;
    float lastVolume = 0;
    Color32 orange = new Color32(255, 116, 0, 255);

    // new load, find musicbar

    // Use this for initialization
    void Start () {
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
            muteButtomImage.color = orange;
            musicplay = !musicplay;
        }
        else
        {
            muteButtomImage.color = Color.white;
            musicplay = !musicplay;
        }
    }

    private void SliderEqualsVolume()
    {
        
        if (musicBar != null)
        {
            lastVolume = musicBar.value;
            musicVolume = lastVolume;
        }
        else if(musicBar == null)
        {
            musicVolume = lastVolume;
        }
        
    }
}
