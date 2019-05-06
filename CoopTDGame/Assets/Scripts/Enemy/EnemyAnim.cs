using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnim : MonoBehaviour {
    [SerializeField]
    private float dampTime = 0.2f;
    [Tooltip ("Sets the 'speedPercentage' variable of the animator to handle movement animations")]
    [SerializeField]
    private bool setSpeedPercentage = true;
    [Tooltip ("Sets the agent speed to 0 when not in the 'Grounded' animation blend tree")]
    [SerializeField]
    private bool stopMovementWhenNotGrounded;
    [Tooltip ("Sets the animators 'Injured' layer weigth corresponding the health percentage calculate from the 'LifeAndStats' script")]
    [SerializeField]
    private bool setInjuredLayerWeight;

    private BasicEnemy basicEnemy;
    private LifeAndStats lifeAndStats;
    private NavMeshAgent agent;
    private Animator enemyAnim;
    private int injuredLayerIndex;
    private float maxSpeed;
    private float speedPercentage;
    private float healthPercentage; 

    void Start()
    {
        basicEnemy = GetComponent<BasicEnemy>();
        lifeAndStats = GetComponent<LifeAndStats>();
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<Animator>();

        maxSpeed = basicEnemy.enemySpeed;
        injuredLayerIndex = enemyAnim.GetLayerIndex("Injured");

        if (injuredLayerIndex == -1) {
            Debug.Log("Couldn't find 'Injured' layer");
        }
    }

    void Update()
    {
        if (stopMovementWhenNotGrounded) {
            //No movement when not in "Grounded" state
            if (!enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Grounded")) {
                agent.speed = 0;
            }
            else {
                agent.speed = maxSpeed;
            }
        }

        if (setInjuredLayerWeight) {
            //Calculate the health percentage & apply it to the Injured animation layer weight
            healthPercentage = Mathf.Clamp01(1 - (lifeAndStats.health / lifeAndStats.maxhealth));
            enemyAnim.SetLayerWeight(injuredLayerIndex, healthPercentage);
        }

        if (setSpeedPercentage) {
            //Calculate the speed percentage & apply it to the 'Grounded' animations
            speedPercentage = Mathf.Clamp01(agent.velocity.magnitude / maxSpeed);
            enemyAnim.SetFloat("speedPercentage", speedPercentage, 0, Time.deltaTime);
        }
    }
}
