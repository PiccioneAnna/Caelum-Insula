using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public enum TransitionType
{
    Warp,
    Scene,
    Dungeon
}

public class Transition : MonoBehaviour
{
    [SerializeField] TransitionType transitionType;
    [SerializeField] SceneEnum sceneNameToTransition;
    [SerializeField] Vector3 offsetPosition;

    [SerializeField] List<GameObject> transparentDependecies;

    public int newSortLayer;

    private Vector3 targetPosition;
    private Vector3 playerPosition;

    Transform destination;

    // Start is called before the first frame update
    void Start()
    {
        destination = transform;
    }

    /// <summary>
    /// Method scts when the player triggers the border collision
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (sceneNameToTransition == SceneEnum.Null) { return; }

            playerPosition = collision.transform.position;
            InitiateTransition(collision.transform);
        }
    }

    internal void InitiateTransition(Transform toTransition)
    {
        switch (transitionType)
        {
            case TransitionType.Warp:
                CalculateTransitionDistance();
                ApplyNewSortOrder();
                playerPosition = targetPosition;
                break;
            case TransitionType.Scene:
                CalculateTransitionDistance();
                GameManager.Instance.sceneManager.InitSwitchScene(EnumHelper.GetDescription(sceneNameToTransition), targetPosition);
                break;
            case TransitionType.Dungeon:
                FindSpawnArea();
                GameManager.Instance.sceneManager.InitSwitchScene(EnumHelper.GetDescription(sceneNameToTransition), targetPosition);
                break;
        }
    }

    private void CalculateTransitionDistance()
    {
        targetPosition = playerPosition + (offsetPosition * 5f);
    }

    private void ApplyNewSortOrder()
    {
        GameManager.Instance.player.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = newSortLayer;

        foreach (GameObject o in transparentDependecies)
        {
            o.SetActive(!o.activeSelf);
        }
    }

    private void FindSpawnArea()
    {
        targetPosition = new Vector3(50, 50, 0);
    }
}

/// <summary>
/// https://blog.hildenco.com/2018/07/getting-enum-descriptions-using-c.html
/// </summary>
public static class EnumHelper
{
    public static string GetDescription<T>(this T enumValue)
        where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum)
            return null;

        var description = enumValue.ToString();
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        if (fieldInfo != null)
        {
            var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                description = ((DescriptionAttribute)attrs[0]).Description;
            }
        }

        return description;
    }
}
