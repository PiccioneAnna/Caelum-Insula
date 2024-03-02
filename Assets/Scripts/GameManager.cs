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
    public TilemapInfoManager tilemapInfoManager;
    public EnviroSpawner enviroSpawner;

    /// <summary>
    /// Class is a singleton, only one should exist at ALL times
    /// </summary>
    void Awake()
    {
        FindTilemaps();

        if (Instance == null) // If there is no instance already
        {
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
        GameObject grid = GameObject.Find("Tilemap Parent");

        if (grid == null) { return; }

        tilemapInfoManager = grid.GetComponent<TilemapInfoManager>();
    }

    public void FindEnviroSpawner()
    {
        enviroSpawner = FindAnyObjectByType<EnviroSpawner>();
    }
}
