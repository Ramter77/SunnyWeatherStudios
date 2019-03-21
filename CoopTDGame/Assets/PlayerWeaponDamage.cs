﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponDamage : MonoBehaviour
{
    private float attackDamage = 0f;

    public GameObject ParentEnemy;
    // Start is called before the first frame update
    void Start()
    {
        attackDamage = ParentEnemy.GetComponent<MeleeAttack>().attackDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" )
        {
            Debug.Log("Weapon hit enemy");
            other.gameObject.GetComponent<LifeAndStats>().health -= attackDamage;
        }
    }
}
