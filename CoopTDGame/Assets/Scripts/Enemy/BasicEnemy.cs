using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{
    [Header("Navigation")]
    public NavMeshAgent agent;
    public GameObject closest = null; // closest target that is attackable
    private GameObject[] gos; // array containing all possible targets
    private Rigidbody rigid;

    [Header("BehaviorStates")]
    public int attackState = 0; // 0 == not attacking // 1 == attacking // 2 == has recently attacked

    [Header("Interaction/Vision/Attack Radius")]
    public float attackRange = 5f;
    private float stoppingRange = 3.5f; // stops the ai from bumping into targets


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        FindClosestTarget();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDestinationReached();
        //Debug.Log("Agent" + agent.velocity);


        //Move animation here?
    }
    #region check destination reached
    /// <summary>
    /// Checks if enemy is close to target location
    /// </summary>
    public void CheckDestinationReached()
    {
        if(closest != null)
        {
            Vector3 diffToTarget = closest.transform.position - gameObject.transform.position;
            float curDistance = diffToTarget.sqrMagnitude;

            if (curDistance <= attackRange) // in attack range
            {
                //Debug.Log("AI: queue attack");
                attackState = 1;
                gameObject.GetComponent<AttackAndDamage>().performAttack();
            }
            if (curDistance <= stoppingRange) // in stopping range prevents ai from bumping into player
            {
                agent.destination = gameObject.transform.position;
                rigid.velocity = Vector3.zero;
                rigid.angularVelocity = Vector3.zero;

            }
            if ((attackState == 1 && curDistance >= attackRange) || closest.gameObject.tag == "destroyedTarget") // if target moves away or 
            {
                Array.Clear(gos, 0, gos.Length);
                FindClosestTarget();
                //gameObject.GetComponent<AttackAndDamage>().enableAttack = false;
            }
        }
        
    }

#endregion

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

}
