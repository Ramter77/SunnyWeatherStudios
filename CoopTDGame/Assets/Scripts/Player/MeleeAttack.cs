using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    #region Variables and References    
    [Header("References")]
    [SerializeField] private Collider WeaponTriggerCollider;
    [SerializeField] private BoxCollider UltimateWeaponTriggerCollider;
    
    private PlayerController playC;
    private Animator playerAnim;
    private bool _input, _runInput;
    #endregion

    void Awake()
    {
        playC = GetComponent<PlayerController>();
        playerAnim = GetComponent<Animator>();

        WeaponTriggerCollider.enabled = false;
        UltimateWeaponTriggerCollider.enabled = false;
    }

    void Update()
    {
        if (!playC.isDead) {
            //* Player 0 input */
            if (playC.Player_ == 0)
            {
                _input = InputManager.Instance.Melee0;
                _runInput = InputManager.Instance.Run0;
            }

            //* Player 1 input */
            else if (playC.Player_ == 1)
            {
                _input = InputManager.Instance.Melee1;
                _runInput = InputManager.Instance.Run1;
            }

            //*Player 2 input */
            else if (playC.Player_ == 2) {
                _input = InputManager.Instance.Melee2;
                _runInput = InputManager.Instance.Run2;
            }
        }

        #region Input
        if (_input) {
            //If not already attacking or in build mode
            if (!playC.isMeleeAttacking && !playC.isInBuildMode && playC.isGrounded && !playC.isJumping) {
                playC.isMeleeAttacking = true;
                _MeleeAttack();
            }
        }
        #endregion
    }

    private void _MeleeAttack() {
        #region Running melee attack
        if (_runInput) {
            //Start animation which resets cooldown
            playerAnim.SetTrigger("RunAttack");
        }
        #endregion

        #region Walking melee attack
        else {
            //Start animation which resets cooldown
            playerAnim.SetTrigger("MeleeAttack");
        }
        #endregion

        ActivateWeaponCollider();
        
    }

    public void ActivateWeaponCollider() {
        //Enable weaponCollider which gets disabled along the reset of the attackCD
        //WeaponTriggerCollider.enabled = true;
        if (WeaponTriggerCollider != null) {
            WeaponTriggerCollider.enabled = true;
        }
        //else if (GameObject.FindGameObjectWithTag("UltWep")) {
            UltimateWeaponTriggerCollider.enabled = true;
        //}
    }
    
    #region Reset melee CD from the melee animations
    public void resetMeleeAttackCD() {
        playC.isMeleeAttacking = false;

        if (WeaponTriggerCollider != null) {
            WeaponTriggerCollider.enabled = false;
        }
UltimateWeaponTriggerCollider.enabled = false;

        /* else if (UltimateWeaponTriggerCollider != null) {
            UltimateWeaponTriggerCollider.enabled = false;
        } */
    }
    #endregion
}
