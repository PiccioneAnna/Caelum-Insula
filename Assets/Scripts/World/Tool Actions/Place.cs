using Data;
using UnityEngine;

namespace ToolActions
{
    [CreateAssetMenu(menuName = "Data/Tool Action/Place Object")]
    public class Place : Base
    {
        PlaceableObjectsManager objectsManager;

        public override bool OnApplyToTileMap(Vector3Int gridPosition, TilemapScripts.Reader tilemapReadController, Item item)
        {
            objectsManager = GameManager.Instance.placeableObjectsManager.placeableObjectsManager;

            // If there is an object already in the position then return
            if (objectsManager.Check(gridPosition))
            {
                return false;
            }

            GameManager.Instance.inventory.RemoveItem(item);
            objectsManager.Place(item, gridPosition);
            return true;
        }
    }
}


