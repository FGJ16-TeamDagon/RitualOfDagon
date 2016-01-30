using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridController : MonoBehaviour 
{
    [SerializeField]
    private GameObject linePrefab;

    public int sizeX = 8;
    public int sizeZ = 8;

    private List<LineRenderer> lines;

    public List<GridPosition> points = new List<GridPosition>();

    void Start()
    {
        CreateLines();
    }

    void CreateLines()
    {
        lines = new List<LineRenderer>();

        for (int x = 0; x < sizeX + 1; x++)
        {
            var xPos = -Mathf.FloorToInt(sizeX * 0.5f) + x;
            lines.Add(CreateVerticalLineRenderer(xPos));
        }

        for (int z = 0; z < sizeZ + 1; z++)
        {
            var zPos = -Mathf.FloorToInt(sizeZ * 0.5f) + z;
            lines.Add(CreateHorizontalLineRenderer(zPos));
        }
    }

    private LineRenderer CreateHorizontalLineRenderer(float zPos)
    {
        var go = Instantiate<GameObject>(linePrefab);
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.forward * zPos;
        go.transform.localRotation = Quaternion.identity;
        var line = go.GetComponent<LineRenderer>();
        line.SetPosition(0, new Vector3(-sizeX * 0.5f, 0, 0));
        line.SetPosition(1, new Vector3(sizeZ * 0.5f, 0, 0));
        return line;
    }

    private LineRenderer CreateVerticalLineRenderer(float xPos)
    {
        var go = Instantiate<GameObject>(linePrefab);
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.right * xPos;
        go.transform.localRotation = Quaternion.identity;
        var line = go.GetComponent<LineRenderer>();
        line.SetPosition(0, new Vector3(0, 0, -sizeZ * 0.5f));
        line.SetPosition(1, new Vector3(0, 0, sizeZ * 0.5f));
        return line;
    }

    public void AddPoint(GridPosition point)
    {
        if (!points.Contains(point))
        {
            points.Add(point);
        }
    }

    public void RemovePoint(GridPosition point)
    {
        if (points.Contains(point))
        {
            points.Remove(point);
        }
    }
}
