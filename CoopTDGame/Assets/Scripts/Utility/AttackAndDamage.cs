﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAndDamage : MonoBehaviour
{
    [Header("Attack Stats")]
    public float damage = 20f; // flat damage amount
    private float attackSpeed = 2f; // 1f = 1 attack per second ; 2f = 1 attack every 2 seconds ; ...
    private float penetrationFactor = 3f; // factor that recudes the amount of damage reduction the target recives through its defense stat
    public float damageDelay = 1f;


    [Header("Target")]
    public GameObject Target = null;
    public bool enableAttack = true;

    [Header("Damage Calculation")]
    public float targetDefense = 0f;

    [Header ("Animation")]
    private Animator enemyAnim;

    // Start is called before the first frame update
    void Start()
    {
        enableAttack = true;
        enemyAnim = GetComponent<Animator>();
    }


    public void performAttack()
    {
        //Debug.Log("Enemy wdasfattacked");
        if (enableAttack)
        {
            if(Target != null)
            {
                enemyAnim.SetTrigger("Attack");
                enableAttack = false;
                StartCoroutine(resetAttackCooldown());
            }
            else
            {
                return;
            }
        }
    }

    public void enemyDamageApply()
    {
        StartCoroutine(damageApply());
    }

    IEnumerator damageApply()
    {
        yield return new WaitForSeconds(damageDelay);
        targetDefense = Target.GetComponent<LifeAndStats>().defense;
        float applyingDamage = damage - targetDefense / penetrationFactor; // calculates the damage for the 
        Target.GetComponent<LifeAndStats>().TakeDamage(applyingDamage);
    }



    public void applyDamage()
    {
        targetDefense = Target.GetComponent<LifeAndStats>().defense;
        float applyingDamage = damage - targetDefense / penetrationFactor; // calculates the damage for the 
        Target.GetComponent<LifeAndStats>().TakeDamage(applyingDamage);
    }



    IEnumerator resetAttackCooldown()
    {
        yield return new WaitForSeconds(attackSpeed);
        enableAttack = true;
    }

}
