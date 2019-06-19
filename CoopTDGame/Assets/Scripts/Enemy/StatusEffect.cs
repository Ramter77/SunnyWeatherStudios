using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatusEffect : MonoBehaviour
{
    private ElementInteractor elemInteractorScript;

    [Header ("VISUALS")]
    public bool resetVFXinstantly = true;
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

    [Header ("Freeze")]
    [SerializeField]    
    private bool freezeOnStart;
    [SerializeField]
    private float freezeStartDelay = 0f;
    [SerializeField]
    private float freezeDelay = 4f;
    private bool startFreezing;

    
    [Header("EFFECT")]
    [Space (25)]
    public float effectDuration = 10f;
    
    [Header ("Burn")]
    //BURNING
    [SerializeField]
    private bool burning = false;
    public float burnDamage = 10f;
    public float timeBetweenEachBurn = 2f;
    private float fallbackTimeBetweenBurns = 2.0f;
    private LifeAndStats LifeScript;
    Coroutine BurnCoroutineReset;

    [Header ("Freeze")]
    //FREEZING
    [SerializeField]
    private bool freezing = false;
    //private bool appliedDot = false;
    [SerializeField]
    private float moveSpeedMultiplier = 0.7f;
    private float multipliedSpeedPercentage;
    private float multipliedSpeedMultiplier;
    private NavMeshAgent agent;
    private EnemyAnim enemyAnim;
    private BasicEnemy basicEnemyScript;
    private bool allowReset = true;
    Coroutine FreezeCoroutineReset;

    private float defaultMultipliedSpeedPercentage;
    private float defaultSpeed;
    private float defaultMoveSpeedMultiplier;
    
    

    void Start()
    {
        adjustMaterialScript = GetComponent<AdjustMaterial>();
        basicEnemyScript = GetComponent<BasicEnemy>();
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<EnemyAnim>();
        LifeScript = GetComponent<LifeAndStats>();
        fallbackTimeBetweenBurns = timeBetweenEachBurn;

        elemInteractorScript = GetComponent<ElementInteractor>();
        elemInteractorScript.allowInteraction = false;

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

        //defaultMultipliedSpeedPercentage = enemyAnim.speedMultiplier
        defaultSpeed = basicEnemyScript.enemySpeed;
        defaultMoveSpeedMultiplier = enemyAnim.speedMultiplier;
    }

    void Update() {
        /* #region Sinking
        if (startDissolving) {
            if (sinkWhileDissolving) {
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(0, sinkAmount, lerp), transform.position.z);
            }
        }
        #endregion */


        if (freezing)
        {
            SlowMovement();

            if (burning)
            {
                BurnEnemy();
            }

            /* if (!appliedDot) {
                appliedDot = true;
            } */
        }
        else
        {
            ResetSlow();
        }
    }

    public void DissolveCoroutine() {
        StartCoroutine(Dissolve());
    }

    public void BurnCoroutine() {
        //if already burning then reset resetting DoT
        if (burning) {
            StopCoroutine(BurnCoroutineReset);
            BurnCoroutineReset = StartCoroutine(resetDot());
        }
        else
        {
            StartCoroutine(Burn(true));
            BurnCoroutineReset = StartCoroutine(resetDot());
        }
    }

    public void FreezeCoroutine() {
        //if already freezing then reset resetting DoT
        if (freezing) {
            StopCoroutine(FreezeCoroutineReset);
            FreezeCoroutineReset = StartCoroutine(resetDot());
            
        }
        else
        {
            StartCoroutine(Freeze(true));
            FreezeCoroutineReset = StartCoroutine(resetDot());
        }
    }

    private IEnumerator Dissolve() {
        yield return new WaitForSeconds(dissolveStartDelay);

        adjustMaterialScript.Dissolve(dissolveDelay);

        //Disable colliders
        if (GetComponent<Ragdoll>() != null) {
            if (GetComponent<Ragdoll>().disableAllCollidersInstantly == false) {
                GetComponent<Ragdoll>().disableAllColliders();
            }
        }

        //Destroy after dissolveDelay time when completely dissolved
        if (weaponRenderer != null) {
            Destroy(weaponRenderer.gameObject, dissolveDelay);
        }
        Destroy(gameObject, dissolveDelay);
    }

    private IEnumerator Burn(bool isActive) {
        yield return new WaitForSeconds(burnStartDelay);
        burning = isActive;

        //Set type
        setInteraction(Element.Fire, true);

        //Adjust material
        adjustMaterialScript.Burn(burnDelay);
    }
    
    private IEnumerator Freeze(bool isActive) {
        yield return new WaitForSeconds(freezeStartDelay);
        freezing = isActive;

        //Set type
        setInteraction(Element.Ice, true);

        //Adjust material
        adjustMaterialScript.Freeze(freezeDelay);
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

    private void SlowMovement()
    { 
        multipliedSpeedPercentage = defaultSpeed * moveSpeedMultiplier;
        agent.speed = multipliedSpeedPercentage;

        multipliedSpeedMultiplier = defaultMoveSpeedMultiplier * moveSpeedMultiplier;
        enemyAnim.speedMultiplier = multipliedSpeedMultiplier;
    }

    private void ResetSlow() {
        agent.speed = defaultSpeed;
        enemyAnim.speedMultiplier = defaultMoveSpeedMultiplier;
    }

    public IEnumerator resetDot()
    {
        yield return new WaitForSeconds(effectDuration);
        if (burning) {
            StartCoroutine(Burn(false));

            timeBetweenEachBurn = 2f;
        }

        if (freezing) {
            StartCoroutine(Freeze(false));

            /* appliedDot = false; */
        }
        else if (!burning)
        {
            //multipliedSpeedPercentage = basicEnemyScript.enemySpeed / defaultMoveSpeedMultiplier;
            agent.speed = defaultSpeed;
            enemyAnim.speedMultiplier /= moveSpeedMultiplier;
        }

        setInteraction(Element.NoElement, false);
    }

    void setInteraction(Element element, bool allowInteraction) {
        elemInteractorScript.elementType = element;
        elemInteractorScript.allowInteraction = allowInteraction;
    }

    public void resetOnRagdoll() {
        //appliedDot = false;

        adjustMaterialScript.resetFX(resetVFXinstantly);

        setInteraction(Element.NoElement, false);
    }
}