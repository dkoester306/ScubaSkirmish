using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        GameObject.FindGameObjectWithTag("GameTitle").GetComponent<Image>().enabled=false;
        NavigationCanvas.enabled = false;
        OptionsCanvas.enabled = true;
        HelpCanvas.enabled = false;
    }

    public void StartHelp()
    {
        GameObject.FindGameObjectWithTag("GameTitle").GetComponent<Image>().enabled=false;
        NavigationCanvas.enabled = false;
        OptionsCanvas.enabled = false;
        HelpCanvas.enabled = true;
    }

    public void StartBack()
    {
        GameObject.FindGameObjectWithTag("GameTitle").GetComponent<Image>().enabled=true;
        NavigationCanvas.enabled = true;
        OptionsCanvas.enabled = false;
        HelpCanvas.enabled = false;
    }
}
