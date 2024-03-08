using Inventory;
using TilemapScripts;
using UnityEngine;

namespace ToolActions
{
    public class Base : ScriptableObject
    {
        public virtual bool OnApply(Vector2 worldPoint)
        {
            Debug.LogWarning("OnApply is not implimented");
            return true;
        }

        public virtual bool OnApplyToTileMap(Vector3Int tilemapPosition, Reader tilemapReadController, Data.Item item)
        {
            Debug.LogWarning("OnApplyToTileMap is not implimented");
            return true;
        }

        public virtual bool VisualizeOnApplyToTileMap(Vector3Int tilemapPosition, Reader tilemapReadController, Data.Item item)
        {
            Debug.LogWarning("VisualizeOnApplyToTileMap is not implimented");
            return true;
        }

        public virtual void OnItemUsed(Data.Item usedItem, Manager inventory)
        {

        }
    }
}


