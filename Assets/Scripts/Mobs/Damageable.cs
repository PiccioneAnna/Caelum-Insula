using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    IDamageable damageable;

    internal void TakeDamage(float damage)
    {
        damageable ??= GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.CalculateDamage(ref damage);
            damageable.ApplyDamage(damage);
            GameManager.Instance.screenMessageSystem.PostMessage(transform.position, damage.ToString());
            damageable.CheckState();
        }
    }
}
