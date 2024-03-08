using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapScripts
{
    public class Reader : MonoBehaviour
    {
        #region fields
        [SerializeField] public Tilemap tilemap;
        private Vector3 worldPosition;
        #endregion

        public Vector3Int GetGridPosition(Tilemap tm, Vector2 pos, bool mousePos)
        {
            tilemap = tm;

            if (tilemap == null) { return Vector3Int.zero; }

            if (mousePos)
            {
                worldPosition = Camera.main.ScreenToWorldPoint(pos);
            }
            else
            {
                worldPosition = pos;
            }

            worldPosition.z = 0;

            Vector3Int gridPosition = tilemap.WorldToCell(worldPosition);
            gridPosition = new Vector3Int(gridPosition.x, gridPosition.y, 0);
            return gridPosition;
        }

        public TileBase GetTileBase(Tilemap tm, Vector3Int gridPos)
        {
            tilemap = tm;

            if (tilemap == null) { return null; }

            gridPos = tilemap.WorldToCell(new Vector3(gridPos.x, gridPos.y, 0));
            TileBase tile = tilemap.GetTile(gridPos);

            return tile;
        }
    }
}


