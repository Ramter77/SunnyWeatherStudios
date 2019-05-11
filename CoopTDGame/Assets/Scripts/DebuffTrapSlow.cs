using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebuffTrapSlow : MonoBehaviour
{
    /* public float enemyMaxSpeed = 7f; */
    public float moveSpeedMultiplier = 0.9f;

    private float multipliedSpeedPercentage;
    private NavMeshAgent agent;
    private EnemyAnim enemyAnim;
    //public float lifetime = 30f;



    BasicEnemy basicEnemyScript;
    private int oldEnemyType = -1;


    private void Start()
    {
        //enemyMaxSpeed = EnemySpawnCycle.Instance.Enemies[0].GetComponent<NavMeshAgent>().speed;

        //Destroy(gameObject, lifetime);
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Enemy")
        {
            basicEnemyScript = other.gameObject.GetComponent<BasicEnemy>();
            /* if (basicEnemyScript.enemyType != oldEnemyType) {
                oldEnemyType = basicEnemyScript.enemyType;
                enemyMaxSpeed = basicEnemyScript.enemySpeed;
            } */

            agent = other.gameObject.GetComponent<NavMeshAgent>();
            enemyAnim = other.gameObject.GetComponent<EnemyAnim>();

            
            //enemyMaxSpeed = agent.speed;

            multipliedSpeedPercentage = basicEnemyScript.enemySpeed * moveSpeedMultiplier;
            agent.speed = multipliedSpeedPercentage;
            enemyAnim.speedMultiplier *= moveSpeedMultiplier;

            Debug.Log("speed: "+agent.speed + " & enemyAnim: "+enemyAnim.speedMultiplier);
        }
    }

    /* private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            basicEnemyScript = other.gameObject.GetComponent<BasicEnemy>();
            if (basicEnemyScript.enemyType != oldEnemyType) {
                oldEnemyType = basicEnemyScript.enemyType;
                enemyMaxSpeed = basicEnemyScript.enemySpeed;
            }

            agent = other.gameObject.GetComponent<NavMeshAgent>();
            enemyAnim = other.gameObject.GetComponent<EnemyAnim>();

            
            //enemyMaxSpeed = agent.speed;

            multipliedSpeedPercentage = enemyMaxSpeed * moveSpeedMultiplier;
            agent.speed = multipliedSpeedPercentage;
            enemyAnim.speedMultiplier *= moveSpeedMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            basicEnemyScript = other.gameObject.GetComponent<BasicEnemy>();
            agent = other.gameObject.GetComponent<NavMeshAgent>();

            
            multipliedSpeedPercentage = enemyMaxSpeed / moveSpeedMultiplier;
            agent.speed = multipliedSpeedPercentage;
            enemyAnim.speedMultiplier /= moveSpeedMultiplier;
        }
    } */
}
