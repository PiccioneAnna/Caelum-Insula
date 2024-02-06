using Data;
using UnityEngine;

namespace ToolActions
{
    [CreateAssetMenu(menuName = "Data/Tool Action/Pick Up")]
    public class Pickup : Base
    {
        public override bool OnApplyToTileMap(Vector3Int gridPosition, TilemapScripts.Reader tilemapReadController, Item item)
        {
            GameManager.Instance.cropsManager.PickUp(gridPosition);

            GameManager.Instance.placeableObjectsManager.placeableObjectsManager.PickUp(gridPosition);

            return true;
        }
    }
}


