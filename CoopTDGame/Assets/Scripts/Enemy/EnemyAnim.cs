using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnim : MonoBehaviour {
    /* [SerializeField]
    private float dampTime = 0.2f; */
    [Tooltip ("Sets the 'speedPercentage' variable of the animator to handle movement animations")]
    [SerializeField]
    private bool setSpeedPercentage = true;
    [Tooltip ("How long to lerp the speed percentage")]
    [SerializeField]
    private float setSpeedPercentageLerp = 0.2f;
    [Tooltip ("Multiplies the speed of all animations by the speedMultiplier")]
    [SerializeField]
    private bool setSpeedMultiplier = true;
    [Tooltip ("How long to lerp the speed multiplier")]
    [SerializeField]
    private float setSpeedMultiplierLerp = 0.2f;
    [Tooltip ("Multiplies the speed of all animations by this multiplier")]
    [SerializeField]
    public float speedMultiplier = 1f;    
    [Tooltip ("Sets the agent speed to 0 when not in the 'Grounded' animation blend tree")]
    [SerializeField]
    private bool stopMovementWhenNotGrounded;
    [Tooltip ("Sets the animators 'Injured' layer weigth corresponding the health percentage calculate from the 'LifeAndStats' script")]
    [SerializeField]
    private bool setInjuredLayerWeight;
    [SerializeField]
    [Range (0, 1)]
    private float maxLayerWeigth = 1;  
    [Tooltip ("Allows the 'Injured' layer to be blended")]
    [SerializeField]
    private bool blendInjuredIdle;

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
    }

    void SetInjuredLayerWeight(int layer, bool reset) {
        if (setInjuredLayerWeight) {
            if (reset) {
                enemyAnim.SetLayerWeight(layer, 0);
            }
            else {
                //Calculate the health percentage & apply it to the Injured animation layer weight
                healthPercentage = Mathf.Clamp01(1 - (lifeAndStats.health / lifeAndStats.maxhealth));

                //Clamp to minimum layer weigth
                if (healthPercentage >= maxLayerWeigth) {
                    healthPercentage = maxLayerWeigth;
                }

                enemyAnim.SetLayerWeight(layer, healthPercentage);
            }
        }
    }

    void ToggleLayerWeight(bool toggle) {
        if (blendInjuredIdle) {
            SetInjuredLayerWeight(injuredLayerIndex, toggle);
        }
        else
        {
            enemyAnim.SetLayerWeight(injuredLayerIndex, 0);
        }
        
        SetInjuredLayerWeight(injuredBlendLayerIndex, !toggle);
    }

    void Update()
    {
        if (stopMovementWhenNotGrounded) {
            //No movement when not in "Grounded" state
            if (!enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Grounded")) {
                agent.speed = 0;

                ToggleLayerWeight(false);

                //Dont blend it for ranged enemy
                if (basicEnemy.enemyType == 1) {

                }
            }
            //In grounded state set use injured blend tree
            else {
                agent.speed = maxSpeed;

                ToggleLayerWeight(true);
            }
        }

        
        #region SET SPEED PERCENTAGE
        if (setSpeedPercentage) {
            //Calculate the speed percentage & apply it to the 'Grounded' animations
            speedPercentage = Mathf.Clamp01(agent.velocity.magnitude / maxSpeed);
            enemyAnim.SetFloat("speedPercentage", speedPercentage, setSpeedPercentageLerp, Time.deltaTime);
        }
        #endregion

        #region SET SPEED MULTIPLIER
        if (setSpeedMultiplier) {
            enemyAnim.SetFloat("speedMultiplier", speedMultiplier, setSpeedMultiplierLerp, Time.deltaTime);
        }
        #endregion
    }
}
