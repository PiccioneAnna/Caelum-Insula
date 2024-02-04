using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ToolActions
{
    [CreateAssetMenu(menuName = "Data/Tool Action/Hoe")]
    public class Hoe : Base
    {
        [SerializeField] List<TileBase> canHoe;
        Tilemap tilemap;
        public override bool OnApplyToTileMap(Vector3Int gridPosition, TilemapScripts.Reader tilemapReadController, Item item)
        {
            tilemap = GameManager.Instance.cropsManager.parentTilemap;

            if (tilemap == null)
            {
                Debug.Log("Target tilemap not set");
                return false;
            }

            TileBase tileToPlow = tilemapReadController.GetTileBase(tilemap, gridPosition);

            if (!canHoe.Contains(tileToPlow))
            {
                return false;
            }

            GameManager.Instance.cropsManager.Plow(gridPosition);

            return true;
        }
    }
}

