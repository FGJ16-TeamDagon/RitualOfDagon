using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour
{
    public enum GameplayState
    {
        Undefined = 0, // Game is not ready
        Ready = 1, // Game is ready to begin
        TurnStart = 2,
        Playing = 3,
        TurnEnd = 4,
        GameOver = 5
    }

    private static GamePlay instance;
    public static GamePlay Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GamePlay>();
            }

            return instance;
        }
    }

    public Player StrandedPlayer { get; private set; }
    public Player DeepOnesPlayer { get; private set; }
    public Player CurrentPlayer;

    public GameplayState State { get; private set; }

    public GridController grid;

    void Start()
    {
        StrandedPlayer = CreateStrandedPlayer();
        DeepOnesPlayer = CreateDeepOnesPlayer();
    }

    private Player CreateStrandedPlayer()
    {
        var player = new Player("Stranded");

        return player;
    }

    private Player CreateDeepOnesPlayer()
    {
        var player = new Player("Deep Ones");

        return player;
    }

    private void StartGame()
    {
        CurrentPlayer = DeepOnesPlayer;
        State = GameplayState.Ready;
    }
}
