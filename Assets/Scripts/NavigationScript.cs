using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationScript : MonoBehaviour {

    public Canvas NavigationCanvas;
    public Canvas OptionsCanvas;
    public Canvas HelpCanvas;
    public Canvas AboutCanvas;

    private void Awake()
    {
        if (NavigationCanvas != null) 
        {
            NavigationCanvas.enabled = true;
        }
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
        AboutCanvas.enabled = false;
    }

    public void StartHelp()
    {
        NavigationCanvas.enabled = false;
        OptionsCanvas.enabled = false;
        HelpCanvas.enabled = true;
        AboutCanvas.enabled = false;
    }

    public void StartBack()
    {
        NavigationCanvas.enabled = true;
        OptionsCanvas.enabled = false;
        HelpCanvas.enabled = false;
        AboutCanvas.enabled = false;
    }

    public void StartAbout()
    {
        NavigationCanvas.enabled = false;
        OptionsCanvas.enabled = false;
        HelpCanvas.enabled = false;
        AboutCanvas.enabled = true;
    }

    public void OpenPlaytstForm()
    {
        Application.OpenURL("https://forms.gle/uwRVruCuFCPyNFZr9");
    }

    public void EndButtonRestart()
    {
        SceneManager.LoadScene(0);
    }

    public void EndButtonExit()
    {
        Application.Quit();
    }
}
