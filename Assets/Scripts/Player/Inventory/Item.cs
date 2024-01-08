using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class ItemHighlight
    {

    }


    public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("UI")]
        [HideInInspector] public Image image;
        [HideInInspector] public Text countText;

        public Data.Item item;
        public int count = 1;
        [HideInInspector] public Transform parentAfterDrag;

        public void InitialiseItem(Data.Item newItem)
        {
            item = newItem;
            Debug.Log(newItem);
            image = newItem.image;
            RefreshCount();
        }

        public void RefreshCount()
        {
            countText.text = count.ToString();
            bool textActive = count > 1;
            countText.gameObject.SetActive(textActive);
        }

        #region Drag & Drop
        public void OnBeginDrag(PointerEventData eventData)
        {
            image.raycastTarget = false;
            countText.raycastTarget = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.parent.parent.parent.parent);
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            countText.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
        }
        #endregion
    }
}

