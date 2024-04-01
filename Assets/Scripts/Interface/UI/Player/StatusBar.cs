using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Stat
{
    public int maxVal;
    public float currVal;

    public Stat(int curr, int max)
    {
        maxVal = max;
        currVal = curr;
    }

    internal void Subtract(int amount)
    {
        currVal -= amount;
        Mathf.Clamp(currVal, -.5f, maxVal);
    }

    internal void Add(float amount)
    {
        currVal += amount;
        Mathf.Clamp(currVal, -.5f, maxVal);
    }

    internal void SetToMax()
    {
        currVal = maxVal;
    }

    internal float ReturnPercentage()
    {
        return (int)((currVal / maxVal) * 100);
    }
}

[Serializable]
public class PersonalityStat
{
    public int minVal;
    public int maxVal;
    public float currVal;

    public PersonalityStat(int curr, int max, int min)
    {
        minVal = min;
        maxVal = max;
        currVal = curr;
    }

    internal void Set(int val)
    {
        currVal = val;
        Mathf.Clamp(currVal, minVal, maxVal);
    }
}

namespace UI
{
    public class StatusBar : MonoBehaviour
    {
        [SerializeField] Slider bar;

        public void Set(float curr, int max)
        {
            bar.maxValue = max;
            bar.value = curr;
        }
    }
}

