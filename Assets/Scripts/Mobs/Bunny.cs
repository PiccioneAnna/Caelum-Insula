using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : Creature
{

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
        Attack();
    }

    public override void Attack()
    {
        attackTimer -= Time.deltaTime;
        stamina.currVal -= staminaDecay;

        if (attackTimer > 0f) { return; }

        attackTimer = timeToAttack;

        Collider2D[] targets = Physics2D.OverlapBoxAll(transform.position, attackSize, 0f);

        for (int i = 0; i < targets.Length; i++)
        {
            Character character = targets[i].GetComponent<Character>();
            if (character != null)
            {
                animator.SetTrigger(StaticAnimationStates.ATTACK);

                character.TakeDamage(damage);
            }
        }
    }


}
