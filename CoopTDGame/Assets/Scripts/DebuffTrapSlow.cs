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
            other.gameObject.GetComponent<StatusEffect>().FreezeCoroutine();
        }
    }
}
