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

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        WeaponTriggerCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region Input
        //if (Input.GetKeyDown(hotkey))
        if (Input.GetButton(fire))
        {
            //If cooldown is low enough: shoot
            if (Time.time > attackSpeed)
            {
                WeaponTriggerCollider.enabled = true;
                attackSpeed = Time.time + attackCD;
                //Start animation & delay damage output
                playerAnim.SetTrigger("Attack");
                StartCoroutine(deactivateWeaponTrigger());
            }
        }
        #endregion
    }
    IEnumerator deactivateWeaponTrigger()
    {
        yield return new WaitForSeconds(1f);
        WeaponTriggerCollider.enabled = false;
    }
}
