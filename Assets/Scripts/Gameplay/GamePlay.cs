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
        ShowPattern = 2,
        Playing = 3,
        TurnEnd = 4,
        GameOver = 5
    }

    public static event System.Action GamestateChanged;

    private GameCameraController cameraController;

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

    private GameplayState state;
    public GameplayState State
    {
        get
        {
            return state;
        }
        private set
        {
            if (value != state)
            {
                state = value;
                if (GamestateChanged != null) GamestateChanged();
            }
        }
    }
    public GridController grid;

    public Ritual Ritual { get; private set; }

    public Player Winner { get; private set; }

    public int CurrentTurn { get; private set; }
    public int MaxTurns { get; private set; }
    public int TurnsLeft
    {
        get
        {
            return MaxTurns - CurrentTurn;
        }
    }

    void Awake()
    {
        cameraController = Camera.main.GetComponent<GameCameraController>();
        UnityEngine.SceneManagement.SceneManager.LoadScene("UI", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        LeanTween.init();
    }

    void Start()
    {
        StrandedPlayer = CreateStrandedPlayer();
        DeepOnesPlayer = CreateDeepOnesPlayer();

        var ritualFactory = new RitualFactory();

        Ritual = ritualFactory.GetRandomRitual();
        Debug.Log(Ritual.image.name);

        var characters = RandomPermutation(FindObjectsOfType<GameCharacter>());

        foreach (var character in characters)
        {
            if (character.gameObject.tag.ToLower() == "stranded")
            {
                StrandedPlayer.characters.Add(character);
            }
            else if (character.gameObject.tag.ToLower() == "deepone")
            {
                if (DeepOnesPlayer.characters.Count < Ritual.pattern.Length)
                {
                    DeepOnesPlayer.characters.Add(character);
                }
                else
                {
                    Destroy(character.gameObject);
                }
            }
        }

        MaxTurns = Mathf.FloorToInt(Ritual.pattern.Length * 0.5f) + 3;

        State = GameplayState.Ready;

        ShowPattern();   
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

    public void StartGame()
    {
        if (State != GameplayState.Ready && State != GameplayState.ShowPattern)
        {
            return;
        }

        CurrentPlayer = DeepOnesPlayer;
        State = GameplayState.Playing;
        GameCharacter.Selection = DeepOnesPlayer.characters[0];
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
        if (State != GameplayState.GameOver)
        {
            StartCoroutine(StartTurn_Coroutine());
        }
    }

    IEnumerator StartTurn_Coroutine()
    {
        yield return null;

        SoundManager.Instance.PlaySound(SoundManager.SoundEffect.TurnChange);

        State = GameplayState.Playing;
        if (CurrentPlayer == StrandedPlayer)
        {
            GameCharacter.Selection = StrandedPlayer.characters[0];
        }
        else
        {
            CurrentTurn++;
            
            Debug.Log("Turn " + CurrentTurn + "/" + MaxTurns);
            
            if (TurnsLeft > 0)
            {
                GameCharacter.Selection = DeepOnesPlayer.characters[0];
            }
        }
    }

    public void EndTurn()
    {
        if (CurrentPlayer == DeepOnesPlayer)
        {
            CurrentPlayer = StrandedPlayer;
            var bestFit = Ritual.BestFit(DeepOnesPlayer.characters);
            Debug.Log("Ritual fit: " + bestFit + "/" + Ritual.pattern.Length);
            if (bestFit >= Ritual.pattern.Length)
            {
                DeepOneVictory();
            }
            else if (TurnsLeft <= 1)
            {
                StrandedVictory();
            }
            else if (bestFit == Ritual.pattern.Length - 1)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundEffect.DeepOneVictoryWarning);
            }
        }
        else
        {
            CurrentPlayer = DeepOnesPlayer;
        }
        GameCharacter.Selection = null;
        if (State != GameplayState.GameOver)
        {
            State = GameplayState.TurnEnd;
        }
    }
    
    private void DeepOneVictory()
    {
        Winner = DeepOnesPlayer;
        Debug.Log("DeepOneVictory");
        State = GameplayState.GameOver;
        var prefab = Resources.Load<GameObject>("RitualEffect");
        Instantiate(prefab);
        GameGUI.Instance.DeepOneEnd();
        cameraController.Shake();
    }

    private void StrandedVictory()
    {
        Winner = StrandedPlayer;
        Debug.Log("StrandedVictory");
        State = GameplayState.GameOver;
        var effectPrefab = Resources.Load<GameObject>("StrandedVictoryEffect");
        Instantiate(effectPrefab);
        GameGUI.Instance.StrandedEnd();
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

    public void ShowPattern()
    {
        State = GameplayState.ShowPattern;
    }

    public void AutoSelectNext()
    {
        var current = GameCharacter.Selection;

        int index = current == null ? 0 : CurrentPlayer.characters.IndexOf(current);

        current = CurrentPlayer.characters[index];
        int attempts = 0;

        while (current.MovementLeft <= 0)
        {
            attempts++;
            index++;
            if (index >= CurrentPlayer.characters.Count) index = 0;

            current = CurrentPlayer.characters[index];

            if (attempts > CurrentPlayer.characters.Count)
            {
                current = null;
                break;
            }
        }

        GameCharacter.Selection = current;

        Debug.Log("Auto selected " + index, current);

        if (current == null)
        {
            cameraController.Shake();
            // TODO: hint turn end
        }
    }
}
