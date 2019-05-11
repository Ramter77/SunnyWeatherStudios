﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePrefab : MonoBehaviour
{
    [SerializeField]
    private float trapDuration = 10.0f;
    private SphereCollider activationCollider;
    private Animation anim;
    private Transform meshChild;
    private bool isTower;
    private bool towerActive;
    private bool trapActive;
    private bool startTrapCD;
    private AnimationClip riseTrap, lowerTrap;

    private PlayerController playC;
    private bool _input;

    [Header ("VFX")]
    [SerializeField]
    private GameObject disabledVFX;
    [SerializeField]
    private GameObject enabledVFX;
    

    void Start()
    {
        activationCollider = GetComponent<SphereCollider>();
        anim = GetComponent<Animation>();
        meshChild = transform.GetChild(0).GetChild(0);

        //isTower = GetComponent<BasicTower>().enabled;
        if (GetComponent<BasicTower>()) {
            isTower = true;
        }
        else {
            riseTrap = anim.GetClip("RiseTrap");
            lowerTrap = anim.GetClip("LowerTrap");
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Colliding with " + other.tag);
        if (other.CompareTag("Player") || other.CompareTag("Player2")) {
            playC = other.GetComponent<PlayerController>();

            //* Player 0 input */
            if (playC.Player_ == 0)
            {
                _input = InputManager.Instance.Interact0;
            }

            //* Player 1 input */
            else if (playC.Player_ == 1)
            {
                _input = InputManager.Instance.Interact1;
            }

            //*Player 2 input */
            else if (playC.Player_ == 2) {
                _input = InputManager.Instance.Interact2;
            }

            if (_input) {
                if (isTower) {
                    _RiseTower();
                }
                else
                {
                    //It's a Trap
                    _RiseTrap();
                }
            }
        }
    }

    void Update() {
        if (startTrapCD) {
            trapDuration -= Time.deltaTime;
            if (trapDuration <= 0.0f)
            {
                _LowerTrap();
            }
        }
    }

    void _RiseTower() {
        if (!towerActive) {
            towerActive = true;
            enabledVFX.SetActive(true);
            disabledVFX.SetActive(false);

            activationCollider.enabled = false;
            anim.Play();
            meshChild.tag = "possibleTargets";
        }
    }

    void _RiseTrap() {
        if (!trapActive) {
            trapActive = true;
            enabledVFX.SetActive(true);
            disabledVFX.SetActive(false);

            activationCollider.enabled = false;
            anim.clip = riseTrap;
            anim.Play();
            startTrapCD = true;
        }
    }

    void _LowerTrap() {
        startTrapCD = false;

        trapActive = false;
        enabledVFX.SetActive(false);
        disabledVFX.SetActive(true);

        anim.clip = lowerTrap;
        anim.Play();
    }
}
