using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ToolActions
{
    [CreateAssetMenu(menuName = "Data/Tool Action/Till")]
    public class Till : Base
    {
        [SerializeField] List<TileBase> canTill;
        Tilemap tilemap;
        public override bool OnApplyToTileMap(Vector3Int gridPosition, TilemapScripts.Reader tilemapReadController, Item item)
        {
            tilemap = GameManager.Instance.cropsManager.parentTilemap;

            if (tilemap == null) 
            {
                Debug.Log("Target tilemap not set");
                return false; 
            }

            TileBase tileToTill = tilemapReadController.GetTileBase(tilemap,gridPosition);

            if (!canTill.Contains(tileToTill))
            {
                return false;
            }

            GameManager.Instance.cropsManager.Till(gridPosition);

            return true;
        }
    }
}


