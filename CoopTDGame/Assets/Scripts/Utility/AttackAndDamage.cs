﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAndDamage : MonoBehaviour
{
    [Header("Attack Stats")]
    public float damage = 20f; // flat damage amount
    [SerializeField]
    private float attackSpeed = 2f; // 1f = 1 attack per second ; 2f = 1 attack every 2 seconds ; ...
    private float penetrationFactor = 3f; // factor that recudes the amount of damage reduction the target recives through its defense stat
    public float damageDelay = 1f;


    [Header("Target")]
    public GameObject Target = null;
    public bool enableAttack = true;
    private bool targetInRange = false;
    public GameObject weapon;
    private BoxCollider weaponCollider;
    public Transform shootPoint = null;
    public GameObject rangedAttackProjectilePrefab = null;
    public GameObject MuzzleObject;

    [Header("Damage Calculation")]
    public float targetDefense = 0f;

    [Header ("Animation")]
    private Animator enemyAnim;
    private BasicEnemy basicEnemy;
    //private NavMeshAgent agent;
    private float defaultSpeed;

    private GameObject sphere;

    // Start is called before the first frame update
    void Start()
    {
        enableAttack = true;
        enemyAnim = GetComponent<Animator>();
        basicEnemy = GetComponent<BasicEnemy>();
        defaultSpeed = basicEnemy.enemySpeed;
        sphere = GameObject.FindGameObjectWithTag("Sphere");
        if(MuzzleObject != null)
        {
            MuzzleObject.SetActive(false);
        }

        if (weapon) {
            weaponCollider = weapon.GetComponent<BoxCollider>();
            DisableWeaponCollider();
        }
    }

    private void Update()
    {
        targetInRange = basicEnemy.targetInAttackRange;
    }

    public void performAttack(string attackMode)
    {
        if (enableAttack)
        {
            if (Target != null)
            {
                if(attackMode == "sphereAttack")
                {
                    enemyAnim.SetTrigger("AttackSphere");
                }

                if(attackMode == "melee")
                {
                    enemyAnim.SetTrigger("Attack");
                }
                enableAttack = false;
                StartCoroutine(resetAttackCooldown());

                if (attackMode == "range")
                {
                    //instantiate prefab
                    rangeAttack();
                }
            }
        }
    }
    


    /* public void enemyDamageApply()
    {
        StartCoroutine(damageApply());
    }

    IEnumerator damageApply()
    {
        yield return new WaitForSeconds(damageDelay);
        targetDefense = Target.GetComponent<LifeAndStats>().defense;
        float applyingDamage = damage - targetDefense / penetrationFactor; // calculates the damage for the 
        Target.GetComponent<LifeAndStats>().TakeDamage(applyingDamage);
    } */

    void rangeAttack()
    {
        if (shootPoint)
        {
            Vector3 targetRot = Target.transform.position - gameObject.transform.position;
            Vector3 aimOffset = new Vector3(0, 1, 0);
            Vector3 aimRotation = targetRot - aimOffset;

            MuzzleObject.transform.position = shootPoint.position;
            MuzzleObject.transform.rotation = Quaternion.LookRotation(aimRotation);
            MuzzleObject.SetActive(true);
            MuzzleObject.GetComponent<ParticleSystem>().Simulate(0.0f, true, true);
            MuzzleObject.GetComponent<ParticleSystem>().Play();

            Instantiate(rangedAttackProjectilePrefab, shootPoint.position, Quaternion.LookRotation(aimRotation));
        }
    }


    public void applyDamage()
    {
        if (Target != null) {
            targetDefense = Target.GetComponent<LifeAndStats>().defense;
            float applyingDamage = damage - targetDefense / penetrationFactor; // calculates the damage for the 
            Target.GetComponent<LifeAndStats>().TakeDamage(applyingDamage);
            
            //enemyAnim.SetBool("Charge", false);
        }
        else
        {
            Debug.Log("Target already destroyed: no damage applied");
        }
    }



    IEnumerator resetAttackCooldown()
    {
        yield return new WaitForSeconds(attackSpeed);
        enableAttack = true;
    }



    public void EnableWeaponCollider() {
        if (weaponCollider != null) {
            weaponCollider.enabled = true;
        }
    }

    public void DisableWeaponCollider() {
        if (weaponCollider != null) {
            weaponCollider.enabled = false;
        }
    }

}
