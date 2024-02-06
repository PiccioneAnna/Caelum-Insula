using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class BreakableObject : MonoBehaviour, IDamageable
{
    [SerializeField] float hp = 10;

    public void ApplyDamage(float damage)
    {
        hp -= damage;
    }

    public void CalculateDamage(ref float damage)
    {
        damage /= 2;
    }

    public void CheckState()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}