using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFireball : MonoBehaviour
{
    //! ONLY TO TEST
    //Todo: convert this to player attack script?

    #region Variables
    [Tooltip("Projectile to throw")]
    [SerializeField]
    private Rigidbody projectile;

    [Tooltip("Origin of thrown projectile")]
    [SerializeField]    
    private Transform projectileOrigin;

    [Tooltip("HotKey to throw Fireball")]
    [SerializeField]
    private KeyCode hotkey = KeyCode.Mouse0;

    [SerializeField]
    private float shootDelay = 0.3f;
    [SerializeField]
    private float attackCD = 0.1f;
    private float attackSpeed;

    [SerializeField]
    private float speed = 10;

    [SerializeField]
    private float destroyTime = 10;



    private Animator playerAnim;

    
    #endregion

    void Start() {
        playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        #region Input
        if (Input.GetKeyDown(hotkey)) {
            //If cooldown is low enough: shoot
            if (Time.time > attackSpeed) {
                attackSpeed = Time.time + attackCD;

                //Start animation & delay projectile
                playerAnim.SetTrigger("Attack");
                StartCoroutine(shootProjectile(shootDelay));
            }
        }    
        #endregion
    }

    #region shootProjectile
    IEnumerator shootProjectile(float delay)
    {
        yield return new WaitForSeconds(delay);
        Rigidbody projectileRB = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation); 
    }
    #endregion
}
