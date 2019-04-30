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
    private Animator enemyAnim;

    private float maxSpeed;
    private float speedPercentage;    

    void Start()
    {
        basicEnemy = GetComponent<BasicEnemy>();
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<Animator>();

        maxSpeed = basicEnemy.enemySpeed;
    }

    void Update()
    {
        //Debug.Log(enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"));
        //if (enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Grounded")) {
        //if (enemyAnim.GetLayerName(0) != "Grounded") {
         if (!enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Grounded")) {
            agent.speed = 0;
        } 
                 
        
         else {
            agent.speed = maxSpeed;
        }


        //Debug.Log("NOT GROUNDED ANIMA");
            //speedPercentage = agent.velocity.magnitude / maxSpeed;

            //Debug.Log("mag: " + agent.velocity.magnitude + " & maxSpeed: " + maxSpeed);
            speedPercentage = Mathf.Clamp01(agent.velocity.magnitude / maxSpeed);
            //Debug.Log(agent.velocity.magnitude + " :mag / maxSpeed: " + maxSpeed + " = " + speedPercentage);
            
            enemyAnim.SetFloat("speedPercentage", speedPercentage, 0, Time.deltaTime); 
    }
}
