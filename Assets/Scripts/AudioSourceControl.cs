using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSourceControl : MonoBehaviour {

    public AudioSource musicSource;
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
        Debug.Log("Something Happened");
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
    }

    // Use this for initialization
    void Start () {
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
