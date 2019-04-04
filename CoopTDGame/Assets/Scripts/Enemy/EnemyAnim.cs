using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnim : MonoBehaviour
{
    [SerializeField]
    private float dampTime = 0.2f;


    private NavMeshAgent agent;
    private Animator anim;
    private float speedPercentage;    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        speedPercentage = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("speedPercentage", speedPercentage, dampTime, Time.deltaTime);        
    }
}
