using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    # region Variables and References
    
    [Header("References for components and gameObjects")]
    [SerializeField] private Animator playerAnim; // player animator
    [SerializeField] private Transform sphereSpawnPoint;
    public Collider WeaponTriggerCollider;
    //[SerializeField] private int enemyLayer;

    [Header("Key references for attacking")]
    [Tooltip("HotKey to trigger attack")]
    [SerializeField] private KeyCode hotkey = KeyCode.Mouse0; // key that triggers the attack
    [SerializeField] private string fire;

    [Header("Attack Settings")] 
    [SerializeField] private float damageSphereRadius; // radius to check for collision
    [SerializeField] public float attackDamage; // damage to apply
    [SerializeField] private float attackSpeed = 0.0f;
    [SerializeField] private float attackCD = .5f;
    [SerializeField] private bool attacking = false;

    #endregion

    void Start()
    {
        attackSpeed = 0;
        attacking = false;
        playerAnim = GetComponent<Animator>();
        WeaponTriggerCollider.enabled = false;
    }

    void Update()
    {
        #region Input
        if (InputManager.Instance.Fire1)
        {
            //If cooldown is low enough
            if (Time.time > attackSpeed)
            {
                //If not already attacking
                if (!attacking) {
                    attacking = true;


                    #region Running attack
                    if (InputManager.Instance.isRunning) {
                        playerAnim.SetTrigger("RunAttack");
                    }
                    #endregion

                    #region Normal melee
                    else {
                        playerAnim.SetTrigger("MeleeAttack");
                    }
                    #endregion

                    //Enable weaponCollider which gets diabled along the reset of the attackCD
                    WeaponTriggerCollider.enabled = true;




                    //attackSpeed = Time.time + attackCD;
                    //StartCoroutine(activateDelayWeaponTrigger());
                    //Start animation & delay damage output
                    //StartCoroutine(deactivateWeaponTrigger());
                    //StartCoroutine(resetAttackingBool());
                }
            }
        }
        #endregion
    }
    IEnumerator deactivateWeaponTrigger()
    {
        yield return new WaitForSeconds(.6f);
        WeaponTriggerCollider.enabled = false;
    }


    IEnumerator resetAttackingBool()
    {
        yield return new WaitForSeconds(1f);
        attacking = false;
    }

    IEnumerator activateDelayWeaponTrigger()
    {
        yield return new WaitForSeconds(.2f);
        WeaponTriggerCollider.enabled = true;
        attacking = true;
    }


    public void resetMeleeAttackCD() {
        attacking = false;
        attackSpeed = 0;
        //attackCD = 0;
        WeaponTriggerCollider.enabled = false;
    }
}
