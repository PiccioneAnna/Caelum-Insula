using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

namespace Data
{
    [CreateAssetMenu(menuName = "Game Data/Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        public string id;
        public ItemType itemType;

        public TileBase tile;
        public GameObject obj;
        public Sprite image;

        public bool usesGrid = false;
        public bool stackable = true;
        public bool iconHighlight;

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
