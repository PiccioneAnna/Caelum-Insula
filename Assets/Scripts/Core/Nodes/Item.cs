using UnityEngine;
using UnityEngine.Tilemaps;
using Data;
using System;
using Crafting;

[Serializable]
public struct ItemForPickup
{
    [SerializeField] public Item item;
    public int count;
}

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
        public bool isWeapon;
        public bool isMelee;
        public bool isRanged;
        public bool isConsumable;

        [Header("Stats")]
        public float damage = 2f;

        [Header("Dynamic")]
        public TileBase tile;
        public GameObject obj;
        public Crop crop;
        public GameObject treeN;
        public GameObject projectile;
        public CraftRecipe recipe;

        [Header("Actions")]
        public ToolActions.Base onAction;
        public ToolActions.Base onTileMapAction;
        public ToolActions.Base onItemUsed;

        public enum ItemType
        {
            Crop,
            TreeSeed,
            Material,
            Tool,
            Weapon,
            PlaceableObject,
            Consumable
        }
    }
}
