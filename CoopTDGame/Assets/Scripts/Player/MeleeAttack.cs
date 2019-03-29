using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    # region Variables and References
    
    [Header("References")]
    [SerializeField] private Animator playerAnim; // player animator
    [SerializeField] private Transform sphereSpawnPoint;
    public Collider WeaponTriggerCollider;
    //[SerializeField] private int enemyLayer;

    [Header("Attack Settings")] 
    [SerializeField] private float damageSphereRadius; // radius to check for collision
    [SerializeField] public float attackDamage; // damage to apply
    [SerializeField] private float attackSpeed = 99999;
    [SerializeField] private float attackCD = .5f;
    [SerializeField] private bool attacking = false;


    private PlayerController playC;
    private bool _input, _runInput;
    #endregion

    void Start()
    {
        playC = GetComponent<PlayerController>();
        playerAnim = GetComponent<Animator>();

        attackSpeed = 0;
        attacking = false;
        WeaponTriggerCollider.enabled = false;
    }

    void Update()
    {
        //* Player 1 input */
        if (playC.Player_ == 1)
        {
            _input = InputManager.Instance.Fire2;
            _runInput = InputManager.Instance.isRunning;
        }

        //*Player 2 input */
        else {
            //Convert fire float to bool
            if (InputManager.Instance.Fire22 > 0) {
                _input = true;
            }
            else {
                _input = false;
            }
            _runInput = InputManager.Instance.isRunning2;
        }

        #region Input
        if (_input) {
            //If cooldown is low enough   dont need? cos of anim triggers resetting attacking bool
            if (Time.time > attackSpeed)
            {
                //If not already attacking
                if (!attacking) {
                    attacking = true;
                    _MeleeAttack();


                    //! OLD?
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

    private void _MeleeAttack() {
        #region Running melee attack
        if (_runInput) {
            playerAnim.SetTrigger("RunAttack");
        }
        #endregion

        #region Walking melee attack
        else {
            playerAnim.SetTrigger("MeleeAttack");
        }
        #endregion

        //Enable weaponCollider which gets disabled along the reset of the attackCD
        WeaponTriggerCollider.enabled = true;
    }
    
    public void resetMeleeAttackCD() {
        attacking = false;
        attackSpeed = 0;    //??
        //attackCD = 0;
        WeaponTriggerCollider.enabled = false;
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
}
