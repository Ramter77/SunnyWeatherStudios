using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnim : MonoBehaviour
{
    [SerializeField]
    private float dampTime = 0.2f;

    private BasicEnemy basicEnemy;
    private NavMeshAgent agent;
    private Animator anim;

    private float maxSpeed;
    private float speedPercentage;    

    void Start()
    {
        basicEnemy = GetComponent<BasicEnemy>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        maxSpeed = basicEnemy.enemySpeed;
    }

    void Update()
    {
        
        //speedPercentage = agent.velocity.magnitude / maxSpeed;

        //Debug.Log("mag: " + agent.velocity.magnitude + " & maxSpeed: " + maxSpeed);
        speedPercentage = Mathf.Clamp01(agent.velocity.magnitude / maxSpeed);
        //Debug.Log(agent.velocity.magnitude + " :mag / maxSpeed: " + maxSpeed + " = " + speedPercentage);
        
        anim.SetFloat("speedPercentage", speedPercentage, 0, Time.deltaTime);        
    }
}
