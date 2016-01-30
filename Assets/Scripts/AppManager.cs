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
                var prefab = Resources.Load<GameObject>("AppManager");
                var go = Instantiate(prefab);
                instance = go.GetComponent<AppManager>();
            }

            return instance;
        }
    }

    void OnEnable()
    {
        Debug.Log("AppManage OnEnable");
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

    void OnDisable()
    {
        Debug.Log("AppManage OnDisable");
    }

    public void GoToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void StartLevel()
    {
        SceneManager.LoadScene("Level01");
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
