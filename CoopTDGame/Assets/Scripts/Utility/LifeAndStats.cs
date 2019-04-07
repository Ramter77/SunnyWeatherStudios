﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeAndStats : MonoBehaviour
{
    public float health = 100f;
    public float maxhealth = 100f;
    public float defense = 20f;
    private float healCooldown = 5f;
    private float fallbackHealCooldown;
    public int amountOfUnitsAttacking = 0;

    #region Soul
    private bool dropSoul = true;   //(Controls if an object drops souls) //Todo: Randomize?
    private GameObject Soul;
    #endregion

    public GameObject particleEffect;
    public float ParticleOnHitEffectYoffset = 1;
    public bool destroyable;

    private Animator playerAnim;
    private FractureObject fractureScript;
    

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        fractureScript = GetComponent<FractureObject>();
        fallbackHealCooldown = healCooldown;
        amountOfUnitsAttacking = 0;
}

    void Update()
    {
        reduceHealthCooldown();

        if(gameObject.CompareTag("possibleTargets") && health <= 0)
        {
            //Debug.Log("yeah im dead");
            gameObject.tag = "destroyedTarget";

            if (destroyable) {
                fractureScript.Fracture();
            }
        }
        if (gameObject.CompareTag("Enemy"))
        {
            if (health <= 0) {
                #region Instantiate Soul & destroy self
                if (dropSoul) {
                    Vector3 dropPos = new Vector3(transform.position.x, transform.position.y+2, transform.position.z);
                    GameObject _Soul = Instantiate(Resources.Load("Soul", typeof(GameObject)), dropPos, Quaternion.identity) as GameObject;
                }
                if(GetComponent<BasicEnemy>().Target != null && GetComponent<BasicEnemy>().Target.GetComponent<LifeAndStats>().amountOfUnitsAttacking > 0)
                {
                    GetComponent<BasicEnemy>().Target.GetComponent<LifeAndStats>().amountOfUnitsAttacking -= 1;
                }
                Destroy(gameObject);
                #endregion
            }
        }

        if (gameObject.CompareTag("Player") || gameObject.CompareTag("Player2"))
        {
            if (health <= 0) {
                Debug.Log("Player dead");
                playerAnim.SetBool("Dead", true);
            }
        } 
    }

    public void TakeDamage(float dmg) {
        health -= dmg;
        ParticleOnHitEffect(ParticleOnHitEffectYoffset);
    }

    public void ParticleOnHitEffect(float yOffset) {
        if (particleEffect != null) {
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
            Instantiate(particleEffect, spawnPos, Quaternion.identity);
        }
    }

    public void healHealth(float healAmount)
    {
        if(healCooldown <= 0)
        {
            health += healAmount;
            healCooldown = fallbackHealCooldown;
        }
    }    

    void reduceHealthCooldown()
    {
        healCooldown -= Time.deltaTime;
    }

}
