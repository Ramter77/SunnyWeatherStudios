using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebuffTrapSlow : MonoBehaviour
{
    public float enemyMaxSpeed = 7f;
    public float moveSpeedMultiplier = 0.9f;

    private float multipliedSpeedPercentage;
    private NavMeshAgent agent;
    public float lifetime = 30f;


    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            agent = other.gameObject.GetComponent<NavMeshAgent>();

            multipliedSpeedPercentage = enemyMaxSpeed * moveSpeedMultiplier;
            agent.speed = multipliedSpeedPercentage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            agent = other.gameObject.GetComponent<NavMeshAgent>();

            multipliedSpeedPercentage = enemyMaxSpeed / moveSpeedMultiplier;
            agent.speed = multipliedSpeedPercentage;
        }
    }
}
