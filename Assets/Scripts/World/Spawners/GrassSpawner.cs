using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GrassSpawner : TimeAgent
{
    #region Fields

    [Header("Components")]
    public GameObject grassPrefab;
    public Tilemap refTilemap;

    [Header("Bounds")]
    public BoundsInt bounds;

    [Header("Spawned Grass")]
    public List<GameObject> grassList;

    private CellularAutomata cellularAutomata;
    [HideInInspector] public int _width;
    [HideInInspector] public int _height;

    [Header("Cellular Automata Stats")]
    public float _fillPercent;
    public int _liveNeighboursRequired;
    public int _stepCount;
    public float _newGrassSpawnChance;
    private int stepIndex = 1;
    private int iteration = 0;

    public int[,] grassArray;
    public int[,] grassArrayGenerated;
    [HideInInspector] public List<Vector3Int> validPositions;
    [HideInInspector] public List<Vector3Int> spawnedPositions;

    [Header("Debug Data : DNE")]
    public float actualFilledPercent;
    public int removedCellCount;
    public int totalCellCount;
    public int filledCellCount;

    [Header("Visuals - Color")]
    Gradient gradient;

    public GradientColorKey[] colors = new GradientColorKey[4];
    public GradientAlphaKey[] alphas = new GradientAlphaKey[2];

    #endregion

    #region Runtime

    // Start is called before the first frame update
    void Start()
    {
        bounds = TilemapGenerator.ReturnTilemapInfo(refTilemap);
        //Debug.Log("Defining bounds and calling spawner");
        totalCellCount = TilemapGenerator.GetTilemapTileCount(refTilemap);

        cellularAutomata = GetComponent<CellularAutomata>();

        SetGradient();

        onTimeTick += Tick;
        Init();

        SetInitCellularAutomata();
    }

    public void Tick()
    {        
        SpawnGrass();
        grassArray = cellularAutomata.UpdateMap(stepIndex);

        Debug.Log("Grass Regrowth");

        CheckFillPercent();
    }

    #endregion

    #region Helper Methods

    public void SpawnGrass()
    {
        Vector3Int pos;
        int indexX = 0;
        int indexY = 0;
        removedCellCount = 0;

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                pos = new Vector3Int(x, y, 0);
                bool hasTile = refTilemap.HasTile(new Vector3Int(x, y, 0));
                bool inBounds = (indexX < _width && indexY < _height);
                bool alreadySpawned = spawnedPositions.Contains(pos);

                if (inBounds)
                {
                    bool filledTile = grassArray[indexX, indexY] == 1;

                    if (hasTile)
                    {
                        if (!alreadySpawned && filledTile)
                        {
                            GameObject grass = Instantiate(grassPrefab, transform);
                            grass.transform.position = pos;
                            spawnedPositions.Add(pos);

                            SetGrassColor(grass);

                            if (!grassList.Contains(grass)) { grassList.Add(grass); }
                        }
                    }
                    else
                    {
                        if (filledTile) { removedCellCount++; }
                    }
                }

                indexY++;             
            }

            indexY = 0;
            indexX++;
        }

        iteration++;
    }

    public void SetInitCellularAutomata()
    {
        _width = Mathf.Abs(bounds.min.x) + Mathf.Abs(bounds.max.x);
        _height = Mathf.Abs(bounds.min.y) + Mathf.Abs(bounds.max.y);

        Debug.Log(_width + " , " + _height);

        cellularAutomata.Set(_width, _height, _fillPercent, _liveNeighboursRequired, _stepCount, _newGrassSpawnChance, refTilemap);
        grassArray = cellularAutomata.GenerateMap(null);

        //foreach (var grass in grassArray) { Debug.Log(grass); }
    }

    public void CheckFillPercent()
    {
        filledCellCount = cellularAutomata._filledCells;
        
        actualFilledPercent = ((float)(filledCellCount - removedCellCount) / (float)totalCellCount);
    }

    public void SetGrassColor(GameObject grass)
    {
        SpriteRenderer[] renderers = grass.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = gradient.Evaluate(.25f * iteration + .15f);
        }
    }

    public static Color green0 = new Color32(46, 158, 57, 255);
    public static Color green1 = new Color32(39, 150, 50, 255);
    public static Color green2 = new Color32(87, 201, 98, 255);
    public static Color green3 = new Color32(255, 255, 255, 255);

    public void SetGradient()
    {
        gradient = new Gradient();

        colors[0] = new GradientColorKey(green0, 0f);
        colors[1] = new GradientColorKey(green1, .35f);
        colors[2] = new GradientColorKey(green2, .65f);
        colors[3] = new GradientColorKey(green3, 1f);

        alphas[0] = new GradientAlphaKey(1.0f, 0f);
        alphas[1] = new GradientAlphaKey(0.75f, 1.0f);

        gradient.SetKeys(colors, alphas);
    }

    #endregion
}
