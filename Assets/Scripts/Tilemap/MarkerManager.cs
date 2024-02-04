using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapScripts
{
    public class MarkerManager : MonoBehaviour
    {
        #region Fields
        public static MarkerManager instance;

        [Header("Dependencies")]
        [SerializeField] Reader tileMapReader;

        [Header("Tilemaps")]
        [SerializeField] Tilemap markerTilemap;
        [SerializeField] Tilemap floorTilemap;
        [SerializeField] Tilemap wallTilemap;
        [SerializeField] Tilemap borderTilemap;

        [Header("Tiles")]
        [SerializeField] TileBase markerTileValid; // green - represents tile positions that can be placed
        [SerializeField] TileBase markerTileInValid; // red - represents tiles that can't be placed
        [SerializeField] TileBase markerTileDuplicate; // white - represents tile that already exit
        [SerializeField] TileBase tempTile;

        [Header("Positions")]
        public Vector3Int markedCellPosition;
        public Vector3Int holdStartPosition;
        Vector3Int oldCellPosition;

        [Header("Conditional States")]
        public bool isBuildMode = false;
        public bool isShow;
        public bool isMultiple;
        public bool isPlace;
        public bool isRemove;
        private bool setStart;

        BoundsInt bounds;
        #endregion

        #region Runtime
        // Defaults marker to not shown
        private void Awake()
        {
            Show(isShow);
            instance = this;
            isMultiple = false;
        }

        private void Start()
        {
            tileMapReader = GameManager.Instance.reader;
        }

        private void Update()
        {
            if (isShow == false) { return; }

            SelectTile();
            Marker();

            if (isBuildMode) { ExpandFloor(); }
        }
        #endregion

        #region Floor Expansion
        public void ExpandFloor()
        {
            isRemove = Input.GetKey(KeyCode.LeftShift) == true;
            isPlace = Input.GetMouseButtonUp(0) == true;
            if (isPlace && !isRemove)
            {
                DrawBounds(bounds, floorTilemap, tempTile);
            }
            else if (isPlace && isRemove)
            {
                RemoveBounds(bounds, floorTilemap);
            }
        }
        #endregion

        #region Marker
        public void Marker()
        {
            isMultiple = Input.GetMouseButton(0) == true;

            if (isMultiple)
            {
                if (!setStart)
                {
                    holdStartPosition = markedCellPosition;
                    setStart = true;
                }
                MultipleTileMarker();
            }
            else
            {
                setStart = false;
                ClearTilemap(markerTilemap);
                SingleTileMarker();
            }
        }

        /// <summary>
        /// Displays a single tile marker
        /// </summary>
        public void SingleTileMarker()
        {
            markerTilemap.SetTile(oldCellPosition, null);
            markerTilemap.SetTile(markedCellPosition, markerTileValid);
            oldCellPosition = markedCellPosition;
        }

        /// <summary>
        /// Displays multiple tile markers - should only be valid when mouse is down
        /// </summary>
        public void MultipleTileMarker()
        {
            markerTilemap.SetTile(markedCellPosition, markerTileValid);
            oldCellPosition = markedCellPosition;

            RectangleRenderer();
        }
        private void SelectTile()
        {
            markedCellPosition = tileMapReader.GetGridPosition(markerTilemap, Input.mousePosition, true);
        }

        public void Show(bool selectable)
        {
            isShow = selectable;
            markerTilemap.gameObject.SetActive(isShow);
        }
        #endregion

        #region Shapes & Bounds
        private void RectangleRenderer()
        {
            markerTilemap.ClearAllTiles();

            bounds.xMin = markedCellPosition.x < holdStartPosition.x ? markedCellPosition.x : holdStartPosition.x;
            bounds.xMax = markedCellPosition.x > holdStartPosition.x ? markedCellPosition.x : holdStartPosition.x;
            bounds.yMin = markedCellPosition.y < holdStartPosition.y ? markedCellPosition.y : holdStartPosition.y;
            bounds.yMax = markedCellPosition.y > holdStartPosition.y ? markedCellPosition.y : holdStartPosition.y;

            DrawBounds(bounds, markerTilemap, markerTileValid);
        }

        private void DrawBounds(BoundsInt b, Tilemap target, TileBase tile)
        {
            // Draws bounds on given map
            for (int x = b.xMin; x <= b.xMax; x++)
            {
                for (int y = b.yMin; y <= b.yMax; y++)
                {
                    target.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

        private void RemoveBounds(BoundsInt b, Tilemap target)
        {
            // Draws bounds on given map
            for (int x = b.xMin; x <= b.xMax; x++)
            {
                for (int y = b.yMin; y <= b.yMax; y++)
                {
                    target.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }
        #endregion

        #region Tilemap Behaviour
        public void ClearTilemap(Tilemap target)
        {
            target.ClearAllTiles();
        }
        #endregion
    }

}

