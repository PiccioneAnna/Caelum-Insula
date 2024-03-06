using System;
using UnityEngine;
using TMPro;
using UI;
using System.Collections;

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
}

public class Character : MonoBehaviour
{
    #region Fields

    public int level = 1;

    [Header("Stats")]
    public Stat hp;   
    public Stat energy;
    public Stat elemental;
    
    [Header ("States")]
    public bool isDead;
    public bool isExhausted;
    public bool isElementalDrained;

    [Header("Status Bars")]
    [SerializeField] StatusBar hpBar;
    [SerializeField] StatusBar energyBar;
    [SerializeField] StatusBar elementalBar;

    public TextMeshProUGUI hpValue;
    public TextMeshProUGUI energyValue;
    public TextMeshProUGUI elementalValue;

    [Header("Color Theory")]
    [SerializeField] Color damageColor = Color.red;
    private SpriteRenderer spriteRenderer;

    #endregion

    #region Runtime
    private void Start()
    {
        hp.currVal = hp.maxVal;
        energy.currVal = energy.maxVal;
        elemental.currVal = elemental.maxVal;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        UpdateHPBar();
        UpdateEnergyBar();
        UpdateElementalBar();
    }
    #endregion

    #region Update Status Bars
    private void UpdateHPBar()
    {
        hpBar.Set(hp.currVal, hp.maxVal);

        hpValue.text = FormatStatValue(hp);
        hpValue.transform.parent.gameObject.SetActive(DisplayValue(hp));
    }
    private void UpdateEnergyBar()
    {
        energyBar.Set(energy.currVal, energy.maxVal);

        energyValue.text = FormatStatValue(energy);
        energyValue.transform.parent.gameObject.SetActive(DisplayValue(energy));
    }
    private void UpdateElementalBar()
    {
        elementalBar.Set(elemental.currVal, elemental.maxVal);

        elementalValue.text = FormatStatValue(elemental);
        elementalValue.transform.parent.gameObject.SetActive(DisplayValue(elemental));
    }

    #endregion

    #region HP

    public void TakeDamage(int amount)
    {
        //Visual appearence of taking damage
        spriteRenderer.color = damageColor;
        StartCoroutine(Whitecolor());

        hp.Subtract(amount);
        if (hp.currVal <= 0)
        {
            isDead = true;
        }

        UpdateHPBar();
    }

    public void Heal(float amount)
    {
        hp.Add(amount);
        UpdateHPBar();
    }

    public void FullHeal()
    {
        hp.SetToMax();
        UpdateHPBar();
    }

    #endregion

    #region Energy

    public void GetTired(int amount)
    {
        energy.Subtract(amount);
        if (energy.currVal <= 0)
        {
            isExhausted = true;
        }
        UpdateEnergyBar();
    }

    public void Rest(float amount)
    {
        energy.Add(amount);
        if (energy.currVal >= 0)
        {
            isExhausted = false;
        }
        UpdateEnergyBar();
    }

    public void FullRest()
    {
        isExhausted = false;
        energy.SetToMax();
        UpdateEnergyBar();
    }

    #endregion

    #region Elemental Energy

    public void GetTiredElemental(int amount)
    {
        elemental.Subtract(amount);
        if (elemental.currVal <= 0)
        {
            isElementalDrained = true;
        }
        UpdateElementalBar();
    }

    public void RestElemental(float amount)
    {
        elemental.Add(amount);
        if (elemental.currVal >= 0)
        {
            isElementalDrained = false;
        }
        UpdateElementalBar();
    }

    public void FullRestElemental()
    {
        isElementalDrained = false;
        elemental.SetToMax();
        UpdateElementalBar();
    }

    #endregion

    #region Helper Methods
    IEnumerator Whitecolor()
    {
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
    }

    private String FormatStatValue(Stat stat)
    {
        return $"{(int)stat.currVal} / {stat.maxVal}";
    }

    private bool DisplayValue(Stat stat)
    {
        return stat.currVal < stat.maxVal;
    }
    #endregion

}