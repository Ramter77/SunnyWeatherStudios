﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Random = UnityEngine.Random;

public class BasicEnemy : MonoBehaviour
{
    [Header("Define enemy Type")]
    [Tooltip("0 = Melee; 1= Range; 2 = Boss")] public int enemyType = 0;
    

    [Header("Navigation")]
    public NavMeshAgent agent;
    public GameObject Target = null; // picked Target
    private Rigidbody rigid;
    private GameObject Sphere;
    private GameObject checkedTarget = null;
    public GameObject attackIndication;
    private Transform targetPos = null;
    public List<GameObject> possibleTargets;
    public bool checkedForTarget = false;

    [Header("BehaviorStates / Effect States")]

    [Tooltip("// 0 == not attacking // 1 == attacking // 2 == has recently attacked")]
    public int attackState = 0; 

    [Tooltip("True if there is a target in attack range")]
    public bool targetInAttackRange = false;

    [SerializeField] private int action = 0; // used for a decision if enemy wants to attack a target
    [SerializeField] private int decisionLimit = 0; // used to evaluate the decision
    [SerializeField] private bool detectedTarget = false; // if target got detected
    [Space(10)]
    [SerializeField] private int maxEnemiesSwarmingPlayer; // of this type of enemy
    [SerializeField] private int maxEnemiesSwarmingTower; // of this type of enemy


    [Header("Interaction/Vision/Attack Radius")]

    [Tooltip("default speed for enemies")]
    public float enemySpeed = 2f;
    private float fallbackSpeed = 0f;
    [Space(10)]
    [Tooltip("Attack range of the enemies")]
    public float attackRange = 5f;

    [Tooltip("the radius that the enemy can follow the target")]
    [SerializeField] private float followRadius = 15f;

    [Tooltip("makes the enemy stop and prevents bumping into targets")]
    [SerializeField] private float stoppingRange = 3.5f; 

    [Tooltip("The radius for enemies to detect a target")]
    [SerializeField] private float detectionRadius = 15f;

    [Tooltip("Seconds between each Scan (for targets)")]
    [SerializeField] private float scanDelay = 5f;
    [Space(10)]
    [Tooltip("Min time between attacks")]
    [SerializeField] private float minPreparationTimeForAttack = 1f;
    [SerializeField] private float preparationTime = 0f;
    [Tooltip("Max time between attacks")]
    [SerializeField] private float maxPreparationTimeForAttack = 5f;

    [Tooltip("Distance that the enemy flees")]
    public float fleeRange = 3f;

    private Animator enemyAnim;
    private bool charging;
    public bool enabledAttack;


    [SerializeField]
    private bool isFallbackTarget;

    [Header("Gizmos")]
    public bool alwaysShowGizmos = false;
    public float opacityOfGizmos = 0.1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        Sphere = GameObject.FindGameObjectWithTag("Sphere");
        WalkToSphere();
        StartCoroutine(ScanCycle());
        preparationTime = Random.Range(1, maxPreparationTimeForAttack);
        attackIndication.SetActive(false);
        enemyAnim = GetComponent<Animator>();
        possibleTargets = new List<GameObject>();
        agent.speed = enemySpeed;
        fallbackSpeed = enemySpeed;

    }

    void Update()
    {
        behaviorManager();
        
    }


    #region Enemy Behavior Manager

    /// <summary>
    /// Behavior Manager 
    /// </summary>

    public void behaviorManager()
    {
        if (Target != null)
        {
            targetPos = Target.transform;
            float distance = Vector3.Distance(targetPos.position, transform.position);

            /// if target is in range for the enemy
            if (distance <= detectionRadius && distance > stoppingRange)
            {
                MoveToTarget();
            }

            if (distance > followRadius) // if enemy 
            {
                stopAttackingTarget();
            }

            if (distance <= attackRange) // in attack range
            {
                FaceTowardsPlayer();
                prepareAttack();
                targetInAttackRange = true;
                attackState = 1;

                if (!isFallbackTarget) {
                    enemyAnim.SetBool("AllowDamage", true);
                }
            }
            else
            {
                targetInAttackRange = false;

                if (!isFallbackTarget) {
                    enemyAnim.SetBool("AllowDamage", false);

                    enemyAnim.SetBool("Charge", false);
                }
            }

            if (distance <= stoppingRange) // in stopping range prevents ai from bumping into player
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                rigid.velocity = Vector3.zero;
                rigid.angularVelocity = Vector3.zero;
                enemySpeed = 0f;
            }

            if ((attackState == 1 && distance > followRadius) || attackState == 1 && Target == null || Target != null && Target.GetComponent<LifeAndStats>().health <= 0)
            {
                if (Target != null)
                {
                    if (Target.GetComponent<LifeAndStats>().health <= 0)
                    {
                        stopAttackingTarget();
                    }
                }
                else
                {
                    stopAttackingTarget();
                }

            }
        }
    }


    #endregion






    #region enemyBehaviorStates
    /// <summary>
    /// move towards the target to attack it
    /// </summary>
    void MoveToTarget()
    {
        agent.isStopped = false;
        attackState = 1;
        agent.SetDestination(targetPos.position);
        enemySpeed = fallbackSpeed;
    }
    
    /// <summary>
    /// makes the enemy flee from the target
    /// </summary>
    void RunFromTarget()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - targetPos.position);
        Vector3 runTo = transform.position + transform.forward * fleeRange;
        NavMeshHit hit;
#pragma warning disable CS0618 // Typ oder Element ist veraltet
        NavMesh.SamplePosition(runTo, out hit, 5, areaMask: 1 << NavMesh.GetNavMeshLayerFromName("Default"));
#pragma warning restore CS0618 // Typ oder Element ist veraltet
        agent.SetDestination(hit.position);
    }

    /// <summary>
    /// makes the enemy stop attacking the target
    /// enemy then walks towards sphere
    /// </summary>
    void stopAttackingTarget()
    {
        if (!isFallbackTarget) {
            enemyAnim.SetBool("Charge", false);
        }

        Target.GetComponent<LifeAndStats>().amountOfUnitsAttacking -= 1;
        Target = null;
        StartCoroutine(ScanCycle());
        agent.isStopped = false;
        attackState = 0;
        attackIndication.SetActive(false);
        detectedTarget = false;
        enemySpeed = fallbackSpeed;
        WalkToSphere();
    }

    /// <summary>
    /// prepares the attack 
    /// </summary>
    public void prepareAttack()
    {
        if (!isFallbackTarget) {
            enemyAnim.SetBool("Charge", true);
        }

        enemySpeed = 0f;
        preparationTime -= Time.deltaTime;  
        if(preparationTime <= 0)
        {
            preparationTime = Random.Range(minPreparationTimeForAttack, maxPreparationTimeForAttack);

            gameObject.GetComponent<AttackAndDamage>().Target = Target;
            if(enemyType == 0 || enemyType == 2)
            {
                gameObject.GetComponent<AttackAndDamage>().performAttack("melee");
            }
            if(enemyType == 1)
            {
                gameObject.GetComponent<AttackAndDamage>().performAttack("range");
            }


            attackIndication.SetActive(false);
            
            //charging = false;
            //agent.speed *= 1;
            /* enemyAnim.SetBool("Charge", false); */
        }
        if(preparationTime <= 2)
        {
            // display attack indication 
            attackIndication.SetActive(true);
        }
    }

    /// <summary>
    /// makes the enemy rotate towards the target
    /// </summary>
    void FaceTowardsPlayer()
    {
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    #endregion 





    #region Detect Targets in close proximity

    /// <summary>
    /// checks the surrounding area for turrets and other targets
    /// </summary>
    void ScanScloseAreaForTargets()
    {
        if(Target == null)
        {
            //Debug.Log("scanning");
            StartCoroutine(ScanCycle());
            Collider[] col = Physics.OverlapSphere(transform.position, detectionRadius); // draw a sphere at desire point based on player pos + offset and desired radius of effect
            if (col.Length > 0)
            {
                foreach (Collider hit in col) // checks each object hit
                {
                    if(hit.tag == "Sphere")
                    {
                        if (Target == null)
                        {
                            Target = hit.gameObject;
                            if (enemyType != 1)
                            {
                                attackRange = 26f;
                                followRadius = 45f;
                                detectionRadius = 43f;
                                stoppingRange = 25f;
                            }
                            return;
                        }
                        
                    }

                    if ((hit.tag == "Player" || hit.tag == "Player2") && hit.GetComponent<LifeAndStats>().health > 0 && hit.GetComponent<LifeAndStats>().amountOfUnitsAttacking < maxEnemiesSwarmingPlayer)
                    {
                        if(Target == null)
                        {
                            if(hit.gameObject.GetComponent<LifeAndStats>().health > 0)
                            {
                                Target = hit.gameObject;
                                hit.GetComponent<LifeAndStats>().amountOfUnitsAttacking += 1;
                            }
                        }
                    }
                }
            }
        }
    }

    void ScanForTower()
    {
        if (Target == null)
        {
            Collider[] tol = Physics.OverlapSphere(transform.position, detectionRadius);
            if (tol.Length > 0)
            {
                //Debug.Log("Checking for towers");
                foreach (Collider hit in tol)
                {
                    if (hit.tag == "possibleTargets" && hit.transform.parent.gameObject.transform.parent.GetComponent<LifeAndStats>().health > 0 && hit.transform.parent.gameObject.transform.parent.GetComponent<LifeAndStats>().amountOfUnitsAttacking < maxEnemiesSwarmingTower)
                        // if hit object has equal tag to possibleTarget tag
                    {
                        action = Random.Range(0, 100);
                        //Debug.Log("tower found");
                        if (checkedTarget == null)
                        {
                            if (action <= decisionLimit) // if decisionmaking percentage is lower than the limit, decide to do this
                            {
                                //Debug.Log(hit.transform.parent.gameObject);          
                                //Debug.Log("I will rather go for a tower");
                                checkedTarget = hit.transform.parent.transform.parent.gameObject;
                                NavMeshPath path = new NavMeshPath();
                                agent.CalculatePath(checkedTarget.transform.position, path);
                                if (path.status != NavMeshPathStatus.PathPartial) // checks if path is reachable
                                {
                                    agent.destination = checkedTarget.transform.position;
                                    Target = checkedTarget;
                                    hit.transform.parent.transform.parent.GetComponent<LifeAndStats>().amountOfUnitsAttacking += 1;
                                }
                                else
                                {
                                    Debug.Log("AI: Target is unreachable");
                                    checkedTarget = null;
                                }
                            }
                        }
                    }
                }
            }
        }
        
    }
    
    IEnumerator ScanCycle()
    {
        yield return new WaitForSeconds(scanDelay);
        //Debug.Log("check 2");
        ScanScloseAreaForTargets();
        ScanForTower();
    }

    #endregion






    #region SetDestinationToSphere


    private void WalkToSphere()
    {
        if (Sphere != null)
        { // check if path is reachable, if so then set destination to closest target
            agent.destination = Sphere.transform.position;
            if (Target != null)
            {
                if(Target.GetComponent<LifeAndStats>())
                {
                    Target.GetComponent<LifeAndStats>().amountOfUnitsAttacking -= 1;
                }
            }
        }
        else
        {
            Debug.Log("No sphere found in game, unable to go there");
        }
    }

    #endregion



#if UNITY_EDITOR

    /// <summary>
    ///  Gizmos etc
    /// </summary>

    private void OnDrawGizmosSelected()
    {
        if(!alwaysShowGizmos)
        {
            if (!Target)
            {
                Handles.color = new Color(0, 1.0f, 0, opacityOfGizmos);
                Handles.DrawSolidDisc(transform.position, Vector3.down, detectionRadius);
                Handles.color = new Color(1.0f, 0, 0, opacityOfGizmos);
                Handles.DrawSolidDisc(transform.position, Vector3.down, followRadius);
                Handles.color = new Color(0, 0, 1.0f, opacityOfGizmos);
                Handles.DrawSolidDisc(transform.position, Vector3.down, attackRange);
                Gizmos.color = new Color(0.2f, 0.2f, 0.2f, opacityOfGizmos);
                Handles.DrawSolidDisc(transform.position, Vector3.down, stoppingRange);
            }
            Handles.color = new Color(0, 1.0f, 0, 0.05f);
            Handles.DrawSolidDisc(transform.position, Vector3.down, detectionRadius);
            float dashSize = 5f;
            if (Target)
            {
                Handles.color = new Color(1f, 0, 0, 1f);
                Handles.DrawDottedLine(transform.position, Target.transform.position, dashSize);
            }
        }
        
    }
    private void OnDrawGizmos()
    {
        if (enemyType == 0)
        {
            Handles.Label(transform.position + Vector3.up * 10f, "Enemy Type: Melee");
        }
        if (enemyType == 1)
        {
            Handles.Label(transform.position + Vector3.up * 10f, "Enemy Type: Ranged");
        }
        if (enemyType == 2)
        {
            Handles.Label(transform.position + Vector3.up * 10f, "Enemy Type: Boss");
        }
        if (alwaysShowGizmos)
        {
            if (!Target)
            {
                Handles.color = new Color(0, 1.0f, 0, opacityOfGizmos);
                Handles.DrawSolidDisc(transform.position, Vector3.down, detectionRadius);
                Handles.color = new Color(1.0f, 0, 0, opacityOfGizmos);
                Handles.DrawSolidDisc(transform.position, Vector3.down, followRadius);
                Handles.color = new Color(0, 0, 1.0f, opacityOfGizmos);
                Handles.DrawSolidDisc(transform.position, Vector3.down, attackRange);
                Gizmos.color = new Color(0.2f, 0.2f, 0.2f, opacityOfGizmos);
                Handles.DrawSolidDisc(transform.position, Vector3.down, stoppingRange);
            }
            Handles.color = new Color(0, 1.0f, 0, 0.05f);
            Handles.DrawSolidDisc(transform.position, Vector3.down, detectionRadius);
            float dashSize = 5f;
            if (Target)
            {
                Handles.color = new Color(1f, 0, 0, 1f);
                Handles.DrawDottedLine(transform.position, Target.transform.position, dashSize);
            }
        }
        
    }

#endif 
}
