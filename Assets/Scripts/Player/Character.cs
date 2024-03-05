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
    public int level = 1;

    [Header("Stats")]
    public Stat hp;   
    public Stat energy;
    
    [Header ("States")]
    public bool isDead;
    public bool isExhausted;

    [Header("Status Bars")]
    [SerializeField] StatusBar hpBar;
    [SerializeField] StatusBar energyBar;

    [Header("Color Theory")]
    [SerializeField] Color damageColor = Color.red;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        hp.currVal = hp.maxVal;
        energy.currVal = energy.maxVal;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        UpdateHPBar();
        UpdateEnergyBar();
    }

    #region Update Status Bars
    private void UpdateHPBar()
    {
        hpBar.Set(hp.currVal, hp.maxVal);
    }
    private void UpdateEnergyBar()
    {
        energyBar.Set(energy.currVal, energy.maxVal);
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

    #region Helper Methods
    IEnumerator Whitecolor()
    {
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.white;
    }
    #endregion

}