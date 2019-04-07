using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BasicEnemy : MonoBehaviour
{
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
    public int attackState = 0; // 0 == not attacking // 1 == attacking // 2 == has recently attacked
    [SerializeField] private int action = 0;
    [SerializeField] private int decisionLimit = 0;
    [SerializeField] private float preparationTime = 0f;
    [SerializeField] private int maxEnemiesSwarmingPlayer;
    [SerializeField] private int maxEnemiesSwarmingTower;


    [Header("Interaction/Vision/Attack Radius")]
    public float enemySpeed = 2f;
    public float attackRange = 5f;
    [SerializeField] private float followRadius = 15f; 
    [SerializeField] private float stoppingRange = 3.5f; // stops the ai from bumping into targets
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private float scanDelay = 5f;
    [SerializeField] private float minPreparationTimeForAttack = 1f;
    [SerializeField] private float maxPreparationTimeForAttack = 5f;



    private Animator enemyAnim;
    private bool charging;




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
    }

    void Update()
    {
        behaviorManager();
        
    }

    #region enemyBehaviorStates


    public void behaviorManager()
    {
        if(Target != null)
        {
            targetPos = Target.transform;
            float distance = Vector3.Distance(targetPos.position, transform.position);

            if (distance <= detectionRadius && distance > stoppingRange)
            {
                agent.isStopped = false;
                attackState = 1;
                agent.SetDestination(targetPos.position);
            }
            if (distance <= attackRange) // in attack range
            {
                FaceTowardsPlayer();
                prepareAttack();
                attackState = 1;
                //Debug.Log("Ai: Preparing Attack now");
            }
            if (distance <= stoppingRange) // in stopping range prevents ai from bumping into player
            {
                agent.isStopped = true;
                rigid.velocity = Vector3.zero;
                rigid.angularVelocity = Vector3.zero;
            }

            if ((attackState == 1 && distance > followRadius || Target.GetComponent<LifeAndStats>().health <= 0) || attackState == 1 && Target == null)
            {
                Target.GetComponent<LifeAndStats>().amountOfUnitsAttacking -= 1;
                Target = null;
                StartCoroutine(ScanCycle());
                agent.isStopped = false;
                attackState = 0;
                WalkToSphere();
            }

        }
    }


    public void prepareAttack()
    {
       // set the enemy animation to idle / preparation for attack
        /* if (!charging) {
            charging = true;
            agent.speed *= 0;
            enemyAnim.SetBool("Charge", true);
        } */

        preparationTime -= Time.deltaTime;  
        if(preparationTime <= 0)
        {
            gameObject.GetComponent<AttackAndDamage>().Target = Target;
            gameObject.GetComponent<AttackAndDamage>().performAttack();
            preparationTime = Random.Range(1, maxPreparationTimeForAttack);
            attackIndication.SetActive(false);
            
            charging = false;
            agent.speed *= 1;
            /* enemyAnim.SetBool("Charge", false); */
        }
        if(preparationTime <= 2)
        {
            // display attack indication 
            attackIndication.SetActive(true);
        }
    }


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
                    if ((hit.tag == "Player" || hit.tag == "Player2") && hit.GetComponent<LifeAndStats>().health > 0 && hit.GetComponent<LifeAndStats>().amountOfUnitsAttacking < maxEnemiesSwarmingPlayer)
                    {
                        if(Target == null)
                        {
                            Target = hit.gameObject;
                            hit.GetComponent<LifeAndStats>().amountOfUnitsAttacking += 1;
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
                    if (hit.tag == "possibleTargets" && hit.transform.parent.transform.parent.GetComponent<LifeAndStats>().health > 0 && hit.transform.parent.transform.parent.GetComponent<LifeAndStats>().amountOfUnitsAttacking < maxEnemiesSwarmingTower)
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
        NavMeshPath path = new NavMeshPath();
        if (Sphere != null)
        { // check if path is reachable, if so then set destination to closest target
            agent.CalculatePath(Sphere.transform.position, path);
            if (path.status != NavMeshPathStatus.PathPartial)
            {
                agent.destination = Sphere.transform.position;
            }
            if(Target != null)
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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stoppingRange);
    }

}
