using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneEnum
{
    [Description("Fire")]
    Fire,
    [Description("Fire-Water")]
    FireWater,
    [Description("Water")]
    Water,
    [Description("Fire-Air")]
    FireAir,
    [Description("Center")]
    Center,
    [Description("Earth-Water")]
    EarthWater,
    [Description("Air")]
    Air,
    [Description("Earth-Air")]
    AirEarth,
    [Description("Earth")]
    Earth,
    [Description("Home")]
    Home,
    [Description("Other")]
    Other,
    [Description("Random Dungeon")]
    RandomDungeon,
    [Description("Random Island")]
    RandomIsland,
    [Description("Null")]
    Null
}

/// <summary>
/// This class contains reference to scene specific information
/// </summary>
public class SceneInformation : MonoBehaviour
{
    [Header("Camera Properties")]
    public PolygonCollider2D cameraCollider;

    [Header("Surrounding Scenes")]
    [SerializeField] public SceneEnum current;
    [SerializeField] public SceneEnum right;
    [SerializeField] public SceneEnum left;
    [SerializeField] public SceneEnum top;
    [SerializeField] public SceneEnum bottom;

}


