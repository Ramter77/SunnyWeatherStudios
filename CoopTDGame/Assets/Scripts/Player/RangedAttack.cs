using System.Collections;
using System.Collections.Generic;
using System;
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
    [Tooltip("Delay of instantiating projectile")]
    [SerializeField]
    private float shootDelay = 0.3f;

    [Tooltip("Attack cooldown")]
    [SerializeField]
    private float attackCD = 0.1f;
    private float attackSpeed;

    [Header ("RayCast")]
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private float maxDistance = 99999;
    [SerializeField]
    private float defaultDistance = 100.0f;
    private Vector3 intersectionPoint, direction;

    #region Internal variables
    private PlayerController playC;
    private Camera MainCamera;
    private Animator playerAnim;
    private bool _input;


    private CapsuleCollider myCollider;
    #endregion
    #endregion

    void Start() {        
        playerAnim = GetComponent<Animator>();
        playC = GetComponent<PlayerController>();

        string tag = "MainCamera";
        if (playC.Player_ == 2)
        {
            tag = "MainCamera2";
        }
        Debug.Log("Finding " + tag + " tag");
        MainCamera = GameObject.FindGameObjectWithTag(tag).GetComponent<Camera>();

        myCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        //* Player 1 input */
        if (playC.Player_ == 1)
        {
            _input = InputManager.Instance.Fire1;
        }

        //*Player 2 input */
        else if (playC.Player_ == 2) {
            _input = InputManager.Instance.Fire12;
        }
        
        #region Input
        if (_input) {
            _RangedAttack();
        }    
        #endregion
    }

    private void _RangedAttack() {
        //If not ranged attacking
        if (!playC.isRangedAttacking && !playC.isMeleeAttacking && !playC.isInBuildMode) {
            playC.isRangedAttacking = true;

            //Start animation which ShootProjectile() on event & resets isRangedAttacking
            playerAnim.SetTrigger("RangedAttack");
        }
    }

    private void IgnoreCollisionSelf(Rigidbody rb) {
        //Ignore collisions with owner
        if (rb.GetComponent<SphereCollider>()) {
            Physics.IgnoreCollision(myCollider, rb.GetComponent<SphereCollider>());
        }
    }

    public void resetRangedAttackCD() {
        playC.isRangedAttacking = false;
    }

    #region Public ShootProjectile
    /// <summary>
    /// Shoots a projetile to the middle of the viewport
    /// </summary>
    /// <param name="_projectile">Projectile to shoot. Pass null to shoot projectile attached on "RangedAttack" component</param>
    /// <param name="_projectileSpeedMultiplier">Multiplies base speed by this multiplier.</param>
    public void ShootProjectile(Rigidbody _projectile, float _projectileSpeedMultiplier)
    {
        //Raycast from screen center
        Ray ray = MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //If ray hits something direct a projectile to the intersection
        if (Physics.Raycast(ray, out hit, maxDistance, mask))
        {
            //Get the intersection
            intersectionPoint = hit.point;

            //Create a NORMALIZED vector for the path of the projectile from the projectileOrigin to the intersection
            direction = (intersectionPoint - projectileOrigin.position).normalized;            
        }

        //else direct the projectile to the GetPoint(defaultDistance) intersection
        else {
            direction = (ray.GetPoint(defaultDistance) - projectileOrigin.position).normalized;
        }

        Rigidbody projectileRB = null;

        //Create & send PROJECTILE from projectileOrigin
        if (_projectile == null) {
            projectileRB = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
        }
        else {
            projectileRB = Instantiate(_projectile, projectileOrigin.position, projectileOrigin.rotation);
        }

        //Add force
        projectileRB.AddForce(direction * projectileSpeed * _projectileSpeedMultiplier, ForceMode.Impulse);

        //Ignore collisions with owner
        IgnoreCollisionSelf(projectileRB);
    }
    #endregion
}