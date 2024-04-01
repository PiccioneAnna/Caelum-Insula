using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureUIManager : MonoBehaviour
{
    #region Fields

    [Header("UI References")]
    public GameObject infoPopupContainer;


    [Header("Data References")]
    public CreatureInformation creature;

    #endregion







    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Update creature info w./ updated scriptable object
    /// </summary>
    /// <param name="creature"></param>
    public void SetCreatureInfo(CreatureInformation creature)
    {
        this.creature = creature;
    }

    #region Populating UI w./ Data
    public void UpdateAllUI()
    {

    }
    public void UpdateExternalUI()
    {

    }

    /// <summary>
    /// Helper methods for chunking data changing
    /// </summary>
    public void UpdatePersonality()
    {

    }
    public void UpdateTraits()
    {

    }
    public void UpdateTaskList() 
    { 
    
    }
    #endregion
}
