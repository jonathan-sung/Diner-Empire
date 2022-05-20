using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Canvas pauseCanvas;
    public static bool paused;
    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(true);
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
        Time.timeScale = paused ? 0 : 1;
        pauseCanvas.enabled = paused ? true : false;
    }

    public void TogglePause()
    {
        paused = paused ? false : true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
