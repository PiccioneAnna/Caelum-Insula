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
        [SerializeField] Tilemap tilemap;
        public override bool OnApplyToTileMap(Vector3Int gridPosition, TilemapScripts.Reader tilemapReadController, Item item)
        {
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


