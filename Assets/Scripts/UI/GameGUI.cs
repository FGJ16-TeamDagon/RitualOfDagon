using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour 
{
    public enum GUIState
    {
        Hidden,
        GameGUI,
        Options,
    }

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
        State = GUIState.Options;
    }

    public void OpenGameGUI()
    {
        // TODO: if hidden/options, transition to game GUI
        State = GUIState.GameGUI;
    }
}
