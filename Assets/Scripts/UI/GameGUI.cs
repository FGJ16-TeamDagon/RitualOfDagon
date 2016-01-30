using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour 
{
    public GameObject optionsPanel;

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

    public void OpenOptions()
    {
        // TODO: open options view
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
    }
}
