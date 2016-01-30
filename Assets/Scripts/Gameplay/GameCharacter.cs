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

    public static event System.Action<SelectionEvent> Selected;

    LTDescr tween;
    List<GridPosition> path;
    int pathIndex;
    [SerializeField]
    private float moveTime = 0.5f;

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

            if (selection == value)
            {
                if (selection != null)
                {
                    selection.HandleSelected(false);
                }
                selection = null;
            }
            else
            {
                if (selection) selection.HandleSelected(false);
                selection = value;
                if (selection) selection.HandleSelected(true);
            }

            if (oldSelection != newSelection && Selected != null)
            {
                Selected(new SelectionEvent(oldSelection, newSelection));
            }
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

    void Start()
    {
        Debug.Log("selectionIndicator", selectionIndicator);
    }
    public void HandleSelected(bool selected)
    {
        selectionIndicator.SetActive(selected);
    }

    public void MoveTowards(GridPosition target)
    {
        if (tween != null) LeanTween.cancel(tween.id);

        var lookPos = target.GetWorldPos();
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
        
        path = GamePlay.Instance.grid.FindPath(Position, target);

        if (path.Count > 0)
        {
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
        }
    }

    void OnCompletePathStep()
    {
        pathIndex++;
        
        if (pathIndex < path.Count 
            && GamePlay.Instance.CurrentPlayer != null
            && GamePlay.Instance.CurrentPlayer.characters.Contains(this) 
            && GamePlay.Instance.State == GamePlay.GameplayState.Playing)
        {
            if (tween != null) LeanTween.cancel(tween.id);

            tween = LeanTween.move(gameObject, path[pathIndex].GetWorldPos(), moveTime);
            tween.setEase(LeanTweenType.easeInOutQuad);
            Position.occupant = null;
            Position = path[pathIndex];
            Position.occupant = gameObject;
            var lookPos = path[pathIndex].GetWorldPos();
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);

            tween.onComplete = OnCompletePathStep;
        }
        else
        {
            path = null;
        }
    }
}
