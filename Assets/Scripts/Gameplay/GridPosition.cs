using UnityEngine;
using System.Collections;

public class GridPosition : MonoBehaviour
{
    public int X { get; private set; }
    public int Z { get; private set; }

    const float gridCellSize = 1;
    static readonly Vector3 gridOffset = new Vector3(-3,0,-3);

    public void SetGridPosition(int x, int z)
    {
        this.X = x;
        this.Z = z;

        transform.position = GetWorldPos();

        gameObject.name = "Cell " + X + " " + Z;
    }

    public GameObject occupant;

    public Vector3 GetWorldPos()
    {
        return gridOffset + gridCellSize * new Vector3(X, 0, Z);
    }

    public bool ContainsPoint(Vector3 point)
    {
        var col = GetComponent<BoxCollider>();
        point.y = col.bounds.center.y;
        return col.bounds.Contains(point);
    }

    public bool IsWalkable()
    {
        return occupant == null;
    }

    public int MovementCost()
    {
        return 1;
    }
}
