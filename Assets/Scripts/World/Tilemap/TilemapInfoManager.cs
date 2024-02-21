using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// This class contains all tilemap info to be referenced from gamemanager
public class TilemapInfoManager : MonoBehaviour
{
    [Header("Biome Specific Tiles")]
    public TileBase grass;
    public TileBase dirt;

    [Header("Actual Tile Tilemaps")]
    public List<Tilemap> grassTileMaps;
    public List<Tilemap> dirtTilemaps;

    [Header("Object/Interactable Tilemaps")]
    public List<Tilemap> grassPrefabMaps; // above ground grass that can be cut
    public List<Tilemap> cropTilemaps;
    public List<Tilemap> objectTilemaps;
}
