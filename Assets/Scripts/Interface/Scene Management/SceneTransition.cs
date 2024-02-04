using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TransitionType
{
    Warp,
    Scene
}

public class Transition : MonoBehaviour
{
    [SerializeField] TransitionType transitionType;
    [SerializeField] SceneEnum sceneNameToTransition;
    [SerializeField] Vector3 offsetPosition;
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
            CalculateTransitionDistance();
            InitiateTransition(collision.transform);
        }
    }

    internal void InitiateTransition(Transform toTransition)
    {
        switch (transitionType)
        {
            case TransitionType.Warp:
                Cinemachine.CinemachineBrain currentCamera = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();

                currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(
                    toTransition,
                    destination.position - toTransition.position
                    );

                toTransition.position = new Vector3(
                    destination.position.x,
                    destination.position.y,
                    toTransition.position.z
                    );
                break;
            case TransitionType.Scene:
                GameManager.Instance.sceneManager.InitSwitchScene(EnumHelper.GetDescription(sceneNameToTransition), targetPosition);
                break;
        }
    }

    private void CalculateTransitionDistance()
    {
        targetPosition = playerPosition + (offsetPosition * 5f);
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
