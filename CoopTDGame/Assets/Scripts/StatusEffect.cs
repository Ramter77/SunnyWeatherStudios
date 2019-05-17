using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatusEffect : MonoBehaviour
{
    public MeshRenderer weaponRenderer; 
    private MeshRenderer meshRenderer;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private AdjustMaterial adjustMaterialScript;



    [Header ("Dissolve")]
    [SerializeField]    
    private bool dissolveOnStart;
    [SerializeField]
    private float dissolveStartDelay = 0f;
    [SerializeField]
    private float dissolveDelay = 4f;
    /* [SerializeField]    
    private bool sinkWhileDissolving;
    [SerializeField]    
    private float sinkAmount = 1f; */
    private bool startDissolving;

    [Header ("Burn")]
    [SerializeField]    
    private bool burnOnStart;
    [SerializeField]
    private float burnStartDelay = 0f;
    [SerializeField]
    private float burnDelay = 4f;
    private bool startBurning;

    public float burnDamage = 10f;
    public float timeBetweenEachBurn = 2f;
    private float fallbackTimeBetweenBurns = 2.0f;

    [Header ("Freeze")]
    [SerializeField]    
    private bool freezeOnStart;
    [SerializeField]
    private float freezeStartDelay = 0f;
    [SerializeField]
    private float freezeDelay = 4f;
    private bool startFreezing;


    [Header("DOT SETTINGS")]
    public float DotDuration = 10f;

    public bool burning = false;

    public bool freezing = false;

    private bool appliedDot = false;

    [Space (10)]
    //SLOW
    [SerializeField]
    private float moveSpeedMultiplier = 0.7f;
    private float multipliedSpeedPercentage;
    private NavMeshAgent agent;
    private EnemyAnim enemyAnim;
    private BasicEnemy basicEnemyScript;
    private LifeAndStats LifeScript;
    private bool slowed;

    void Start()
    {
        adjustMaterialScript = GetComponent<AdjustMaterial>();
        basicEnemyScript = GetComponent<BasicEnemy>();
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<EnemyAnim>();
        LifeScript = GetComponent<LifeAndStats>();
        fallbackTimeBetweenBurns = timeBetweenEachBurn;

        if (dissolveOnStart) {
            DissolveCoroutine();
        }

        if (burnOnStart) {
            BurnCoroutine();
        }

        if (freezeOnStart) {
            FreezeCoroutine();
        }

        /* if (blastOnStart) {
            BlastCoroutine();
        } */
    }

    void Update() {
        /* #region Sinking
        if (startDissolving) {
            if (sinkWhileDissolving) {
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(0, sinkAmount, lerp), transform.position.z);
            }
        }
        #endregion */

        /* if (burning)
        {
            if (freezing && !appliedDot)
            {
                SlowMovement(freezing);
                appliedDot = true;
            }
            BurnEnemy();
        } */

        if (burning) {
            BurnEnemy();
        }
    }

    public void DissolveCoroutine() {
        StartCoroutine(Dissolve());
    }

    public void BurnCoroutine() {
        if (!burning) {
            StartCoroutine(Burn());
        }
    }

    public void FreezeCoroutine() {
        if (!freezing) {
            StartCoroutine(Freeze());
        }
    }

    private IEnumerator Dissolve() {
        yield return new WaitForSeconds(dissolveStartDelay);

        adjustMaterialScript.Dissolve(dissolveDelay);

        //Disable colliders
        if (GetComponent<Ragdoll>() != null) {
            GetComponent<Ragdoll>().disableAllColliders();
        }

        //Destroy after dissolveDelay time when completely dissolved
        if (weaponRenderer != null) {
            Destroy(weaponRenderer.gameObject, dissolveDelay + 0.1f);
        }
        Destroy(gameObject, dissolveDelay + 0.1f);
    }

    private IEnumerator Burn() {
        yield return new WaitForSeconds(burnStartDelay);
        burning = !burning;

        adjustMaterialScript.Burn(burnDelay);

        if (burning) {
            StartCoroutine(resetDot());
        }
    }

    
    private IEnumerator Freeze() {
        yield return new WaitForSeconds(freezeStartDelay);
        freezing = !freezing;

        adjustMaterialScript.Freeze(freezeDelay);
        SlowMovement(freezing);

        if (freezing) {
            StartCoroutine(resetDot());
        }
    }

    private void BurnEnemy()
    {
        if (LifeScript != null)
        {
            timeBetweenEachBurn -= Time.deltaTime;
            if (timeBetweenEachBurn <= 0)
            {
                LifeScript.TakeDamage(burnDamage);
                timeBetweenEachBurn = fallbackTimeBetweenBurns;
            }
        }
    }

    private void SlowMovement(bool toggle)
    {
        if (toggle) {
            multipliedSpeedPercentage = basicEnemyScript.enemySpeed * moveSpeedMultiplier;
            agent.speed = multipliedSpeedPercentage;
            enemyAnim.speedMultiplier *= moveSpeedMultiplier;
        }
        else
        {
            multipliedSpeedPercentage = basicEnemyScript.enemySpeed / moveSpeedMultiplier;
            agent.speed = multipliedSpeedPercentage;
            enemyAnim.speedMultiplier /= moveSpeedMultiplier;
        }
    }

    private IEnumerator resetDot()
    {
        yield return new WaitForSeconds(DotDuration);

        if (burning) {
            StartCoroutine(Burn());
        }

        if (freezing) {
            StartCoroutine(Freeze());
        }

        appliedDot = false;
        timeBetweenEachBurn = 2f;
    }

}