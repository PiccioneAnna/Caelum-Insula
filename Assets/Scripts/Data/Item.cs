using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

namespace Data
{
    [CreateAssetMenu(menuName = "Game Data/Item")]
    public class Item : ScriptableObject
    {
        [Header("Properties")]
        public string itemName;
        public string id;
        public ItemType itemType;
        public Sprite image;

        [Header("Behaviors & Reliances")]
        public bool usesGrid = false;
        public bool stackable = true;
        public bool iconHighlight = false;

        [Header("Dynamic")]
        public TileBase tile;
        public GameObject obj;

        public enum ItemType
        {
            Crop,
            Material,
            Tool,
            PlaceableObject,
            Consumable
        }
    }
}
