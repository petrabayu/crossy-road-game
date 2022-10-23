using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] GameObject controPanel;
    [SerializeField] GameObject mainMenuPanel;
    private void Start()
    {
     mainMenuPanel.SetActive(true);
        controPanel.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Exit()
    {
        Debug.Log("Keluar dari Permainan");
        Application.Quit();
    }


    public void ControlMenu()
    {
        mainMenuPanel.SetActive(false);
        controPanel.SetActive(true);
    }
    public void ExitControlMenu()
    {
        mainMenuPanel.SetActive(true);
        controPanel.SetActive(false);
    }
}
