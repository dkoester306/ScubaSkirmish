using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioSourceControl : MonoBehaviour {

    AudioSource musicSource;
    public AudioController audioController;
    private AudioSourceControl thisSourceControl;
    public Button muteButton;

    public AudioClip mainMenuMusic;
    public AudioClip mainGameMusic;

    bool findInstance()
    {
        return thisSourceControl != null;
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

                musicSource.clip = mainMenuMusic;
                musicSource.Play();
            }
        }

        if (scene.buildIndex == 1)
        {
            musicSource.clip = mainGameMusic;
            musicSource.Play();
            musicSource.volume = AudioController.musicVolume;
        }

        if (scene.buildIndex == 2)
        {
            musicSource.clip = mainMenuMusic;
            musicSource.Play();
            musicSource.volume = AudioController.musicVolume;
        }

        audioController = FindObjectOfType<AudioController>();
    }

    // Use this for initialization
    void Start () {
        musicSource = this.gameObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        //! find better check
        if (thisSourceControl == null)
        {
            thisSourceControl = this.gameObject.GetComponent<AudioSourceControl>();

        }

        audioController = GameObject.Find("GameManager").GetComponent<AudioController>();
        musicSource = gameObject.GetComponent<AudioSource>();
        musicSource.Play();
        muteButton = GameObject.Find("MuteButton").GetComponent<Button>();
        muteButton.onClick.AddListener(delegate { SetAudioMute(); });
    }

    // Update is called once per frame
    void Update () {
        musicSource.volume = AudioController.musicVolume;
        muteButton = GameObject.Find("MuteButton") ? GameObject.Find("MuteButton").GetComponent<Button>() : null;

        if (muteButton == null)
        {
            return;
        }

        muteButton.onClick.AddListener(delegate { SetAudioMute(); });
    }

    private void SetAudioMute()
    {
        if (!AudioController.musicplay)
        {
            musicSource.mute = true;
        }
        else
            musicSource.mute = false;
    }

    

    
}
