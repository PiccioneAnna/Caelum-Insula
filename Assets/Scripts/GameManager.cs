using Player;
using System.Collections;
using System.Collections.Generic;
using TilemapScripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Inventory.Manager inventory;
    public TilemapScripts.CropsManager cropsManager;
    public TilemapScripts.Reader reader;
    public MarkerManager markerManager;
    public TimeController timeController;
    public Controller player;
    public static GameManager Instance;
    public GameObject itemVisual; // for always having item infront of all UI

    /// <summary>
    /// Class is a singleton, only one should exist at ALL times
    /// </summary>
    void Awake()
    {
        if (Instance == null) // If there is no instance already
        {
            DontDestroyOnLoad(gameObject); // Keep the GameObject, this component is attached to, across different scenes
            Instance = this;
        }
        else if (Instance != this) // If there is already an instance and it's not `this` instance
        {
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }

        inventory = GetComponentInChildren<Inventory.Manager>();
    }
}
