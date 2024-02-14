using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    private CellularAutomata cellularAutomata;
    private TileManager tileManager;
    private TilemapCollider2D tilemapCollider;
    public EnviroSpawner enviroSpawner;

    public int[,] _cellularAutomata0;

    public Tilemap _targetTilemap;
    public TileBase _activeTile;

    private int _width;
    private int _height;

    // Start is called before the first frame update
    void Start()
    {
        cellularAutomata = GetComponent<CellularAutomata>();
        tileManager = GetComponent<TileManager>();
        tilemapCollider = _targetTilemap.GetComponent<TilemapCollider2D>();

        _width = cellularAutomata._width;
        _height = cellularAutomata._height;

        PopulateData();

        // Initiating ground floor map
        Debug.Log("Initiating map population...");
        PopulateTilemap(_targetTilemap, _cellularAutomata0);

        Debug.Log("Map generation complete.");
        tilemapCollider.ProcessTilemapChanges();

        SetPlayerPositionValidity();
    }

    // Populates the different arrays w reference to the previous array (for constraints)
    void PopulateData()
    {
        _cellularAutomata0 = cellularAutomata.GenerateMap(null);
    }

    // Converts 2d array returned from cellular automata to a tilemap
    void PopulateTilemap(Tilemap tilemap, int[,] data)
    {
        // returns if the tilemap or the data is null
        if (tilemap == null || data == null)
        {
            return;
        }

        tilemap.ClearAllTiles();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (data[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), _activeTile);
                }
            }
        }
    }

    // Method to determine which tile to spawn
    void DetermineTile()
    {

    }

    /// <summary>
    /// Makes sure that the player doesn't spawn out of bounds
    /// </summary>
    private void SetPlayerPositionValidity()
    {
        GameObject player = GameManager.Instance.player.gameObject;

        PosCheck(player);
        PosCheck(player); // second check should catch if first check fails
    }

    private void PosCheck(GameObject player)
    {
        if (!tilemapCollider.bounds.Contains(player.transform.position))
        {
            Debug.Log("Invalid spawn, adjusting position...");
            player.transform.position = tilemapCollider.ClosestPoint(player.transform.position);
        }
    }

    private void SecondPosCheck(GameObject player)
    {
        Bounds bounds = tilemapCollider.bounds;

        if (!tilemapCollider.bounds.Contains(player.transform.position))
        {
            Debug.Log("Invalid spawn, adjusting position...");
        }
    }
}

