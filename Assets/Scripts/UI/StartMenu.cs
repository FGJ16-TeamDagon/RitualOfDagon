using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    public GameObject creditsCanvas;

    public void StartGame()
    {
        AppManager.Instance.StartLevel();
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
}
