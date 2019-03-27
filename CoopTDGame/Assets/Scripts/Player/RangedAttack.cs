using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    #region Variables
    [Header ("Projectile")]
    [Tooltip("Projectile to throw")]
    [SerializeField]
    private Rigidbody projectile;

    [Tooltip("Speed of projectile")]
    [SerializeField]
    public float projectileSpeed;

    [Tooltip("Origin of thrown projectile")]
    [SerializeField]    
    private Transform projectileOrigin;


    [Header ("Parameters")]
    [Tooltip("HotKey to throw Projectile")]
    [SerializeField]
    private KeyCode hotkey = KeyCode.Mouse0;

    [Tooltip("Delay of instantiating projectile")]
    [SerializeField]
    private float shootDelay = 0.3f;

    [Tooltip("Attack cooldown")]
    [SerializeField]
    private float attackCD = 0.1f;
    private float attackSpeed;

    private Camera CameraM;
    private Animator playerAnim;
    #endregion

    void Start() {
        CameraM = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        #region Input
        if (Input.GetKeyDown(hotkey)) {
            //If cooldown is low enough: shoot
            if (Time.time > attackSpeed) {
                attackSpeed = Time.time + attackCD;

                //Start animation which shoots projectile on event
                playerAnim.SetTrigger("MagicAttack");
            }
        }    
        #endregion
    }

    #region Public ShootProjectile
    public void ShootProjectile()
    {
        //Raycast from screen center
        Ray ray = CameraM.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // take the point of collision (make sure all objects have a collider)
            Vector3 colisionPoint = hit.point;

            //Create a NORMALIZED vector for the path of the bullet from the 'gun' to the target
            Vector3 bulletVector = (colisionPoint - projectileOrigin.transform.position).normalized;

            //GameObject bulletInstance = Instantiate(bullet, bulletTransform) as GameObject;
            Rigidbody projectileRB = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);

            //Add Force
            projectileRB.AddForce(bulletVector * projectileSpeed, ForceMode.Impulse);
        }
    }
    #endregion
}
