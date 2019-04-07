﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    #region Variables and References    
    [Header("References")]
    [SerializeField] private Collider WeaponTriggerCollider;
    //[SerializeField] private int enemyLayer;

    [Header("Attack Settings")] 
    [Tooltip("Damage to apply")] public float attackDamage;

    
    private PlayerController playC;
    private Animator playerAnim;
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
        //* Player 0 input */
        if (playC.Player_ == 0)
        {
            _input = InputManager.Instance.Fire2;
            _runInput = InputManager.Instance.isRunning;
        }

        //* Player 1 input */
        else if (playC.Player_ == 1)
        {
            _input = InputManager.Instance.Fire21;
            _runInput = InputManager.Instance.isRunning1;
        }

        //*Player 2 input */
        else if (playC.Player_ == 2) {
            _input = InputManager.Instance.Fire22;
            _runInput = InputManager.Instance.isRunning2;
        }

        #region Input
        if (_input) {
            //If not already attacking or in build mode
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

        //Enable weaponCollider which gets disabled along the reset of the attackCD
        WeaponTriggerCollider.enabled = true;
    }
    
    #region Reset melee CD from the melee animations
    public void resetMeleeAttackCD() {
        playC.isMeleeAttacking = false;
        WeaponTriggerCollider.enabled = false;
    }
    #endregion
}
