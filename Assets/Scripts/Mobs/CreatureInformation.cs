using Data;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    Fire,
    Water,
    Earth,
    Air
}

[CreateAssetMenu(menuName = "Data/Creature")]
public class CreatureInformation : ScriptableObject
{
    [Header("General")]
    public string _name; // name of the creature
    public int _index; // number in creature index



    [Header("Drops")]
    public List<Item> _drops;


}
 