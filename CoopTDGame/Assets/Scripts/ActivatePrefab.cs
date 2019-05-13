using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePrefab : MonoBehaviour
{
    [SerializeField]
    private float trapDuration = 10.0f;
    private float maxTrapDuration;
    private SphereCollider activationCollider;
    private Animation anim;
    private Transform meshChild;
    private CombineElements combineElementScript;
    private bool isTower;
    public bool towerActive;
    public bool trapActive;
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
        combineElementScript = meshChild.GetComponent<CombineElements>();

        //isTower = GetComponent<BasicTower>().enabled;
        if (GetComponent<BasicTower>()) {
            isTower = true;
        }
        else {
            riseTrap = anim.GetClip("RiseTrap");
            lowerTrap = anim.GetClip("LowerTrap");
        }

        maxTrapDuration = trapDuration;
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
            GetComponent<BasicTower>().activated = true;
            GetComponent<BasicTower>().startAiming();
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
        trapDuration = maxTrapDuration;
        startTrapCD = false;

        trapActive = false;
        enabledVFX.SetActive(false);
        disabledVFX.SetActive(true);

        anim.clip = lowerTrap;
        anim.Play();

        combineElementScript.disableVFX();
        activationCollider.enabled = true;
    }
}
