using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSourceControl : MonoBehaviour {

    AudioSource musicSource;
    public AudioController audioController;
    private AudioSourceControl thissourceControl;

    bool findInstance()
    {
        if (thissourceControl != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            if (findInstance())
            {
                foreach (AudioSource source in FindObjectsOfType<AudioSource>())
                {
                    if (source != this.gameObject.GetComponent<AudioSource>()) 
                    {
                        Destroy(source.gameObject);
                    }
                }
            }
        }
        audioController = FindObjectOfType<AudioController>();
    }

    // Use this for initialization
    void Start () {
        musicSource = this.gameObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        //@ find better check
        if (thissourceControl == null)
        {
            thissourceControl = this.gameObject.GetComponent<AudioSourceControl>();

        }

        audioController = GameObject.Find("GM").GetComponent<AudioController>();
        musicSource = gameObject.GetComponent<AudioSource>();
        musicSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
        musicSource.volume = AudioController.musicVolume;

        CheckMute();
    }

    private void CheckMute()
    {
        if (!AudioController.musicplay)
        {
            musicSource.Pause();
        }
        else
            musicSource.UnPause();
    }

    

    
}
