using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapGenerator 
{
    // Converts 2d array returned from cellular automata to a tilemap
    public static List<Vector3Int> PopulateTilemap(Tilemap tilemap, int[,] data, TileBase tile, bool clear = true)
    {
        List<Vector3Int> positions = new();

        // returns if the tilemap or the data is null
        if (tilemap == null || data == null)
        {
            return positions;
        }

        if (clear == true) { tilemap.ClearAllTiles(); }

        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                if (data[x, y] == 1)
                {
                    Vector3Int p = new Vector3Int(x, y, 0);
                    tilemap.SetTile(p, tile);
                    positions.Add(p);
                }
            }
        }

        return positions;
    }

    public static int[,] DefineWallPlacement(int[,] refArray)
    {
        int[,] caBuffer = new int[refArray.GetLength(0), refArray.GetLength(1)];

        for (int x = 0; x < caBuffer.GetLength(0); x++)
        {
            for (int y = 0; y < caBuffer.GetLength(1); y++)
            {
                if (refArray[x, y] == 1) {

                    if (((y - 1) >= 0) && ((y - 1) < refArray.GetLength(1))) // 1 up & 1 down within range
                    {
                        if ((refArray[x, y - 1] == 0))
                        {
                            caBuffer[x, y] = 1;
                        }
                    }
                    
                    if (((y + 1) > 0) && ((y + 1) < refArray.GetLength(1))) // 1 up & 1 down within range
                    {
                        if ((refArray[x, y + 1] == 0))
                        {
                            caBuffer[x, y] = 1;
                        }
                    }

                    if (caBuffer[x,y] != 1) { caBuffer[x,y] = 0; }
                }
                else
                {
                    caBuffer[x, y] = 0;
                }
            }
        }

        return caBuffer;
    }

    public static BoundsInt ReturnTilemapInfo(Tilemap tm)
    {
        tm.CompressBounds();
        return tm.cellBounds;
    }

    public static int GetTilemapTileCount(Tilemap tm)
    {
        tm.CompressBounds();
        int count = 0;

        foreach (var pos in tm.cellBounds.allPositionsWithin)
        {
            if (pos != null) { count++; }
        }

        return count;
    }

    public static Vector3Int GetRandomPos(List<Vector3Int> tilesPos)
    {
        return tilesPos != null ? tilesPos[Random.Range(0, tilesPos.Count)] : Vector3Int.zero;
    }
}

