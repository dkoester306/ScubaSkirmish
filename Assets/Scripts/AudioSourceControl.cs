using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSourceControl : MonoBehaviour {

    AudioSource musicSource;
    public AudioController audioController;
    private AudioSourceControl thissourceControl;
    public UnityEngine.UI.Button muteButton;

    public AudioClip mainMenuMusic;
    public AudioClip mainGameMusic;

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

                musicSource.clip = mainMenuMusic;
                musicSource.Play();
            }
        }

        if (scene.buildIndex == 1)
        {
            musicSource.clip = mainGameMusic;
            musicSource.Play();
        }

        if (scene.buildIndex == 2)
        {
            musicSource.clip = mainMenuMusic;
            musicSource.Play();
        }

        audioController = FindObjectOfType<AudioController>();
    }

    // Use this for initialization
    void Start () {
        musicSource = this.gameObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        //! find better check
        if (thissourceControl == null)
        {
            thissourceControl = this.gameObject.GetComponent<AudioSourceControl>();

        }

        audioController = GameObject.Find("GM").GetComponent<AudioController>();
        musicSource = gameObject.GetComponent<AudioSource>();
        musicSource.Play();
        muteButton = GameObject.Find("MuteButton").GetComponent<UnityEngine.UI.Button>();
        muteButton.onClick.AddListener(delegate { SetAudioMute(); });
    }

    // Update is called once per frame
    void Update () {
        musicSource.volume = AudioController.musicVolume;
        muteButton = GameObject.Find("MuteButton") ? GameObject.Find("MuteButton").GetComponent<UnityEngine.UI.Button>() : null;

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
