using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGeneration : MonoBehaviour
{
    #region Fields
    private CellularAutomata cellularAutomata;
    private TilemapCollider2D tilemapCollider;

    public bool autoGenerate = false;

    [Header("Cellular Automata Stats")]
    [SerializeField] public int _width;
    [SerializeField] public int _height;
    [SerializeField] float _fillPercent;
    [SerializeField] int _liveNeighboursRequired;
    [SerializeField] int _stepCount;

    [HideInInspector] public int[,] cellularAutomata0;
    [HideInInspector] public int[,] cellularAutomata1;
    [HideInInspector] public List<Vector3Int> validPositions;
    [HideInInspector] public List<Vector3Int> wallPositions;

    [Header("Tiles")]
    public TileBase ground;
    public TileBase wall;

    [Header("Conditionals")]
    public bool hasWalls;

    #endregion

    public Tilemap targetGround;
    public Tilemap targetWalls;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Gathering Components & Setup

    private void GetComponents()
    {
        cellularAutomata = gameObject.GetComponent<CellularAutomata>();
        tilemapCollider = targetGround.GetComponent<TilemapCollider2D>();

        cellularAutomata.Set(_width, _height, _fillPercent, _liveNeighboursRequired, _stepCount);
    }

    #endregion

    #region Cellular Automata References
    private void PopulateDataGround()
    {
        cellularAutomata0 = cellularAutomata.GenerateMap(null);
    }

    private void PopulateDataWall()
    {
        cellularAutomata1 = TilemapGenerator.DefineWallPlacement(cellularAutomata0);
    }

    #endregion

    public void GenerateDungeon()
    {
        GetComponents();
        PopulateDataGround();
        PopulateDataWall();

        // Initiating ground floor map
        Debug.Log("Initiating map population...");
        validPositions = TilemapGenerator.PopulateTilemap(targetGround, cellularAutomata0, ground);
        wallPositions = TilemapGenerator.PopulateTilemap(targetWalls, cellularAutomata1, wall);

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
