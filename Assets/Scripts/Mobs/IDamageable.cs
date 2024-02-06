using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void CalculateDamage(ref float damage);
    public void ApplyDamage(float damage);
    public void CheckState();
}
