using Data;
using InGameTasks;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#region Helper Enums & Mini Classes

    /// <summary>
    /// Default 4 primary elements : fire, water, earth, air
    /// </summary>
    public enum Element
    {
        Fire,
        Water,
        Earth,
        Air
    }

    /// <summary>
    /// Default class for a trait
    /// </summary>
    public class Trait 
    {
        public int _name;
        public int _summary;
        public int _heridityChance;

        public virtual void ApplyTrait() { }
    }

    /// <summary>
    /// Default class for a personality trait value
    /// </summary>
    public class PersonalityTraitValue
    {
        public int _min;
        public int _max;
        public int _curr;

        public virtual void ApplyTrait() { }
    }

    /// <summary>
    /// Default class for a creature relationship
    /// </summary>
    public class CreatureRelationship
    {
        public string _relationshipTitle;
    }

#endregion

[CreateAssetMenu(menuName = "Data/Creature")]
// Class represents default level 1 creature without any mods
public class CreatureInformation : ScriptableObject
{
    [Header("General")]
    public string _name; // name of the creature
    public int _index; // number in creature index
    public int _level;

    [Header("Personality")]
    public PersonalityStat _openness;
    public PersonalityStat _conscientiousness;
    public PersonalityStat _extraversion;
    public PersonalityStat _agreeableness;
    public PersonalityStat _neuroticism;

    public List<PersonalityTraitValue> _traits;

    [Header("Stats")]
    public Stat _health;
    public Stat _stamina;
    public Stat _food;
    public Stat _mood;

    public int _attack;
    public int _defense;
    public int _speed;

    public List<Element> _elements;

    [Header("Abilities")]
    public TaskDictionary _tasksInfo;

    [Header("Social")]
    public List<CreatureRelationship> _relationships;

    [Header("Drops")]
    public List<Item> _drops;

}

// Class handles relationship between creature data and creature ui
public class CreatureInfoManager
{
    public CreatureInformation defaultInfo;

    [Header("General")]
    public string _name; // name of the creature
    public int _index; // number in creature index
    public int _level;

    [Header("Personality")]
    public PersonalityStat _openness;
    public PersonalityStat _conscientiousness;
    public PersonalityStat _extraversion;
    public PersonalityStat _agreeableness;
    public PersonalityStat _neuroticism;

    public List<PersonalityTraitValue> _traits;

    [Header("Stats")]
    public Stat _health;
    public Stat _stamina;
    public Stat _food;
    public Stat _mood;

    public int _attack;
    public int _defense;
    public int _speed;

    public List<Element> _elements;

    [Header("Abilities")]
    public TaskDictionary _tasksInfo;
    public List<TaskInfo> _tasksList;

    [Header("Social")]
    public List<CreatureRelationship> _relationships;

    [Header("Drops")]
    public List<Item> _drops;

    public void PopulateCreature()
    {

    }

    public void PopulateUI()
    {

    }
}
 