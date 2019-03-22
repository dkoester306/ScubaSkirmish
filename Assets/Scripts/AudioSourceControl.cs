using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceControl : MonoBehaviour {

    public AudioSource musicSource;
    public AudioController audioController;

    // Use this for initialization
    void Awake () {
        audioController = GameObject.Find("GM").GetComponent<AudioController>();
        musicSource = gameObject.GetComponent<AudioSource>();
        musicSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
        musicSource.volume = audioController.musicVolume;

        CheckMute();
    }

    private void CheckMute()
    {
        if (!audioController.musicplay)
        {
            musicSource.Pause();
        }
        else
            musicSource.UnPause();
    }

}
