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
    [SerializeField] private string button;

    [Header("Attack Settings")] 
    [SerializeField] private float damageSphereRadius; // radius to check for collision
    [SerializeField] public float attackDamage; // damage to apply
    [SerializeField] private float attackSpeed = 0.0f;
    [SerializeField] private float attackCD = .5f;
    [SerializeField] private bool attacking = false;


    private PlayerCont playC;
    bool _input;
    private bool _runInput;

    #endregion

    void Start()
    {
        attackSpeed = 0;
        attacking = false;
        playerAnim = GetComponent<Animator>();
        WeaponTriggerCollider.enabled = false;

        playC = GetComponent<PlayerCont>();
    }

    void Update()
    {
        #region Input
        //if (InputManager.Instance.Fire1)


        
        //* Player 1 input */
        if (playC.Player_ == 1)
        {
            _input = Input.GetButton(button);
            _runInput = InputManager.Instance.isRunning;
        }

        //*Player 2 input */
        else {
            float t = Input.GetAxis(button);
            if (t > 0) {
                _input = true;
            }
            else {
                _input = false;
            }
            
            _runInput = InputManager.Instance.isRunning2;
        }

        if (_input) {
            //If cooldown is low enough
            if (Time.time > attackSpeed)
            {
                //If not already attacking
                if (!attacking) {
                    attacking = true;


                    #region Running attack
                    if (_runInput) {
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
