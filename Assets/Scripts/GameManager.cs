using Player;
using System.Collections.Generic;
using TilemapScripts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public Inventory.Manager inventory;
    public TilemapScripts.CropsManager cropsManager;
    public Reader reader;
    public MarkerManager markerManager;
    public TimeController timeController;
    public SceneManager sceneManager;
    public OnScreenMessageSystem screenMessageSystem;
    public PlaceableObjectsReferenceManager placeableObjectsManager;
    public Controller player;
    public static GameManager Instance;
    public GameObject itemVisual; // for always having item infront of all UI
    public GetCameraCollider getCameraCollider;

    [Header("Tilemaps")]
    [SerializeField] public Dictionary<string, Tilemap> tilemaps;

    /// <summary>
    /// Class is a singleton, only one should exist at ALL times
    /// </summary>
    void Awake()
    {
        tilemaps = new Dictionary<string, Tilemap>();
        FindTilemaps();

        if (Instance == null) // If there is no instance already
        {
            DontDestroyOnLoad(gameObject); // Keep the GameObject, this component is attached to, across different scenes
            Instance = this;
        }
        else if (Instance != this) // If there is already an instance and it's not `this` instance
        {
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }

        inventory = GetComponent<Inventory.Manager>();
    }

    private void Start()
    {
        getCameraCollider.ResetCameraCollider();
    }

    public void FindTilemaps()
    {
        tilemaps.Clear();

        GameObject grid = GameObject.Find("Tilemap Parent");

        if(grid != null) { return; }

        foreach (Transform tilemap in grid.transform)
        {
            tilemaps.Add(tilemap.name, tilemap.GetComponent<Tilemap>());
        }
    }
}
