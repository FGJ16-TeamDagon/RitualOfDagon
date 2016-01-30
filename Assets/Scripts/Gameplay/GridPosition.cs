using UnityEngine;
using System.Collections;

public class GridPosition 
{
    private void OnEnable()
    {
        GamePlay.Instance.grid.AddPoint(this);
    }

    private void OnDisable()
    {
        GamePlay.Instance.grid.RemovePoint(this);
    }
}
