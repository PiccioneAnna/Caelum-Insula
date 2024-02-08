using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

/// <summary>
/// This class serves as a base class for ALL in game mobs
/// </summary>
[RequireComponent(typeof(Damageable))]
public class Creature : MonoBehaviour, IDamageable
{
    #region Fields
    // Reference to a scriptable object that contains creature information
    public CreatureInformation creatureInformation;
    public Stat health;

    Transform player;
    [SerializeField] float speed;
    [SerializeField] Vector2 attackSize = Vector2.one;
    [SerializeField] int damage = 5;
    [SerializeField] float timeToAttack = 2f;
    float attackTimer;

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

        player = GameManager.Instance.player.transform;
        attackTimer = Random.Range(0, timeToAttack);
    }

    // Update is called once per frame
    void Update()
    {
        // cuts out z so it stays 0 for rendering purposes
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
            );

        Attack();
    }

    #region Base Methods from IDamageable
    public void ApplyDamage(float damage)
    {
        health.currVal -= damage;
        UpdateCreatureUI();
    }

    public void CalculateDamage(ref float damage)
    {
        damage /= 2;
    }

    public void CheckState()
    {
        if (health.currVal <= 0)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Attacks
    private void Attack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer > 0f) { return; }

        attackTimer = timeToAttack;

        Collider2D[] targets = Physics2D.OverlapBoxAll(transform.position, attackSize, 0f);

        for (int i = 0; i < targets.Length; i++)
        {
            Character character = targets[i].GetComponent<Character>();
            if (character != null)
            {
                character.TakeDamage(damage);
            }
        }
    }
    #endregion

    /// <summary>
    /// Updates all Creature UI Elements with relevant data
    /// </summary>
    protected void UpdateCreatureUI()
    {
        bool vis = health.currVal < health.maxVal;

        hpBarExternal.gameObject.SetActive(vis);

        hpBarExternal.Set(health.currVal, health.maxVal);

    }
}
