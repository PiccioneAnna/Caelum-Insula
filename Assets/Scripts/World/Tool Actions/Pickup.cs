using Data;
using UnityEngine;

namespace ToolActions
{
    [CreateAssetMenu(menuName = "Data/Tool Action/Pick Up")]
    public class Pickup : Base
    {
        public override bool OnApply(Vector2 gridPosition)
        {
            Vector3Int newpos = new Vector3Int((int)gridPosition.x, (int)gridPosition.y, 0);

            GameManager.Instance.cropsManager.PickUp(newpos);

            //GameManager.Instance.placeableObjectsManager.placeableObjectsManager.PickUp(newpos);

            return true;
        }
    }
}


