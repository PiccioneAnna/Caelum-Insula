using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

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
                Debug.Log(refArray.GetLength(1));

                if ((refArray[x,y] == 1 && refArray.GetLength(1) > y-1 && refArray[y - 1,x] == 0) ||
                        (refArray[x,y] == 1 && refArray.GetLength(1) < y + 1 && refArray[y + 1, x] == 0))
                {
                    caBuffer[x,y] = 1;
                }
                else
                {
                    caBuffer[x, y] = 0;
                }
            }
        }

        return caBuffer;
    }

    public static Vector3Int GetRandomPos(List<Vector3Int> tilesPos)
    {
        return tilesPos != null ? tilesPos[Random.Range(0, tilesPos.Count)] : Vector3Int.zero;
    }
}

