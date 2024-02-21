using Data;
using System.Collections.Generic;
using TilemapScripts;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ToolActions
{
    [CreateAssetMenu(menuName = "Data/Tool Action/Hoe")]
    public class Hoe : Base
    {
        [SerializeField] List<TileBase> canHoe;

        private Reader reader;
        private TilemapInfoManager tilemapInfo;
        private Vector3Int gridPos;

        private bool success;

        public override bool OnApplyToTileMap(Vector3Int gridPosition, TilemapScripts.Reader tilemapReadController, Item item)
        {
            tilemapInfo = GameManager.Instance.tilemapInfoManager;
            reader = tilemapReadController;
            gridPos = gridPosition;

            NullCheck();

            HoeCheckTilemaps();

            return success;
        }

        private void HoeCheckTilemaps()
        {
            foreach (Tilemap tilemap in tilemapInfo.dirtTilemaps)
            {
                TileBase tileToPlow = reader.GetTileBase(tilemap, gridPos);

                if (!canHoe.Contains(tileToPlow))
                {
                    success = false;
                }
                else
                {
                    success = true;
                    GameManager.Instance.cropsManager.Plow(gridPos);
                    return;
                }
            }
        }

        private void NullCheck()
        {
            if (tilemapInfo == null)
            {
                Debug.Log("Tilemap info not found");
                success = false;
            }
        }
    }
}

