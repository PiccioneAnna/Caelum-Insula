using UnityEngine.UI;
using UnityEngine;
using Data;

namespace Crafting
{
    public class CraftingSlot : MonoBehaviour
    {
        public Item item;
        public CraftableItem craftableItem;
        public Image image;
        public Color selectedColor, notSelectedColor;
        public Crafting crafting;

        public Item ItemInSlot { get { return item; } set { item = value; } }

        private void Awake()
        {
            Deselect();
            crafting = GameManager.Instance.GetComponent<CraftingManager>().crafting;
        }

        public void Select()
        {
            image.color = selectedColor;
        }

        public void Deselect()
        {
            image.color = notSelectedColor;
        }

        public void Clear()
        {
            craftableItem = GetComponentInChildren<CraftableItem>();
            Destroy(craftableItem.gameObject);
        }

        public void CraftButton()
        {
            if (ItemInSlot != null)
            {
                if (ItemInSlot.recipe != null)
                {
                    if (crafting != null)
                    {
                        crafting.Craft(ItemInSlot.recipe);
                    }
                }
            }
        }
    }
}

