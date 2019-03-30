using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    #region Variables and References    
    [Header("References")]
    [SerializeField] private Animator playerAnim; // player animator
    [SerializeField] private Transform sphereSpawnPoint;
    public Collider WeaponTriggerCollider;
    //[SerializeField] private int enemyLayer;

    [Header("Attack Settings")] 
    [SerializeField] private float damageSphereRadius; // radius to check for collision
    [SerializeField] public float attackDamage; // damage to apply
    //[SerializeField] private bool attacking = false;


    private PlayerController playC;
    private bool _input, _runInput;
    #endregion

    void Start()
    {
        playC = GetComponent<PlayerController>();
        playerAnim = GetComponent<Animator>();

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
            _input = InputManager.Instance.Fire22;
            _runInput = InputManager.Instance.isRunning2;
        }

        #region Input
        if (_input) {
            //If not already attacking
            if (!playC.isMeleeAttacking && !playC.isRangedAttacking && !playC.isInBuildMode) {
                playC.isMeleeAttacking = true;
                _MeleeAttack();
            }
        }
        #endregion
    }

    private void _MeleeAttack() {
        #region Running melee attack
        if (_runInput) {
            //Start animation & resets isMeleeAttacking
            playerAnim.SetTrigger("RunAttack");
        }
        #endregion

        #region Walking melee attack
        else {
            //Start animation & resets isMeleeAttacking
            playerAnim.SetTrigger("MeleeAttack");
        }
        #endregion

        //Enable weaponCollider which gets disabled along the reset of the attackCD
        WeaponTriggerCollider.enabled = true;
    }
    
    public void resetMeleeAttackCD() {
        playC.isMeleeAttacking = false;
        WeaponTriggerCollider.enabled = false;
    }
}
