using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Containers/Placeable Object Container")]
    public class PlaceableObjectsContainer : ScriptableObject
    {
        public List<PlaceableObject> placeableObjects;

        internal PlaceableObject Get(Vector3Int position)
        {
            foreach (PlaceableObject obj in placeableObjects)
            {
                if (obj.positionOnGrid == position)
                {
                    return obj;
                }
            }
            return null;
        }

        internal void Remove(PlaceableObject placedObject)
        {
            placeableObjects.Remove(placedObject);
        }

        public void Clear()
        {
            placeableObjects.Clear();
        }
    }
}


