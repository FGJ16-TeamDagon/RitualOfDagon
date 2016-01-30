using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        AppManager.Instance.StartLevel();
    }
}
