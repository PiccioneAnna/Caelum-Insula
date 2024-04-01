using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    IDamageable damageable;

    internal void TakeDamage(float damage)
    {       
        if (TryGetComponent<IDamageable>(out damageable))
        {
            damageable.CalculateDamage(ref damage);
            damageable.ApplyDamage(damage);
            GameManager.Instance.screenMessageSystem.PostMessage(transform.position, damage.ToString());
            damageable.CheckState();
        }
    }
}
