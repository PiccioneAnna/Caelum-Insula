using System.Collections.Generic;
using UnityEngine;
using Inventory;

namespace Crafting
{
    public class Crafting : MonoBehaviour
    {
        [SerializeField] Manager inventory;
        [SerializeField] CraftingManager craftingManager;
        public List<CraftingSlot> craftingSlots;

        public void Awake()
        {
            ClearSlotBackground();
        }

        private void ClearSlotBackground()
        {
            foreach (CraftingSlot slot in craftingSlots)
            {
                slot.image.color = Color.clear;
            }
        }

        private void CheckCraftableItems()
        {
            for (int i = 0; i < craftingManager.knownRecipes.Count; i++)
            {
                if (!CheckForMaterials(craftingManager.knownRecipes[i]))
                {
                    craftingSlots[i].GetComponentInChildren<CraftableItem>().image.color = Color.black;
                }
                else
                {
                    craftingSlots[i].GetComponentInChildren<CraftableItem>().image.color = Color.white;
                }
            }

            ClearSlotBackground();
        }

        public void OnEnable()
        {
            ClearSlotBackground();
            CheckCraftableItems();
        }

        public void Craft(CraftRecipe recipe)
        {
            // If there is free space
            if (!inventory.CheckFreeSpace())
            {
                Debug.Log("Not enough space to craft object");
                return;
            }

            if (CheckForMaterials(recipe) == false) { return; }

            // Remove crafting components from inventory
            for (int i = 0; i < recipe.inputs.Count; i++)
            {
                inventory.RemoveItem(recipe.inputs[i].item, recipe.inputs[i].count);
            }

            // Add new crafted item
            inventory.AddItem(recipe.output);
            CheckCraftableItems();
        }

        private bool CheckForMaterials(CraftRecipe recipe)
        {
            for (int i = 0; i < recipe.inputs.Count; i++)
            {
                if (inventory.CheckItem(recipe.inputs[i]) == false)
                {
                    Debug.Log("Not enough required elements to craft item");
                    return false;
                }
            }
            return true;
        }
    }
}


