using Data;
using TilemapScripts;
using ToolActions;
using UnityEngine;

namespace ToolAction
{
    [CreateAssetMenu(menuName = "Data/Tool Action/Seed Tile")]
    public class SeedTile : Base
    {
        public override bool OnApplyToTileMap(Vector3Int gridPosition, Reader tilemapReadController, Item item)
        {
            if (!GameManager.Instance.cropsManager.Check(gridPosition)) { return false; }

            GameManager.Instance.cropsManager.Seed(gridPosition, item.crop);

            return true;
        }
    }
}
