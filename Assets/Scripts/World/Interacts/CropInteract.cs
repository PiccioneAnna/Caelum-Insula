using Interacts;
using UnityEngine;

namespace Assets.Scripts.World.Interacts
{
    internal class CropInteract : Interactable
    {
        [HideInInspector] public CropTile cropTile;
        public GameObject child;

        public override void Interact(Player.Controller player)
        {
            child.SetActive(!child.activeSelf);

            Canvas canvas = child.GetComponentInChildren<Canvas>();

            if(canvas == null || cropTile == null) 
            { 
                Debug.Log("Canvas or cropTile null");
                return;
            }

            if (cropTile.Complete) { child.SetActive(false); }

            canvas.gameObject.GetComponentInChildren<CropUI>().UpdateCropUI(cropTile);
        }
    }
}
