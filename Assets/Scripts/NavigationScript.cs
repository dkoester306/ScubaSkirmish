using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationScript : MonoBehaviour {

    public Canvas NavigationCanvas;
    public Canvas OptionsCanvas;
    public Canvas HelpCanvas;

    private void Awake()
    {
        NavigationCanvas.enabled = true;
    }

    public void StartPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void StartOptions()
    {
        NavigationCanvas.enabled = false;
        OptionsCanvas.enabled = true;
        HelpCanvas.enabled = false;
    }

    public void StartHelp()
    {
        NavigationCanvas.enabled = false;
        OptionsCanvas.enabled = false;
        HelpCanvas.enabled = true;
    }

    public void StartBack()
    {
        NavigationCanvas.enabled = true;
        OptionsCanvas.enabled = false;
        HelpCanvas.enabled = false;
    }
}
