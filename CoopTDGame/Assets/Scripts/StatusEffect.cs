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

    [Header ("Freeze")]
    [SerializeField]    
    private bool freezeOnStart;
    [SerializeField]
    private float freezeStartDelay = 0f;
    [SerializeField]
    private float freezeDelay = 4f;
    private bool startFreezing;

    [Space (10)]
    //SLOW
    [SerializeField]
    private float moveSpeedMultiplier = 0.7f;
    private float multipliedSpeedPercentage;
    private NavMeshAgent agent;
    private EnemyAnim enemyAnim;
    private BasicEnemy basicEnemyScript;
    private bool slowed;

    void Start()
    {
        adjustMaterialScript = GetComponent<AdjustMaterial>();
        basicEnemyScript = GetComponent<BasicEnemy>();
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<EnemyAnim>();

        if (dissolveOnStart) {
            DissolveCoroutine();
        }

        if (burnOnStart) {
            BurnCoroutine();
        }

        if (freezeOnStart) {
            FreezeCoroutine();
        }
    }

    void Update() {
        /* #region Sinking
        if (startDissolving) {
            if (sinkWhileDissolving) {
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(0, sinkAmount, lerp), transform.position.z);
            }
        }
        #endregion */
    }

    public void DissolveCoroutine() {
        StartCoroutine(Dissolve());
    }

    public void BurnCoroutine() {
        StartCoroutine(Burn());
    }

    public void FreezeCoroutine() {
        StartCoroutine(Freeze());
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

        adjustMaterialScript.Burn(burnDelay);
    }

    private IEnumerator Freeze() {
        yield return new WaitForSeconds(freezeStartDelay);

        adjustMaterialScript.Freeze(freezeDelay);

        SlowMovement();
    }

    private void SlowMovement()
    {
        //if (!slowed) {
          //  slowed = true;

            multipliedSpeedPercentage = basicEnemyScript.enemySpeed * moveSpeedMultiplier;
            agent.speed = multipliedSpeedPercentage;
            enemyAnim.speedMultiplier *= moveSpeedMultiplier;
        //}
    }
}