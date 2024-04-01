using TMPro;
using UI;
using UnityEngine;

public class CropUI : MonoBehaviour
{
    public StatusBar health;
    public StatusBar growth;

    public TMP_Text healthPercent;
    public TMP_Text growthPercent;

    public void UpdateCropUI(CropTile crop)
    {
        growth.Set(crop.growTimer, crop.crop.timeToGrow);
        health.Set(crop.health.currVal, crop.health.maxVal);

        healthPercent.text = crop.health.ReturnPercentage().ToString() + " %";
        growthPercent.text = crop.growth.ReturnPercentage().ToString() + " %";
    }
}
