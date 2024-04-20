using CreatureClasses;
using InGameTasks;
using UI;
using UnityEngine;

#region CreatureStates
/// <summary>
/// Enum consists of all possible stats for the creature to be in
/// </summary>
public enum CreatureState
{
    Sleeping,
    Walking,
    Eating,
    Idle, // searching for next thing to do
    Hurting,
    Attacking,
    Defending,
    Fleeing,
    Working
}

/// <summary>
/// Enum consists of all possible goals that the creature can prioritize
/// </summary>
public enum CreatureGoal
{
    FindFood,
    Work,
    FindCompanySocial,
    FindRomanticCompany,
    RunAway,
    GetSleep
}
#endregion

/// <summary>
/// This class serves as a base class for ALL in game mobs
/// </summary>
[RequireComponent(typeof(Damageable))]
public class Creature : MonoBehaviour, IDamageable
{
    #region Fields
    // Reference to a scriptable object that contains creature information
    public CreatureInformation defaultCreatureInformation;
    public CreatureInformation creatureInformation;
    public CreatureUIManager creatureUIManager;
    public CreatureAnimationManager animator;
    
    public Stat health;
    public Stat stamina;
    public Stat foodIntake;
    public Stat mood;

    Transform player;
    [SerializeField] protected float speed;
    [SerializeField] protected Vector2 attackSize = Vector2.one;
    [SerializeField] protected Vector2 fovSize = Vector2.one;
    [SerializeField] protected int damage = 5;
    [SerializeField] protected float timeToAttack = 2f;
    [SerializeField] protected float staminaDecay = 1f;
    protected float attackTimer;

    public Vector2 direction;
    public Vector2 position;
    public Vector2 prevPosition;

    #region UI References
    public StatusBar hpBarExternal;

    #endregion

    #endregion

    #region Constructor

    public Creature()
    {

    }

    #endregion

    #region Runtime
    // Start is called before the first frame update
    void Start()
    {
        SetAllStatsToMax();
        FindPlayer();
        UpdateCreatureUI();

        attackTimer = Random.Range(0, timeToAttack);
    }
    #endregion

    #region Animator Calls
    public void Animate()
    {
        if(animator != null)
        {
            animator.dirValue = direction;
            animator.UpdateAnimation();
        }
    }
    #endregion

    #region Virtual Methods

    public virtual void Attack() { }
    public virtual void Defend() { }
    public virtual void Flee() { }
    public virtual void FindFood() { }
    public virtual void FindCompany() { }
    public virtual void Work(TaskInfo task) { }

    #endregion

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

    #region Creature Info
    /// <summary>
    /// Updates all Creature UI Elements with relevant data
    /// </summary>
    protected void UpdateCreatureUI()
    {
        bool vis = health.currVal < health.maxVal;

        hpBarExternal.gameObject.SetActive(vis);

        hpBarExternal.Set(health.currVal, health.maxVal);

    }
    #endregion

    #region Helper Methods Universal
    protected void MoveTowardsPlayer()
    {
        if(player == null) { FindPlayer(); }

        if (prevPosition != (Vector2)transform.position)
        {
            direction = (prevPosition - (Vector2)transform.position).normalized;
            prevPosition = transform.position;
        }

        // cuts out z so it stays 0 for rendering purposes
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
            );

        Animate();
    }

    protected void SetAllStatsToMax()
    {
        health.SetToMax();
        stamina.SetToMax();
        foodIntake.SetToMax();
        mood.SetToMax();
    }

    protected void FindPlayer()
    {
        player = GameManager.Instance.player.transform;
    }
    #endregion

}
