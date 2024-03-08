using Data;
using System.Collections.Generic;
using TilemapScripts;
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

            TillCheckTilemap(true);

            return success;
        }

        public override bool VisualizeOnApplyToTileMap(Vector3Int gridPosition, Reader tilemapReadController, Item item)
        {
            tilemapInfo = GameManager.Instance.tilemapInfoManager;
            reader = tilemapReadController;
            gridPos = gridPosition;

            if (NullCheck()) return false;

            TillCheckTilemap(false);

            return success;
        }

        private void TillCheckTilemap(bool apply)
        {
            foreach (Tilemap tilemap in tilemapInfo.grassTileMaps)
            {
                tilemap.GetComponent<TilemapRenderer>().mode = TilemapRenderer.Mode.Individual;

                TileBase tileToTill = reader.GetTileBase(tilemap, gridPos);

                Debug.Log(gridPos);

                if (!canTill.Contains(tileToTill))
                {
                    success = false;
                }
                else
                { 
                    //CheckBelowTile();
                    if (apply)
                    {
                        GameManager.Instance.cropsManager.Till(gridPos, tilemap);
                        success = true;
                        return;
                    }
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

        private bool NullCheck()
        {
            if (tilemapInfo == null)
            {
                Debug.Log("Tilemap info not found");
                return true;
            }

            return false;
        }
    }
}


