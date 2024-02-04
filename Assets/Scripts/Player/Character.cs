using System;
using UnityEngine;
using TMPro;
using UI;

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
    }

    internal void Add(float amount)
    {
        currVal += amount;

        if (currVal > maxVal) { currVal = maxVal; }
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

    private void Start()
    {
        hp.currVal = hp.maxVal;
        energy.currVal = energy.maxVal;

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
        hp.Subtract(amount);
        if (hp.currVal <= 0)
        {
            isDead = true;
        }
        UpdateHPBar();
    }

    public void Heal(int amount)
    {
        hp.Add(amount);
    }

    public void FullHeal()
    {
        hp.SetToMax();
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
        UpdateEnergyBar();
    }

    public void FullRest()
    {
        energy.SetToMax();
        UpdateEnergyBar();
    }

    #endregion

}