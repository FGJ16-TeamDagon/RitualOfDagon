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
                turnsLeftDisplay.text = GamePlay.Instance.TurnsLeft + " turns to complete ritual";
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
        strandedCanvas.SetActive(true);
        uiCanvas.SetActive(false);
    }

    public void DeepOneEnd()
    {
        deepOneCanvas.SetActive(true);
        uiCanvas.SetActive(false);
    }
}
