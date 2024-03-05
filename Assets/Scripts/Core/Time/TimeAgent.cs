using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAgent : MonoBehaviour
{
    public Action onTimeTick;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        GameManager.Instance.timeController.Subscribe(this);
    }

    public void UnSubscribe()
    {
        GameManager.Instance.timeController.UnSubscribe(this);
    }

    public void Invoke()
    {
        onTimeTick?.Invoke();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }
}
