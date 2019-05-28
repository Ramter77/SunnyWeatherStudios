using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePrefab : MonoBehaviour
{
    [Header ("Trap")]
    public bool trapActive;
    [SerializeField]
    private float trapDuration = 30.0f;
    private float maxTrapDuration;

    [Header ("Tower")]
    public bool towerActive;
    [SerializeField]
    private float towerDuration = 30.0f;
    private float maxTowerDuration;
    [SerializeField]
    private bool playLowerAnim;

    
    private SphereCollider activationCollider;
    private Animation anim;
    private Transform meshChild;
    private CombineElements combineElementScript;
    private bool isTower;
    
    
    private bool startTrapCD, startTowerCD;
    private AnimationClip riseTower, lowerTower, riseTrap, lowerTrap;
    
    private PlayerController playC;
    private bool _input;

    [Header ("VFX")]
    [SerializeField]
    private GameObject riseVFX;
    [SerializeField]
    private float riseDuration = 3f;

    [Space (10)]

    [SerializeField]
    private GameObject disabledVFX;
    [SerializeField]
    private GameObject enabledVFX;
    

    void Start()
    {
        activationCollider = GetComponent<SphereCollider>();
        anim = GetComponent<Animation>();
        meshChild = transform.GetChild(0).GetChild(0);
        combineElementScript = meshChild.GetComponent<CombineElements>();

        //isTower = GetComponent<BasicTower>().enabled;
        if (GetComponent<BasicTower>()) {
            isTower = true;

            anim.Play("LowerTower");

            riseTower = anim.GetClip("RiseTower");
            lowerTower = anim.GetClip("LowerTower");
        }
        else {
            isTower = false;

            anim.Play("LowerTrap");

            riseTrap = anim.GetClip("RiseTrap");
            lowerTrap = anim.GetClip("LowerTrap");
        }

        
        maxTrapDuration = trapDuration;
        maxTowerDuration = towerDuration;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteraction.Instance.Player_1InRange = true;
        }
        if (other.CompareTag("Player2"))
        {
            PlayerInteraction.Instance.Player_2InRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteraction.Instance.Player_1InRange = false;
        }
        if (other.CompareTag("Player2"))
        {
            PlayerInteraction.Instance.Player_2InRange = false;
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
                other.GetComponent<PlayerClassOne>().risePrefab();
                if (other.CompareTag("Player"))
                {
                    PlayerInteraction.Instance.Player_1InRange = false;
                }
                if (other.CompareTag("Player2"))
                {
                    PlayerInteraction.Instance.Player_2InRange = false;
                }
            }
        }
    }

    void Update() {
        if (startTrapCD) {
            trapDuration -= Time.deltaTime;
            if (trapDuration <= 0.0f)
            {
                if (trapActive) {
                    _LowerTrap();
                }
            }
        }
        else if (startTowerCD) {
            towerDuration -= Time.deltaTime;
            if (towerDuration <= 0.0f)
            {
                if (towerActive)
                {
                    _LowerTower();
                }
            }
        }
    }

    void RiseVFX() {
        riseVFX.SetActive(true);
        AudioManager.Instance.PlaySound(playC.playerAudioSource, AudioManager.Instance.playerActivateBuilding);

        StartCoroutine(DisableRiseVFX());
    }

    IEnumerator DisableRiseVFX() {
        yield return new WaitForSeconds(riseDuration);
        riseVFX.SetActive(false);
    }

    void _RiseTower() {
        if (!towerActive) {
            towerActive = true;
            enabledVFX.SetActive(true);
            disabledVFX.SetActive(false);
            RiseVFX();

            activationCollider.enabled = false;

            anim.clip = riseTower;
            anim.Play();
            startTowerCD = true;

            GetComponent<BasicTower>().activated = true;
            GetComponent<BasicTower>().startAiming();
            gameObject.tag = "possibleTargets";
        }
    }

    public void _LowerTower() {
        if (towerActive) {
            towerDuration = maxTowerDuration;
            startTowerCD = false;

            towerActive = false;
            enabledVFX.SetActive(false);
            disabledVFX.SetActive(true);

            activationCollider.enabled = true;

            if (playLowerAnim) {
                anim.clip = lowerTower;
                anim.Play();
            }
            else
            {
                transform.GetChild(0).position = new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y - 24, transform.GetChild(0).position.z);
            }

            combineElementScript._SwitchBack();

            GetComponent<BasicTower>().activated = false;
            //GetComponent<BasicTower>().startAiming();
            gameObject.tag = "Untagged";

            //combineElementScript._SwitchBack();
        }
    }

    void _RiseTrap() {
        if (!trapActive) {
            trapActive = true;
            enabledVFX.SetActive(true);
            disabledVFX.SetActive(false);
            RiseVFX();

            activationCollider.enabled = false;
            anim.clip = riseTrap;
            anim.Play();
            startTrapCD = true;
        }
    }

    void _LowerTrap() {
        trapDuration = maxTrapDuration;
        startTrapCD = false;

        trapActive = false;
        enabledVFX.SetActive(false);
        disabledVFX.SetActive(true);

        anim.clip = lowerTrap;
        anim.Play();

        combineElementScript._SwitchBack();
        //combineElementScript.disableVFX();
        activationCollider.enabled = true;
    }
}
