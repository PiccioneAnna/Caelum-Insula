using System.Collections.Generic;
using UnityEngine;
using Data;

[CreateAssetMenu(menuName = "Data/Crop")]
public class Crop : ScriptableObject
{
    public int timeToGrow = 10;
    public Item yield;
    public Item seeds;
    public int count = 1;
    public bool multiHarvest = false;

    public List<Sprite> sprites;
    public List<int> growthStageTime;
}
