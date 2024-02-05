using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

/// <summary>
/// This class serves as a base class for ALL in game mobs
/// </summary>
public class Creature : BreakableObject
{
    #region Fields
    // Reference to a scriptable object that contains creature information
    public CreatureInformation creatureInformation;
    public Stat health;

    #region UI References
    public StatusBar hpBarExternal;




    #endregion

    #endregion

    #region Constructor

    public Creature()
    {

    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        health.SetToMax();

        UpdateCreatureUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates all Creature UI Elements with relevant data
    /// </summary>
    protected void UpdateCreatureUI()
    {
        hpBarExternal.Set(health.currVal, health.maxVal);
    }
}
