using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGeneration : MonoBehaviour
{
    #region Fields

    private CellularAutomata cellularAutomata;
    private TilemapCollider2D tilemapCollider;

    public bool autoGenerate = false;

    [Header("Visuals")]
    [SerializeField] public int wallHeight = 2;

    [Header("Cellular Automata Stats")]
    public int _width;
    public int _height;
    [SerializeField] float _fillPercent;
    [SerializeField] int _liveNeighboursRequired;
    [SerializeField] int _stepCount;

    [HideInInspector] public int[,] baseLevel;
    [HideInInspector] public int[,] wallLevel;
    [HideInInspector] public List<Vector3Int> validPositions;
    [HideInInspector] public List<Vector3Int> wallPositions;

    [Header("Tiles")]
    public TileBase ground;
    public TileBase wall;

    [Header("Conditionals")]
    public bool hasWalls;

    public Tilemap targetGround;
    public Tilemap targetWalls;

    #endregion

    #region Runtime Only

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Gathering Components & Setup

    private void GetComponents()
    {
        cellularAutomata = gameObject.GetComponent<CellularAutomata>();
        tilemapCollider = targetGround.GetComponent<TilemapCollider2D>();
    }

    #endregion

    #region Cellular Automata References
    private void PopulateDataGround()
    {
        baseLevel = cellularAutomata.GenerateMap(null);
    }

    private void PopulateDataWall()
    {
        wallLevel = TilemapGenerator.DefineWallPlacement(baseLevel);
    }

    #endregion

    #region Dungeon Customization Visually

    private void ChangeWallHeight()
    {
        for (int x = 0; x < wallLevel.GetLength(0); x++)
        {
            for (int y = 0; y < wallLevel.GetLength(1); y++)
            {
                if ((wallLevel[x,y] == 1) && (y - 1 >= 0))
                {
                    wallLevel[x,y-1] = 1;
                }
            }
        }
    }

    #endregion

    public void GenerateDungeon()
    {
        GetComponents();
        PopulateDataGround();
        PopulateDataWall();

        ChangeWallHeight(); // defaults to 2

        // Initiating ground floor map
        Debug.Log("Initiating map population...");
        validPositions = TilemapGenerator.PopulateTilemap(targetGround, baseLevel, ground);
        wallPositions = TilemapGenerator.PopulateTilemap(targetWalls, wallLevel, wall);

        Debug.Log("Map generation complete.");
        tilemapCollider.ProcessTilemapChanges();

        if (GameManager.Instance) { PosCheck(); }
    }

    #region Player Considerations

    /// <summary>
    /// Makes sure that the player doesn't spawn out of bounds
    /// </summary>
    private void PosCheck()
    {
        GameObject player = GameManager.Instance.player.gameObject;

        if (!tilemapCollider.bounds.Contains(player.transform.position))
        {
            Debug.Log("Invalid spawn, adjusting position...");
            player.transform.position = TilemapGenerator.GetRandomPos(validPositions);
        }
    }

    #endregion
}
