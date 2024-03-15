using Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting
{
    [Serializable]
    public class CraftableItem : MonoBehaviour
    {
        [Header("UI")]
        [HideInInspector] public Image image;
        [HideInInspector] public Text countText;

        public Item item;
        public int count = 1;
        [HideInInspector] public Transform parentAfterDrag;

        public void InitialiseItem(Item newItem)
        {
            item = newItem;
            Debug.Log(newItem);
            image.sprite = newItem.image;
            RefreshCount();
        }

        public void RefreshCount()
        {
            countText.text = count.ToString();
            bool textActive = count > 1;
            countText.gameObject.SetActive(textActive);
        }
    }
}


