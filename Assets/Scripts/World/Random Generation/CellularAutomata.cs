using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#region resources and credit
// resources and code help/inspiration
// 1.) https://bronsonzgeb.com/index.php/2022/01/30/procedural-generation-with-cellular-automata/
#endregion
public class CellularAutomata : MonoBehaviour
{
    #region fields
    private int[,] _cellularAutomata;
    private Tilemap _refTilemap;

    private int _width;
    private int _height;
    private float _fillPercent;
    private int _liveNeighboursRequired;
    private int _stepCount;
    private float _additionalSpawnChance = 0f;

    public int _filledCells;
    #endregion

    public void Set(int width, int height, float fillPercent, 
        int liveNeighbors, int step, float addSpawn, Tilemap refTm = null)
    {
        _width = width;
        _height = height;
        _fillPercent = fillPercent;
        _liveNeighboursRequired = liveNeighbors;
        _stepCount = step;
        _additionalSpawnChance = addSpawn;
        _refTilemap = refTm;
    }

    public int[,] GenerateMap(int[,] previousArray)
    {
        ResetAutomata();

        for (int i = 0; i < _stepCount; i++)
        {
            Step();
        }

        // Add lower map constraints
        ApplyPreviousArrayConstraints(previousArray);

        return _cellularAutomata;
    }

    public int[,] UpdateMap(int stepCount)
    {
        for (int i = 0; i < stepCount; i++)
        {
            Step();
        }

        return _cellularAutomata;
    }

    #region Cellular Automata
    void ResetAutomata()
    {
        _cellularAutomata = new int[_width, _height];
        for (int x = 0; x < _width; ++x)
        {
            for (int y = 0; y < _height; ++y)
            {
                _cellularAutomata[x, y] = Random.value > _fillPercent ? 0 : 1;
            }
        }
    }

    // check to make sure neighbors are inside of the bounds
    // add up the cells and return the result
    int GetNeighbourCellCount(int x, int y)
    {
        int neighbourCellCount = 0;
        if (x > 0)
        {
            neighbourCellCount += _cellularAutomata[x - 1, y];
            if (y > 0)
            {
                neighbourCellCount += _cellularAutomata[x - 1, y - 1];
            }
        }

        if (y > 0)
        {
            neighbourCellCount += _cellularAutomata[x, y - 1];
            if (x < _width - 1)
            {
                neighbourCellCount += _cellularAutomata[x + 1, y - 1];
            }
        }

        if (x < _width - 1)
        {
            neighbourCellCount += _cellularAutomata[x + 1, y];
            if (y < _height - 1)
            {
                neighbourCellCount += _cellularAutomata[x + 1, y + 1];
            }
        }

        if (y < _height - 1)
        {
            neighbourCellCount += _cellularAutomata[x, y + 1];
            if (x > 0)
            {
                neighbourCellCount += _cellularAutomata[x - 1, y + 1];
            }
        }

        return neighbourCellCount;
    }

    // iterates through every cell -> if requirement met then cell lives otherwise it dies
    void Step()
    {
        int[,] caBuffer = new int[_width, _height];
        int i;
        int liveCellCount;

        _filledCells = 0;

        for (int x = 0; x < _width; ++x)
        {
            for (int y = 0; y < _height; ++y)
            {
                liveCellCount = _cellularAutomata[x, y] + GetNeighbourCellCount(x, y);
                i = liveCellCount > _liveNeighboursRequired ? 1 : 0;

                if (i == 1)
                {
                    // Boost to additional neighbors, greater chance of spawning
                    if(liveCellCount - _liveNeighboursRequired > 0)
                    {
                        _additionalSpawnChance *= (liveCellCount - _liveNeighboursRequired);
                    }

                    i = Random.value > _additionalSpawnChance ? 0 : 1;

                    if(i == 1)
                    {
                        _filledCells++;
                    }
                }

                caBuffer[x, y] = i;
            }
        }

        for (int x = 0; x < _width; ++x)
        {
            for (int y = 0; y < _height; ++y)
            {
                _cellularAutomata[x, y] = caBuffer[x, y];
            }
        }
    }

    void ApplyPreviousArrayConstraints(int[,] constraintArray)
    {
        if (constraintArray == null) { return; } // returns if no constraint map

        for (int x = 0; x < _width; ++x)
        {
            for (int y = 0; y < _height; ++y)
            {
                // checks to see if there is a supporting tile on the map below, doesn't spawn if nothing below
                _cellularAutomata[x, y] = constraintArray[x, y] == 0 ? 0 : _cellularAutomata[x, y];
            }
        }
    }
    #endregion
}
