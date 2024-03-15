using Crafting;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Manager : MonoBehaviour
    {
        #region Fields

        [Header("Manager References")]
        public Manager inventoryManager;

        [Header("Prefabs")]
        public GameObject inventoryItemPrefab;

        [Header("UI References")]
        public GameObject inventoryUI;

        [Header("Settings")]
        public int maxCount = 999;
        public int toolbarCount = 8;
        public Data.Item selectedItem;

        [Header("Containers")]
        public Slot[] inventorySlots;

        private int selectedSlot = -1;
        private int selectedSlotIndex = 0;

        [SerializeField] ItemHighlight itemHighlight;

        #endregion

        #region Runtime

        // Start is called before the first frame update
        void Start()
        {
            inventoryManager = this;
        }

        // Update is called once per frame
        void Update()
        {
            // Only does highlight if inventory expanded isn't open
            if (inventoryUI != null && !inventoryUI.activeSelf)
            {
                if(selectedSlot == -1)
                {
                    selectedSlot = selectedSlotIndex;
                    inventorySlots[selectedSlot].Select();
                    ChangeSelectedSlot(0);
                }

                if (Input.inputString != null) { HandleScroll(); }
            }
            else
            {
                if(selectedSlot != -1)
                {
                    selectedSlotIndex = selectedSlot;
                    inventorySlots[selectedSlot].Deselect();
                }

                selectedSlot = -1;
                selectedItem = null;
            }
        }

        #endregion

        #region Helpers

        #region Graphics
        /// <summary>
        /// Method handles scroll behavior in relation to toolbar
        /// </summary>
        protected void HandleScroll()
        {
            // Checks if user is pressing keys for toolbar switch
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number <= toolbarCount)
            {
                ChangeSelectedSlot(number - 1);
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    if (selectedSlot < toolbarCount && selectedSlot >= 1)
                    {
                        ChangeSelectedSlot(selectedSlot - 1);
                    }
                    else
                    {
                        ChangeSelectedSlot(toolbarCount - 1);
                    }
                }
                if (Input.mouseScrollDelta.y < 0)
                {
                    if (selectedSlot < toolbarCount - 1)
                    {
                        ChangeSelectedSlot(selectedSlot + 1);
                    }
                    else
                    {
                        ChangeSelectedSlot(0);
                    }
                }
            }
        }

        void ChangeSelectedSlot(int newValue)
        {
            if (selectedSlot >= 0)
            {
                inventorySlots[selectedSlot].Deselect();
            }

            // Selects the item the player picked in the toolbar, checks if slot is empty first
            inventorySlots[newValue].Select();
            selectedSlot = newValue;
            if (inventorySlots[selectedSlot].GetComponentInChildren<Item>() != null)
            {
                selectedItem = inventorySlots[selectedSlot].GetComponentInChildren<Item>().item;
                //Debug.Log("Selected Item: " + selectedItem.itemName);
                //UpdateHighlightItem(selectedSlot);
            }
            else
            {
                selectedItem = null;
                Debug.Log("Selected Item: Null");
            }
        }
        #endregion

        #region Item Manipulation

        public bool AddItem(Data.Item item)
        {
            // Check if any slot has the same item with count lower then the max
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                Slot slot = inventorySlots[i];
                Item itemInSlot = slot.GetComponentInChildren<Item>();
                if (itemInSlot != null &&
                    itemInSlot.item == item &&
                    itemInSlot.count < maxCount &&
                    itemInSlot.item.stackable)
                {
                    itemInSlot.count++;
                    itemInSlot.RefreshCount();
                    //questManager.CheckIfPlayerHasObject();
                    return true;
                }
            }

            // Find any empty slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                Slot slot = inventorySlots[i];
                Item itemInSlot = slot.GetComponentInChildren<Item>();
                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot);
                    slot.item = slot.GetComponentInChildren<Item>();
                    //Debug.Log(questManager);
                    //questManager.CheckIfPlayerHasObject();
                    return true;
                }
            }
            return false;
        }

        public void RemoveItem(Data.Item item, int count = 1)
        {
            if (item == selectedItem && !item.stackable)
            {
                selectedItem = null;
                //UpdateHighlightItem(selectedSlot);
            }

            if (item.stackable)
            {
                Slot inventorySlot = null;
                foreach (Slot slot in inventorySlots)
                {
                    if (slot.item != null && slot.item.item != null && slot.item.item == item)
                    {
                        inventorySlot = slot;
                        break;
                    }
                }
                if (inventorySlot == null) { return; }

                inventorySlot.item = inventorySlot.gameObject.GetComponentInChildren<Item>();

                if (inventorySlot.item == null) { return; }

                inventorySlot.item.count -= count;

                Debug.Log("Removing 1 item, " + inventorySlot.item.count + "left.");

                inventorySlot.item.RefreshCount();

                if (inventorySlot.item.count <= 0)
                {
                    selectedItem = null;
                    inventorySlot.Clear();
                }
            }
            else
            {
                // Removal of nonstackable items
                while (count > 0)
                {
                    count -= 1;

                    Slot inventorySlot = null;
                    foreach (Slot slot in inventorySlots)
                    {
                        if (slot.item == item)
                        {
                            inventorySlot = slot;
                            inventorySlot.Clear();
                        }
                    }
                    if (inventorySlot == null) { return; }
                }
            }
        }

        void SpawnNewItem(Data.Item item, Slot slot)
        {
            UnityEngine.GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
            Item inventoryItem = newItemGo.GetComponent<Item>();
            inventoryItem.InitialiseItem(item);
        }

        #endregion

        #region Checks & External References

        public bool CheckFreeSpace()
        {
            // Find any empty slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                Slot slot = inventorySlots[i];
                Item itemInSlot = slot.GetComponentInChildren<Item>();
                if (itemInSlot == null)
                {
                    return true;
                }
            }
            return false;
        }

        public int CheckItemCount(Data.Item i)
        {
            Slot inventorySlot = null;

            foreach (Slot slot in inventorySlots)
            {
                slot.item = slot.gameObject.GetComponentInChildren<Item>();

                if (slot.item != null &&
                    slot.item.item != null &&
                    slot.item == i)
                {
                    inventorySlot = slot;
                }
            }

            if (inventorySlot == null) { return 0; }

            return inventorySlot.gameObject.GetComponentInChildren<Item>().count;
        }

        public bool CheckItem(RecipeElement itemtoCheck)
        {
            Slot inventorySlot = null;

            foreach (Slot slot in inventorySlots)
            {
                slot.item = slot.gameObject.GetComponentInChildren<Item>();

                if (slot.item != null &&
                    slot.item.item != null &&
                    slot.item.item == itemtoCheck.item)
                {
                    inventorySlot = slot;
                }
            }

            if (inventorySlot == null) { return false; }

            if (itemtoCheck.item.stackable) { return inventorySlot.item.count >= itemtoCheck.count; }

            return true;
        }

        public Item GetSelectedItem(bool use)
        {
            Slot slot = inventorySlots[selectedSlot];
            Item itemInSlot = slot.GetComponentInChildren<Item>();
            if (itemInSlot != null)
            {
                Data.Item item = itemInSlot.item;
                if (use == true)
                {
                    itemInSlot.count--;
                    if (itemInSlot.count <= 0)
                    {
                        Destroy(itemInSlot.gameObject);
                    }
                    else
                    {
                        itemInSlot.RefreshCount();
                    }
                }
            }

            return null;
        }

        public List<Item> GetCurrentItems()
        {
            List<Item> currentItems = new List<Item>();

            foreach (Slot slot in inventorySlots)
            {
                if (slot != null && slot.item != null && !currentItems.Contains(slot.item))
                {
                    currentItems.Add(slot.item);
                }
            }

            return currentItems;
        }

        //public void UpdateHighlightItem(int id)
        //{
        //    Data.Item item = selectedItem;
        //    if (item == null)
        //    {
        //        itemHighlight.Show = false;
        //        return;
        //    }

        //    itemHighlight.Show = item.iconHighlight;

        //    if (item.iconHighlight)
        //    {
        //        itemHighlight.Set(item.image);
        //    }
        //}

        #endregion

        #endregion
    }
}


