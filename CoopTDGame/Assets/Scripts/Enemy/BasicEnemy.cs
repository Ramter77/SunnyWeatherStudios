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

    [Header("BehaviorStates")]
    public int attackState = 0; // 0 == not attacking // 1 == attacking // 2 == has recently attacked
    [SerializeField] private int action = 0;
    [SerializeField] private int decisionLimit = 0;
    [SerializeField] private float preparationTime = 0f;

    [Header("Interaction/Vision/Attack Radius")]
    public float attackRange = 5f;
    [SerializeField] private float followRadius = 15f; 
    [SerializeField] private float stoppingRange = 3.5f; // stops the ai from bumping into targets
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private float scanDelay = 5f;
    [SerializeField] private float minPreparationTimeForAttack = 1f;
    [SerializeField] private float maxPreparationTimeForAttack = 5f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        Sphere = GameObject.FindGameObjectWithTag("Sphere");
        WalkToSphere();
        StartCoroutine(ScanCycle());
        preparationTime = Random.Range(1, maxPreparationTimeForAttack);
        attackIndication.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //CheckDestinationReached();

        if (Target != null)
        {
            float distance = Vector3.Distance(Target.transform.position, transform.position);

            if (distance <= detectionRadius && distance > stoppingRange)
            {
                agent.isStopped = false;
                agent.SetDestination(Target.transform.position);
            }

            if (distance <= attackRange) // in attack range
            {
                attackState = 1;
                FaceTowardsPlayer();
                prepareAttack();
                Debug.Log("Ai: Preparing Attack now");
            }


            if (distance <= stoppingRange) // in stopping range prevents ai from bumping into player
            {
                agent.isStopped = true;
                rigid.velocity = Vector3.zero;
                rigid.angularVelocity = Vector3.zero;
            }

            if ((attackState == 1 && distance >= followRadius) || Target.gameObject.tag == "destroyedTarget") // if target moves away or 
            {
                Target = null;
                WalkToSphere();
                StartCoroutine(ScanCycle());
                agent.isStopped = false;
                checkedTarget = null;
                gameObject.GetComponent<AttackAndDamage>().Target = null;
                preparationTime = Random.Range(1, maxPreparationTimeForAttack);
                attackState = 0;
            }
        }


        //Move animation here?
    }


    public void prepareAttack()
    {
        // set the enemy animation to idle / preparation for attack
        preparationTime -= Time.deltaTime;  
        if(preparationTime <= 0)
        {
            gameObject.GetComponent<AttackAndDamage>().Target = Target;
            gameObject.GetComponent<AttackAndDamage>().performAttack();
            preparationTime = Random.Range(1, maxPreparationTimeForAttack);
            attackIndication.SetActive(false);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }


    #region Detect Targets in close proximity

    /// <summary>
    /// checks the surrounding area for turrets and other targets
    /// </summary>
    void ScanScloseAreaForTargets()
    {
        StartCoroutine(ScanCycle());
        Collider[] col = Physics.OverlapSphere(transform.position, detectionRadius); // draw a sphere at desire point based on player pos + offset and desired radius of effect
        if (col.Length > 0)
        {
            foreach (Collider hit in col) // checks each object hit
            {
                if (hit.tag == "Player" || hit.tag == "Player2")
                {
                    Target = hit.gameObject;
                    checkedTarget = Target;
                    continue;
                }
            }
        }
    }

    void ScanForTower()
    {
        Collider[] tol = Physics.OverlapSphere(transform.position, detectionRadius);
        if(tol.Length > 0)
        {
            //Debug.Log("Checking for towers");
            foreach(Collider hit in tol)
            {
                if (hit.tag == "possibleTargets") // if hit object has equal tag to possibleTarget tag
                {
                    action = Random.Range(0, 100);
                    //Debug.Log("tower found");
                    if (checkedTarget == null)
                    {
                        if (action <= decisionLimit) // if decisionmaking percentage is lower than the limit, decide to do this
                        {
                            Debug.Log(hit.transform.parent.gameObject);          
                            //Debug.Log("I will rather go for a tower");
                            checkedTarget = hit.transform.parent.gameObject.transform.parent.gameObject;
                            NavMeshPath path = new NavMeshPath();
                            agent.CalculatePath(checkedTarget.transform.position, path);
                            if (path.status != NavMeshPathStatus.PathPartial) // checks if path is reachable
                            {
                                agent.destination = checkedTarget.transform.position;
                                Target = checkedTarget;
                                StopCoroutine(ScanCycle());
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
    

                
          

    IEnumerator ScanCycle()
    {
        yield return new WaitForSeconds(scanDelay);
        Debug.Log("check 2");
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
        }
        else
        {
            Debug.Log("No sphere found in game, unable to go there");
        }
    }

    #endregion
    /*
    #region Find closest target
    /// <summary>
    /// Finds the closest target out of all possible targets
    /// </summary>
    
    public void FindClosestTarget()
    {
        gos = GameObject.FindGameObjectsWithTag("possibleTargets");
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        { // loops through all objects in the gos array
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        NavMeshPath path = new NavMeshPath();
        if(closest != null)
        { // check if path is reachable, if so then set destination to closest target
            agent.CalculatePath(closest.transform.position, path);
            if (path.status != NavMeshPathStatus.PathPartial)
            {
                agent.destination = closest.transform.position;
                gameObject.GetComponent<AttackAndDamage>().Target = closest;
            }
        }
    }
#endregion
    */
}
