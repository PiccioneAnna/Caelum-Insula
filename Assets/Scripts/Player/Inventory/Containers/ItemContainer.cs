using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Containers/Item Container")]
    public class ItemContainer : ScriptableObject
    {
        public List<Inventory.Slot> slots;
    }
}
