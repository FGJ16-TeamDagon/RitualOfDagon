﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject creditsCanvas;
    public GameObject instructionsCanvas;

    public void StartGame()
    {
        AppManager.Instance.StartLevel();
    }

    public void OpenInstructions()
    {
        instructionsCanvas.SetActive(true);
    }

    public void OpenCredits()
    {
        creditsCanvas.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsCanvas.SetActive(false);
    }

    public void MainMenu()
    {
        AppManager.Instance.GoToStartMenu();
    }

    public void StrandedWin()
    {
        SceneManager.LoadScene("StrandedWin");
    }

    public void DeepOneWin()
    {
        SceneManager.LoadScene("DeepOneWin");
    }
}
