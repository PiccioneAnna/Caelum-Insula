using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class Slot : MonoBehaviour, IDropHandler
    {
        public Inventory.Item item;
        public Image image;
        public Color selectedColor, notSelectedColor;
        public bool craftingSlot = false;

        private void Awake()
        {
            Deselect();

            if (craftingSlot == false)
            {
                GetComponent<Button>().interactable = false;
            }
        }

        #region Selection
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
            item = GetComponentInChildren<Item>();
            Destroy(item.gameObject);
        }
        #endregion

        #region Drag & Drop
        public void OnDrop(PointerEventData eventData)
        {       
            if (!eventData.pointerDrag.TryGetComponent(out item)) { return; }

            // If there isnt an object then set the item's parent to the slot dropped on
            if (transform.childCount == 0)
            {
                Debug.Log(item);
                item.parentAfterDrag = transform;
            }

            // Otherwise swap positions between the items
            else
            {
                Inventory.Item currentSlotItem = transform.GetComponentInChildren<Inventory.Item>();
                currentSlotItem.gameObject.transform.SetParent(item.parentAfterDrag);

                item.parentAfterDrag.GetComponent<Slot>().item = currentSlotItem;

                item.parentAfterDrag = transform;
                item.parentAfterDrag.GetComponent<Slot>().item = item;
            }
        }
        #endregion
    }
}

