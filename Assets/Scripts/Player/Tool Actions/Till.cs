using Data;
using System.Collections;
using System.Collections.Generic;
using TilemapScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ToolActions
{
    [CreateAssetMenu(menuName = "Data/Tool Action/Till")]
    public class Till : Base
    {
        [SerializeField] List<TileBase> canTill;

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

            TillCheckTilemap();

            return success;
        }

        private void TillCheckTilemap()
        {
            foreach (Tilemap tilemap in tilemapInfo.grassTileMaps)
            {
                TileBase tileToTill = reader.GetTileBase(tilemap, gridPos);

                if (!canTill.Contains(tileToTill))
                {
                    success = false;
                }
                else
                {
                    success = true;
                    CheckBelowTile();
                    GameManager.Instance.cropsManager.Till(gridPos, tilemap);
                    return;
                }
            }
        }

        private void CheckBelowTile()
        {
            Tilemap current = null;

            foreach (Tilemap tilemap in tilemapInfo.dirtTilemaps)
            {
                current = tilemap;
                TileBase tileToTill = reader.GetTileBase(tilemap, gridPos);

                if (tileToTill != null) { return; }
            }

            if (tilemapInfo.dirt == null) 
            { 
                Debug.Log("Dirt tile not set in info");
                return;
            }

            GameManager.Instance.cropsManager.ReplaceTile(gridPos, current, tilemapInfo.dirt);
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


