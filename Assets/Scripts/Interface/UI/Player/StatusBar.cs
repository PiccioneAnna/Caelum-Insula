using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

