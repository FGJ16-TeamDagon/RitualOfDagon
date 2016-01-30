using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    private static AppManager instance;
    public static AppManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = Resources.Load<GameObject>("AppManager");
                instance = go.GetComponent<AppManager>();
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void GoToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void StartLevel()
    {
        SceneManager.LoadScene("Level01");
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
