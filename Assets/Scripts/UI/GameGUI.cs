using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour 
{
    public GameObject optionsPanel;
    public Image patternImage;
    public GameObject strandedCanvas;
    public GameObject deepOneCanvas;
    public GameObject uiCanvas;

    public enum GUIState
    {
        Hidden,
        GameGUI,
        Options,
    }

    [SerializeField]
    private UnityEngine.UI.Text turnsLeftDisplay;

    private static GameGUI instance;
    public static GameGUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameGUI>();
            }

            return instance;
        }
    }

    public GUIState State { get; private set; }

    void Start()
    {
        StartGame(); // TODO: show pattern at start, then call this

        AudioListener.volume = PlayerPrefs.GetFloat("AudioListener.volume", 1);
    }

    public void EndTurn()
    {
        GamePlay.Instance.EndTurn();
        StartTurn(); // TODO: remove this from and call from UI
    }

    public void StartTurn()
    {
        GamePlay.Instance.StartTurn();
    }

    public void RestartLevel()
    {
       AppManager.Instance.RestartLevel();
    }

    public void ReturnToStartMenu()
    {
        AppManager.Instance.GoToStartMenu();
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        State = GUIState.Options;
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void OpenGameGUI()
    {
        // TODO: if hidden/options, transition to game GUI
        State = GUIState.GameGUI;
    }

    void Update()
    {
        if (GamePlay.Instance.State == GamePlay.GameplayState.Playing)
        {
            if (GamePlay.Instance.CurrentPlayer == GamePlay.Instance.StrandedPlayer)
            {
                turnsLeftDisplay.text = "Block the ritual";
            }
            else
            {
                if (GamePlay.Instance.TurnsLeft == 1)
                {
                    turnsLeftDisplay.text = "Last chance to complete ritual!";
                }
                else
                {
                    turnsLeftDisplay.text = GamePlay.Instance.TurnsLeft + " turns to complete ritual";
                }
            }
        }
        else if (GamePlay.Instance.State == GamePlay.GameplayState.GameOver)
        {
            if (GamePlay.Instance.Winner == GamePlay.Instance.StrandedPlayer)
            {
                turnsLeftDisplay.text = "The Ritual failed!";
                
            }
            else
            {
                turnsLeftDisplay.text = "The Ritual succeeds!";
            }
        }
    }

    public void StartGame()
    {
        GamePlay.Instance.StartGame();
        patternImage.sprite = Sprite.Create(GamePlay.Instance.Ritual.image, new Rect(0,0, GamePlay.Instance.Ritual.image.width, GamePlay.Instance.Ritual.image.height),new Vector2(0.5f,0.5f));
    }

    public void StrandedEnd()
    {
        uiCanvas.SetActive(false);
        StartCoroutine(WaitForStrandEnd());
    }

    public void DeepOneEnd()
    {
        uiCanvas.SetActive(false);
        StartCoroutine(WaitForDeepEnd());
    }

    IEnumerator WaitForDeepEnd()
    {
        yield return new WaitForSeconds(3.0f);
        deepOneCanvas.SetActive(true);
    }

    IEnumerator WaitForStrandEnd()
    {
        yield return new WaitForSeconds(3.0f);
        strandedCanvas.SetActive(true);
    }

    public void ToggleSounds()
    {
        AudioListener.volume = AudioListener.volume > 0 ? 0 : 1;

        PlayerPrefs.SetFloat("AudioListener.volume", AudioListener.volume);
    }
}
