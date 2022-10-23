using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{

    [SerializeField] GameObject pausePanel;

    public bool isPaused;
    private void Start()
    {
        pausePanel.SetActive(false);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }
    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
