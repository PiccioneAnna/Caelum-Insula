using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

namespace Inventory
{
    public class Slot : MonoBehaviour, IDropHandler
    {
        public Inventory.Item item;
        public Image image;
        public Color selectedColor, notSelectedColor;

        private void Awake()
        {
            Deselect();
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
            item = eventData.pointerDrag.GetComponent<Inventory.Item>();

            if (item == null) { return; }

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

