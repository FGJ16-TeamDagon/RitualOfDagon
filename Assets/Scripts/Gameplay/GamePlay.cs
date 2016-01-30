using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GamePlay : MonoBehaviour
{
    public enum GameplayState
    {
        Undefined = 0, // Game is not ready
        Ready = 1, // Game is ready to begin
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

    public Ritual ritual;

    [SerializeField]
    private RitualEffect ritualEffect;

    void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("UI", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    void Start()
    {
        StrandedPlayer = CreateStrandedPlayer();
        DeepOnesPlayer = CreateDeepOnesPlayer();

        ritual = new Ritual();
        ritual.pattern = new Ritual.Point[3];
        ritual.pattern[0] = new Ritual.Point(0, 0);
        ritual.pattern[1] = new Ritual.Point(1, 0);
        ritual.pattern[2] = new Ritual.Point(2, 0);

        var characters = RandomPermutation(FindObjectsOfType<GameCharacter>());

        foreach (var character in characters)
        {
            if (character.gameObject.tag.ToLower() == "stranded")
            {
                StrandedPlayer.characters.Add(character);
            }
            else if (character.gameObject.tag.ToLower() == "deepone")
            {
                if (DeepOnesPlayer.characters.Count < ritual.pattern.Length)
                {
                    DeepOnesPlayer.characters.Add(character);
                }
                else
                {
                    Destroy(character.gameObject);
                }
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
            if (Input.GetKeyUp(KeyCode.Return))
            {
                EndTurn();
            }
            else if (Input.GetMouseButtonDown(0))
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
        if (State != GameplayState.Playing) return;

        var character = target.GetComponent<GameCharacter>();
        if (character != null && CurrentPlayer != null && CurrentPlayer.characters.Contains(character))
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

    public void StartTurn()
    {
        StartCoroutine(StartTurn_Coroutine());
    }

    IEnumerator StartTurn_Coroutine()
    {
        yield return null;
        State = GameplayState.Playing;
        if (CurrentPlayer == StrandedPlayer)
        {
            GameCharacter.Selection = StrandedPlayer.characters[0];
        }
    }

    public void EndTurn()
    {
        if (CurrentPlayer == DeepOnesPlayer)
        {
            CurrentPlayer = StrandedPlayer;
            var bestFit = ritual.BestFit(DeepOnesPlayer.characters);
            Debug.Log("Ritual fit: " + bestFit + "/" + ritual.pattern.Length);
            if (bestFit >= ritual.pattern.Length)
            {
                DeepOneVictory();
            }
        }
        else
        {
            CurrentPlayer = DeepOnesPlayer;
        }
        GameCharacter.Selection = null;
        State = GameplayState.TurnEnd;
    }

    private void DeepOneVictory()
    {
        State = GameplayState.GameOver;
        ritualEffect.StartEffect(ritual);
    }

    public static IEnumerable<T> RandomPermutation<T>(IEnumerable<T> sequence)
    {
        T[] retArray = sequence.ToArray();
        System.Random random = new System.Random();

        for (int i = 0; i < retArray.Length - 1; i += 1)
        {
            int swapIndex = random.Next(i, retArray.Length);
            if (swapIndex != i)
            {
                T temp = retArray[i];
                retArray[i] = retArray[swapIndex];
                retArray[swapIndex] = temp;
            }
        }

        return retArray;
    }
}
