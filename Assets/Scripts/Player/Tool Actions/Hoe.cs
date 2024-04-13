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
        private TilemapScripts.CropsManager cropsManager;
        private Vector3Int gridPos;

        private bool success;

        public override bool OnApplyToTileMap(Vector3Int gridPosition, TilemapScripts.Reader tilemapReadController, Item item)
        {
            tilemapInfo = GameManager.Instance.tilemapInfoManager;
            cropsManager = GameManager.Instance.cropsManager;
            reader = tilemapReadController;
            gridPos = gridPosition;

            NullCheck();

            HoeCheckTilemaps(true);

            return success;
        }

        public override bool VisualizeOnApplyToTileMap(Vector3Int gridPosition, Reader tilemapReadController, Item item)
        {
            tilemapInfo = GameManager.Instance.tilemapInfoManager;
            reader = tilemapReadController;
            gridPos = gridPosition;

            NullCheck();

            HoeCheckTilemaps(false);

            return success;
        }

        private void HoeCheckTilemaps(bool apply)
        {
            foreach (Tilemap dirtMap in tilemapInfo.dirtTilemaps)
            {
                success = true;

                dirtMap.GetComponent<TilemapRenderer>().mode = TilemapRenderer.Mode.Individual;

                TileBase tileToPlow = reader.GetTileBase(dirtMap, gridPos);

                if (!canHoe.Contains(tileToPlow))
                {
                    success = false;
                }
                else
                {
                    foreach(Tilemap grassMap in tilemapInfo.grassTileMaps)
                    {
                        if (reader.GetTileBase(grassMap, gridPos) != null)
                        {
                            success = false;
                        }
                    }

                    if (apply && success)
                    {
                        cropsManager.Plow(cropsManager.targetTilemap, gridPos);
                        return;
                    }                 
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

