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
    [Tooltip ("Multiplies the speed of all animations by the speedMultiplier")]
    [SerializeField]
    private bool setSpeedMultiplier = true;
    [Tooltip ("Multiplies the speed of all animations by this multiplier")]
    [SerializeField]
    private float speedMultiplier = 1f;    
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
    private int injuredLayerIndex, injuredBlendLayerIndex;
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
        injuredBlendLayerIndex = enemyAnim.GetLayerIndex("InjuredBlend");

        if (injuredLayerIndex == -1) {
            Debug.Log("Couldn't find 'Injured' layer");
        }
        if (injuredBlendLayerIndex == -1) {
            Debug.Log("Couldn't find 'InjuredBlend' layer");
        }

        #region SET SPEED MULTIPLIER
        if (setSpeedMultiplier) {
            enemyAnim.SetFloat("speedMultiplier", speedMultiplier, 0, Time.deltaTime);
        }
        #endregion
    }

    void SetLayerWeight(int layer, bool reset) {
        if (setInjuredLayerWeight) {
            if (reset) {
                enemyAnim.SetLayerWeight(layer, 0);
            }
            else {
                //Calculate the health percentage & apply it to the Injured animation layer weight
                healthPercentage = Mathf.Clamp01(1 - (lifeAndStats.health / lifeAndStats.maxhealth));
                enemyAnim.SetLayerWeight(layer, healthPercentage);
            }
        }
    }

    void ToggleLayerWeight(bool toggle) {
        SetLayerWeight(injuredLayerIndex, toggle);
        SetLayerWeight(injuredBlendLayerIndex, !toggle);
    }

    void Update()
    {
        if (stopMovementWhenNotGrounded) {
            //No movement when not in "Grounded" state
            if (!enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Grounded")) {
                agent.speed = 0;

                ToggleLayerWeight(false);
            }
            //In grounded state set use injured blend tree
            else {
                agent.speed = maxSpeed;

                ToggleLayerWeight(true);
            }
        }

        

        if (setSpeedPercentage) {
            //Calculate the speed percentage & apply it to the 'Grounded' animations
            speedPercentage = Mathf.Clamp01(agent.velocity.magnitude / maxSpeed);
            enemyAnim.SetFloat("speedPercentage", speedPercentage, 0, Time.deltaTime);
        }
    }
}
