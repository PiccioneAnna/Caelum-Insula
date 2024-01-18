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
        [SerializeField] Tilemap tilemap;
        public override bool OnApplyToTileMap(Vector3Int gridPosition, TilemapScripts.Reader tilemapReadController, Item item)
        {
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

