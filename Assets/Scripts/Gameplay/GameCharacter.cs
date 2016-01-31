using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameCharacter : MonoBehaviour
{
    public struct SelectionEvent
    {
        public GameCharacter newSelection;
        public GameCharacter oldSelection;
        public SelectionEvent(GameCharacter oldSelection, GameCharacter newSelection)
        {
            this.newSelection = newSelection;
            this.oldSelection = oldSelection;
        }
    }

    [SerializeField]
    private int movementPoints;
    private int usedMovementPoints;
    public int MovementLeft
    {
        get
        {
            return movementPoints - usedMovementPoints;
        }
    }

    public static event System.Action<SelectionEvent> Selected;
    
    LTDescr tween;
    List<GridPosition> path;
    int pathIndex;
    [SerializeField]
    private float moveTime = 0.5f;

    private UnityEngine.UI.Image movementPointsFill;

    private static GameCharacter selection;
    public static GameCharacter Selection
    {
        get
        {
            return selection;
        }
        set
        {
            var oldSelection = selection;
            var newSelection = value;

            if (selection != value)
            {
                if (selection) selection.HandleSelected(false);
                selection = value;
                if (selection) selection.HandleSelected(true);

                if (oldSelection != newSelection && Selected != null)
                {
                    Selected(new SelectionEvent(oldSelection, newSelection));
                }
            }
        }
    }

    void OnEnable()
    {
        GamePlay.GamestateChanged += GamePlay_GamestateChanged;
    }

    void OnDisable()
    {
        GamePlay.GamestateChanged -= GamePlay_GamestateChanged;
    }

    private void GamePlay_GamestateChanged()
    {
        if (GamePlay.Instance.State == GamePlay.GameplayState.Playing
            && GamePlay.Instance.CurrentPlayer.characters.Contains(this))
        {
            Reset();
        }
        else
        {
            if (selectionIndicatorBG) selectionIndicatorBG.SetActive(false);
        }
    }

    private GridPosition position;
    public GridPosition Position
    {
        get
        {
            if (position == null)
            {
                position = GamePlay.Instance.grid.PositionToCell(transform.position);
            }
            return position;
        }
        private set
        {
            position = value;
        }
    }

    [SerializeField]
    private GameObject selectionIndicator;
    [SerializeField]
    private GameObject selectionIndicatorBG;
    [SerializeField]
    private ParticleSystem moveParticles;

    public void HandleSelected(bool selected)
    {
        if (selectionIndicator) selectionIndicator.SetActive(selected);

        if (movementPointsFill)
        {
            movementPointsFill.fillAmount = (float)MovementLeft / (float)movementPoints;
        }

        if (MovementLeft > 0 && selectionIndicatorBG)
        {
            selectionIndicatorBG.SetActive(true);
        }
        else
        {
            selectionIndicatorBG.SetActive(false);
        }
    }

    public void MoveTowards(GridPosition target)
    {
        if (MovementLeft <= 0)
        {
            if (selection == this) selection = null;
            return;
        }

        var lookPos = target.GetWorldPos();
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
        
        path = GamePlay.Instance.grid.FindPath(Position, target);

        if (path.Count > 0)
        {
            if (tween != null) LeanTween.cancel(tween.id);

            SetUsedMovementPoint(usedMovementPoints + 1);
            tween = LeanTween.move(gameObject, path[0].GetWorldPos(), moveTime);
            tween.setEase(LeanTweenType.easeInOutQuad);
            Position.occupant = null;
            Position = path[0];
            Position.occupant = gameObject;
            pathIndex = 0;
            tween.onComplete = () =>
            {
                OnCompletePathStep();
            } ;
            if (moveParticles) moveParticles.Play();
        }
    }

    void Update()
    {
        if (movementPointsFill)
        {
            if (movementPointsFill.fillAmount <= 0)
            {
                selectionIndicator.SetActive(false);
                selectionIndicatorBG.SetActive(false);
            }

            if (movementPointsFill.gameObject.activeInHierarchy)
            {
                var targetFill = (float)MovementLeft / (float)movementPoints;
                movementPointsFill.fillAmount = Mathf.MoveTowards(movementPointsFill.fillAmount, targetFill, Time.deltaTime);
            }
        }
    }

    void OnCompletePathStep()
    {
        pathIndex++;
        
        if (pathIndex < path.Count 
            && GamePlay.Instance.CurrentPlayer != null
            && GamePlay.Instance.CurrentPlayer.characters.Contains(this) 
            && GamePlay.Instance.State == GamePlay.GameplayState.Playing
            && MovementLeft > 0)
        {
            
            var target = path[pathIndex];

            if (target.occupant != null)
            {
                path = null;
                return;
            }

            if (tween != null) LeanTween.cancel(tween.id);

            var targetPos = target.GetWorldPos();
            tween = LeanTween.move(gameObject, target.GetWorldPos(), moveTime);
            tween.setEase(LeanTweenType.easeInOutQuad);
            Position.occupant = null;
            Position = path[pathIndex];
            Position.occupant = gameObject;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
            SetUsedMovementPoint(usedMovementPoints + 1);

            tween.onComplete = OnCompletePathStep;

            if (moveParticles) moveParticles.Play();
        }
        else
        {
            path = null;
        }
    }

    private void Reset()
    {
        if (selectionIndicatorBG) selectionIndicatorBG.SetActive(true);
        if (movementPointsFill) movementPointsFill.fillAmount = 1;
        SetUsedMovementPoint(0);
    }

    void SetUsedMovementPoint(int amount)
    {
        if (movementPointsFill == null && selectionIndicator)
        {
            movementPointsFill = selectionIndicator.GetComponent<UnityEngine.UI.Image>();
        }

        usedMovementPoints = amount;

        if (MovementLeft <= 0)
        {
            GamePlay.Instance.AutoSelectNext();
        }
    }
}
