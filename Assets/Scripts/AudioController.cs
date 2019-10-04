using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour {

    public Slider musicBar;
    public Image muteButtomImage;
    public static bool musicplay = true;
    public static float musicVolume = .02f;
    float lastVolume = 0;
    Color32 orange = new Color32(255, 116, 0, 255);

    // new load, find musicbar
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            foreach (Image img in FindObjectsOfType<Image>())
            {
                if (img.gameObject.name == "MuteButton")
                {
                    muteButtomImage = img;
                }
            }
            musicBar = FindObjectOfType<Slider>();
            musicBar.value = musicVolume;
            SetMute();
            muteButtomImage.gameObject.GetComponent<Button>().onClick.AddListener(CheckMute);
        }
    }

    // Use this for initialization
    void Start () {
        musicVolume = musicBar.value;
	}
	
	// Update is called once per frame
	void Update () {
        SliderEqualsVolume();
	}

    public void SetMute()
    {
        if (musicplay == false)
        {
            muteButtomImage.color = orange;
        }
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
