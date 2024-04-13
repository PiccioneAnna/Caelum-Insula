using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapScripts
{
    public enum MarkerValidity
    {
        valid,
        invalid,
        normal
    }

    public struct MarkerTile
    {
        public MarkerTile(Vector3Int pos, MarkerValidity val, TileBase til)
        {
            position = pos;
            valid = val;
            tile = til;
        }

        public Vector3Int position;
        public MarkerValidity valid;
        public TileBase tile;
    }

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

        public Vector2 allowedBounds; // represents amount in each direction the tool has

        public List<MarkerTile> markers;
        public List<Vector3Int> multiPositions;

        [Header("Conditional States")]
        public bool isBuildMode = false;
        public bool isShow;
        public bool isMultiple;
        public bool isPlace;
        public bool isRemove;
        private bool setStart;

        public int markerCount;

        BoundsInt bounds;
        BoundsInt prevBounds;
        #endregion

        #region Runtime
        // Defaults marker to not shown
        private void Awake()
        {
            Show(isShow);
            instance = this;
            isMultiple = false;
            multiPositions = new List<Vector3Int>();
            markers = new List<MarkerTile>();
        }

        private void Start()
        {
            tileMapReader = GameManager.Instance.reader;
        }

        private void Update()
        {
            if (isShow == false) 
            {
                ClearTilemap();
                return;   
            }

            SelectTile(); 
            Marker();

            markerCount = markers.Count;

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
                DrawBounds(bounds, floorTilemap);
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
                SingleTileMarker();
            }
        }

        /// <summary>
        /// Displays a single tile marker
        /// </summary>
        public void SingleTileMarker()
        {
            ClearTilemap();

            markerTilemap.SetTile(oldCellPosition, null);
            markerTilemap.SetTile(markedCellPosition, markerTileDuplicate);
            oldCellPosition = markedCellPosition;
        }

        /// <summary>
        /// Displays multiple tile markers - should only be valid when mouse is down
        /// </summary>
        public void MultipleTileMarker()
        {
            if(Mathf.Abs(holdStartPosition.x - markedCellPosition.x) > allowedBounds.x ||
                Mathf.Abs(holdStartPosition.y - markedCellPosition.y) > allowedBounds.y)
            {
                isShow = false;
                return;
            }

            markerTilemap.SetTile(markedCellPosition, DetermineTile(markers.Find(marker => marker.position == markedCellPosition)));
            oldCellPosition = markedCellPosition;

            RectangleRenderer();        
        }
        private void SelectTile()
        {
            markedCellPosition = tileMapReader.GetGridPosition(markerTilemap, Input.mousePosition, true);
        }

        public void RefreshMarkers()
        {
            foreach (MarkerTile marker in markers)
            {
                markerTilemap.SetTile(marker.position, DetermineTile(marker));
                Debug.Log(marker.valid);
            }
        }

        public void RefreshMarker(MarkerTile marker)
        {
            markerTilemap.SetTile(marker.position, DetermineTile(marker));
        }

        private TileBase DetermineTile(MarkerTile marker)
        {
            TileBase tb = markerTileDuplicate;

            switch (marker.valid)
            {
                case 0: tb = markerTileValid; break;
                case (MarkerValidity)1: tb = markerTileInValid; break;
                case (MarkerValidity)2: tb = markerTileDuplicate; break;
            }

            return tb;
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
            bounds.xMin = markedCellPosition.x < holdStartPosition.x ? markedCellPosition.x : holdStartPosition.x;
            bounds.xMax = markedCellPosition.x > holdStartPosition.x ? markedCellPosition.x : holdStartPosition.x;
            bounds.yMin = markedCellPosition.y < holdStartPosition.y ? markedCellPosition.y : holdStartPosition.y;
            bounds.yMax = markedCellPosition.y > holdStartPosition.y ? markedCellPosition.y : holdStartPosition.y;

            DrawBounds(bounds, markerTilemap);

            prevBounds = bounds;
        }

        private void DrawBounds(BoundsInt b, Tilemap target)
        {
            if (prevBounds == b) return;

            ClearTilemap();

            Vector3Int tempPos;

            // Draws bounds on given map
            for (int x = b.xMin; x <= b.xMax; x++)
            {
                for (int y = b.yMin; y <= b.yMax; y++)
                {
                    tempPos = new Vector3Int(x, y, 0);
                    MarkerTile marker;

                    if (markers.Any(marker => marker.position == tempPos))
                    {
                        marker = markers.Find(marker => marker.position == tempPos);
                    }
                    else
                    {
                        marker = new MarkerTile(tempPos, MarkerValidity.normal, markerTileDuplicate);
                        marker.valid = GameManager.Instance.player.VisualizeToolTile(marker);

                        RefreshMarker(marker);

                        markers.Add(marker);
                    }
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
                    Vector3Int tempPos = new Vector3Int(x, y, 0);

                    target.SetTile(tempPos, null);

                    markers.Remove(markers.Find(marker => marker.position == tempPos));
                }
            }
        }
        #endregion

        #region Tilemap Behaviour
        public void ClearTilemap()
        {
            markerTilemap.ClearAllTiles();
            markers.Clear();
        }
        #endregion
    }

}

