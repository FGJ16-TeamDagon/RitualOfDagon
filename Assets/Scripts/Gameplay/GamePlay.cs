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

        var characters = FindObjectsOfType<GameCharacter>();

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].gameObject.tag.ToLower() == "stranded")
            {
                StrandedPlayer.characters.Add(characters[i]);
            }
            else if (characters[i].gameObject.tag.ToLower() == "deepone")
            {
                DeepOnesPlayer.characters.Add(characters[i]);
            }
        }

        StartGame();
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
        State = GameplayState.Playing;
    }

    private GameObject touchStartObject;
    private float touchDelta;
    private Vector3 lastMousePos;

    private void Update()
    {
        if (State == GameplayState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    touchStartObject = hit.collider.attachedRigidbody ? hit.collider.attachedRigidbody.gameObject : hit.collider.gameObject;
                    Debug.Log("hit: " + touchStartObject, touchStartObject);
                }
                else
                {
                    touchStartObject = null;
                    Debug.Log("miss");
                }

                touchDelta = 0;
            }
            else if (touchStartObject != null && Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    var hitGO = hit.collider.attachedRigidbody ? hit.collider.attachedRigidbody.gameObject : hit.collider.gameObject;
                    if (hitGO == touchStartObject)
                    {
                        HandleTouch(touchStartObject);
                    }
                }

                touchStartObject = null;
            }
            else if (Input.GetMouseButton(0))
            {
                touchDelta += (Input.mousePosition - lastMousePos).sqrMagnitude;

                if (touchDelta > 200)
                {
                    touchStartObject = null;
                }
            }
        }

        lastMousePos = Input.mousePosition;
    }

    void HandleTouch(GameObject target)
    {
        var character = target.GetComponent<GameCharacter>();
        if (character != null)
        {
            GameCharacter.Selection = character;
        }
        else
        {
            var ground = target.GetComponent<GridPosition>();
            if (ground != null 
                && GameCharacter.Selection != null 
                && CurrentPlayer != null
                && CurrentPlayer.characters.Contains(GameCharacter.Selection))
            {
                GameCharacter.Selection.MoveTowards(ground);
            }
        }
        
    }
}
