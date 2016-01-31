using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridController : MonoBehaviour 
{
    [SerializeField]
    private GameObject linePrefab;

    [SerializeField]
    private GameObject gridCellPrefab;

    public int sizeX = 8;
    public int sizeZ = 8;

    private List<LineRenderer> lines;

    private Transform linesParent;
    private Transform cellsParent;

    public GridPosition[,] points;

    private Color gridColor;
    private Color gridTargetColor;
    private Color gridColorTweenStartColor;
    private LTDescr colorTween;
    private float linesAlpha = 0.3f;

    void Start()
    {
        CreateLines();
        CreateCells();
        OccupyCells();
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
        if (GamePlay.Instance.State == GamePlay.GameplayState.Playing)
        {
            gridColorTweenStartColor = gridColor;

            if (GamePlay.Instance.CurrentPlayer == GamePlay.Instance.DeepOnesPlayer)
            {
                gridTargetColor = Player.DeepOneColorDark;
            }
            else
            {
                gridTargetColor = Player.StrandedColorDark;
            }
            gridTargetColor.a = linesAlpha;

            if (colorTween != null) LeanTween.cancel(colorTween.id);

            colorTween = LeanTween.value(gameObject, (c) => {
                gridColor = c;
               
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i].SetColors(gridColor, gridColor);
                }
            }, gridColorTweenStartColor, gridTargetColor, 1f);
        }
    }

    void CreateLines()
    {
        linesParent = new GameObject("Lines parent").transform;
        linesParent.SetParent(transform, false);

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

    private void CreateCells()
    {
        points = new GridPosition[sizeX, sizeZ];

        cellsParent = new GameObject("Cells parent").transform;
        cellsParent.SetParent(transform, false);

        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                var go = Instantiate<GameObject>(gridCellPrefab);
                var cell = go.GetComponent<GridPosition>();
                cell.SetGridPosition(x, z);
                go.transform.SetParent(cellsParent, true);
                points[x, z] = cell;
            }
        }
    }

    private LineRenderer CreateHorizontalLineRenderer(float zPos)
    {
        var go = Instantiate<GameObject>(linePrefab);
        go.transform.SetParent(linesParent, true);
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
        go.transform.SetParent(linesParent, true);
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.right * xPos;
        go.transform.localRotation = Quaternion.identity;
        var line = go.GetComponent<LineRenderer>();
        line.SetPosition(0, new Vector3(0, 0, -sizeZ * 0.5f));
        line.SetPosition(1, new Vector3(0, 0, sizeZ * 0.5f));
        return line;
    }

    public List<GridPosition> FindPath(GridPosition origin, GridPosition goal)
    {
        AStar pathFinder = new AStar();
        pathFinder.FindPath(origin, goal, points, true);
        return pathFinder.CellsFromPath();
    }

    public GridPosition PositionToCell(Vector3 position)
    {
        GridPosition pos = null;

        for (int x = 0; x < points.GetLength(0); x++)
        {
            for (int z = 0; z < points.GetLength(1); z++)
            {
                if (points[x,z].ContainsPoint(position))
                {
                    pos = points[x, z];
                    break;
                }   
            }
            if (pos != null) break;
        }

        return pos;
    }

    private void OccupyCells()
    {
        for (int x = 0; x < points.GetLength(0); x++)
        {
            for (int z = 0; z < points.GetLength(1); z++)
            {
                RaycastHit hit;
                Ray ray = new Ray(points[x, z].transform.position + Vector3.up, -Vector3.up);
                if (Physics.Raycast(ray, out hit, 10))
                {
                    if (hit.collider.gameObject != points[x, z].gameObject)
                    {
                        points[x, z].occupant = hit.collider.gameObject;
                    }
                }
            }
        }
    }
}
