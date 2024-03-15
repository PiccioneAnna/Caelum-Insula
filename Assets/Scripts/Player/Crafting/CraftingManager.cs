using Data;
using System.Collections.Generic;
using UnityEngine;

namespace Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        public static CraftingManager craftingManager;

        public Crafting crafting;

        public List<CraftRecipe> knownRecipes;
        public CraftingSlot[] craftingSlots;
        public GameObject inventoryItemPrefab;

        void Awake()
        {
            craftingManager = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            foreach (CraftRecipe recipe in knownRecipes)
            {
                AddItem(recipe.output);
            }
        }

        public void LearnRecipe(CraftRecipe recipe)
        {
            knownRecipes.Add(recipe);
            AddItem(recipe.output);
        }

        public bool AddItem(Item item)
        {
            // Find any empty slot
            for (int i = 0; i < knownRecipes.Count; i++)
            {
                CraftingSlot slot = craftingSlots[i];

                if (slot == null) { return false; }

                CraftableItem itemInSlot = slot.GetComponentInChildren<CraftableItem>();
                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot);
                    slot.ItemInSlot = item;
                    slot.image.color = Color.clear;
                    return true;
                }
            }
            return false;
        }

        void SpawnNewItem(Item item, CraftingSlot slot)
        {
            GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
            CraftableItem craftingItem = newItemGo.GetComponent<CraftableItem>();
            craftingItem.InitialiseItem(item);
        }
    }
}
