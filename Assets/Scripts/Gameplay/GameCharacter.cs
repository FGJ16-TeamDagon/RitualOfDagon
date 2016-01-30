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
                selection = value;
                selection.HandleSelected(true);
            }

            if (oldSelection != newSelection && Selected != null)
            {
                Selected(new SelectionEvent(oldSelection, newSelection));
            }
        }
    }

    public GridPosition position;

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

        if (position == null) position = GamePlay.Instance.grid.PositionToCell(transform.position);

        path = GamePlay.Instance.grid.FindPath(position, target);

        Debug.Log(position.X + " " + position.Z + " " + path.Count + " " + target.X + " " + target.Z);
        if (path.Count > 0)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Debug.Log(path[i].X + ":" + path[i].Z);
            }
            tween = LeanTween.move(gameObject, path[0].GetWorldPos(), moveTime);
            position = path[0];
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
        Debug.Log("OnCompletePathStep " + pathIndex + "/" + path.Count);
        if (pathIndex < path.Count)
        {
            if (tween != null) LeanTween.cancel(tween.id);

            tween = LeanTween.move(gameObject, path[pathIndex].GetWorldPos(), moveTime);
            position = path[pathIndex];
            Debug.Log(position.X + " " + position.Z);
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
