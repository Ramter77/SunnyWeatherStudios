﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{
    public bool ragdollOnStart;

    private Component[] ownColliders;
    private Component[] colliders;
    private Component[] rbs;
    


    Animator anim;
    EnemyAnim animScript;
    BasicEnemy enemyScript;
    AttackAndDamage dmgScript;
    LifeAndStats lifeScript;
    NavMeshAgent agent;
    Rigidbody rb;


    private float defaultSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //Disable ragdoll colliders
        colliders = GetComponentsInChildren<Collider>();
        foreach (Collider _col in colliders)
            _col.enabled = false;

        ownColliders = GetComponents<Collider>();
        foreach (Collider _col in ownColliders)
            _col.enabled = true;

        //Reenable needed colliders
        //transform.GetChild(2).GetComponent<SphereCollider>().enabled = true;


        rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody _rb in rbs)
            _rb.isKinematic = true;


        anim = GetComponent<Animator>();
        animScript = GetComponent<EnemyAnim>();
        enemyScript = GetComponent<BasicEnemy>();
        dmgScript = GetComponent<AttackAndDamage>();
        lifeScript = GetComponent<LifeAndStats>();
        rb = GetComponent<Rigidbody>();

        agent = GetComponent<NavMeshAgent>();


        defaultSpeed = agent.speed;


        if (ragdollOnStart) {
            toggleRagdoll(true);
        }
    }

    /// <summary>
    /// Sets ragdoll parameters depending on param
    /// </summary>
    /// <param name="ragdoll">Param to enable or disable ragdoll</param>
    public void toggleRagdoll(bool ragdoll) {

        //if ragdoll is true --> disable scripts..
        anim.enabled = !ragdoll;
        animScript.enabled = !ragdoll;
        enemyScript.enabled = !ragdoll;
        dmgScript.enabled = !ragdoll;
        lifeScript.enabled = !ragdoll;
        agent.enabled = !ragdoll;
        
        //and set components
        rb.useGravity = ragdoll;
        rb.isKinematic = !ragdoll;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        //rb.freezeRotation = ragdoll;

        if (ragdoll) {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        else {
            rb.constraints = RigidbodyConstraints.None;
        }

        foreach (Collider _col in colliders)
            _col.enabled = true;

        foreach (Rigidbody _rb in rbs)
            _rb.isKinematic = false;


        //Disable attack indicator
        transform.GetChild(2).gameObject.SetActive(false);
        //Destroy(transform.GetChild(2).gameObject);
        

        /* if (ragdoll) {
            agent.speed = 0;
        }
        else {
            agent.speed = defaultSpeed;
        } */
    }
}