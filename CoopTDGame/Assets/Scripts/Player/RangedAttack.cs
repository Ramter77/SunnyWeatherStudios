using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RangedAttack : MonoBehaviour
{
    #region Variables
    [Header ("Projectile")]
    [Tooltip("Projectile to throw")]
    [SerializeField]
    private GameObject projectile;
    private Rigidbody projectileRB;

    [Tooltip("Speed of projectile")]
    [SerializeField]
    public float projectileSpeed;
    public int projectileCost = 10;
    public float rangedAttackCooldown = 5f;
    private float rangedRechargeSpeed = 5f;

    [Tooltip("Origin of thrown projectile")]
    [SerializeField]    
    private Transform projectileOrigin;

    [Header ("RayCast")]
    [SerializeField]
    private LayerMask mask;
    private float maxDistance = 9999;
    [SerializeField]
    private float fallbackDistance = 100.0f;
    private Vector3 intersectionPoint, direction;

    [Header("UI Settings")]
    public Image rangedAbilityUiImageOn;
    public Image rangedAbilityUiImageOff;
    public Image rangedAbilityCooldownImage;


    #region Internal variables
    private PlayerController playC;
    private Animator playerAnim;
    private Camera MainCamera;
    private Component[] ownColliders;
    
    private bool _input;
    #endregion
    #endregion

    void Start() {
        playC = GetComponent<PlayerController>();   
        playerAnim = GetComponent<Animator>();
        
        /* string tag = "MainCamera";
        if (playC.Player_ == 2)
        {
            tag = "MainCamera2";
        }
        Debug.Log("Finding " + tag + " tag");
        MainCamera = GameObject.FindGameObjectWithTag(tag).GetComponent<Camera>(); */
        if(gameObject.GetComponentInChildren<FreeCameraLook>().GetComponentInChildren<Camera>())
        MainCamera = gameObject.GetComponentInChildren<FreeCameraLook>().GetComponentInChildren<Camera>();

        //Get own colliders
        ownColliders = GetComponents<Collider>();
        rangedRechargeSpeed = rangedAttackCooldown;
        if (rangedAbilityCooldownImage != null)
        {
            rangedAbilityCooldownImage.fillAmount = 0;
        }
    }

    void Update()
    {
        if(rangedAbilityCooldownImage != null)
        {
            rangedAbilityCooldownImage.fillAmount += 1 / rangedAttackCooldown * Time.deltaTime;
        }

        //* Player 0 input */
        if (playC.Player_ == 0)
        {
            _input = InputManager.Instance.Ranged0;
        }

        //* Player 1 input */
        else if (playC.Player_ == 1)
        {
            _input = InputManager.Instance.Ranged1;
        }

        //*Player 2 input */
        else if (playC.Player_ == 2) {
            _input = InputManager.Instance.Ranged2;
        }

        #region Input

        if (Time.time > rangedRechargeSpeed && SoulBackpack.Instance.sharedSoulAmount >= projectileCost)
        {
            if(rangedAbilityUiImageOn != null && rangedAbilityUiImageOff != null)
            {
                rangedAbilityUiImageOn.enabled = true;
                rangedAbilityUiImageOff.enabled = false;
            }

            if (_input)
            {
                if(!playC.isRangedAttacking && !playC.isMeleeAttacking && !playC.isInBuildMode && playC.isGrounded && !playC.isJumping && !playC.isDead)
                {
                    rangedRechargeSpeed = Time.time + rangedAttackCooldown;
                    if (rangedAbilityCooldownImage != null)
                        rangedAbilityCooldownImage.fillAmount = 0;

                    _RangedAttack();
                }
            }
            
        }
        else
        {
            if (rangedAbilityUiImageOn != null && rangedAbilityUiImageOff != null)
            {
                rangedAbilityUiImageOn.enabled = false;
                rangedAbilityUiImageOff.enabled = true;
            }
        }
        #endregion
    }

    private void _RangedAttack() {
        //If not ranged attacking
        if (!playC.isRangedAttacking && !playC.isMeleeAttacking && !playC.isInBuildMode && playC.isGrounded && !playC.isJumping && !playC.isDead) {
            playC.isRangedAttacking = true;

            SoulBackpack.Instance.reduceSoulsByCost(projectileCost);

            //Start animation which ShootProjectile() on event & resets isRangedAttacking
            ChangeProjectileTo(projectile); //change projectile to assigned GameObject
            playerAnim.SetTrigger("RangedAttack");
        }
    }

    private void IgnoreCollisionSelf(Collider col) {
        //!OLD  Ignore collisions with owner
        /* if (rb.GetComponent<SphereCollider>()) {
            Physics.IgnoreCollision(myCollider, rb.GetComponent<SphereCollider>());
        } */

        foreach (Collider _col in ownColliders) {
            Physics.IgnoreCollision(_col, col);
        }
    }

    public void resetRangedAttackCD() {
        playC.isRangedAttacking = false;
    }

    /// <summary>
    /// Changes projectile to provided GameObject
    /// </summary>
    /// <param name="_projectile">Change projectile to this parameter</param>
    public void ChangeProjectileTo(GameObject _projectile) {
        Debug.Log("Changed projectile to: " + _projectile);
        Rigidbody _projectileRB = _projectile.GetComponent<Rigidbody>();

        projectileRB = _projectileRB;
    }

    #region Public ShootProjectile
    /// <summary>
    /// Shoots the current projectile to the middle of the viewport by adding force to it and disabling collision with its owner
    /// </summary>
    public void ShootActiveProjectile()
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

        //else direct the projectile to the GetPoint(fallbackDistance) intersection
        else {
            direction = (ray.GetPoint(fallbackDistance) - projectileOrigin.position).normalized;
        }

        //Create projectile
        Rigidbody _projectileRB = Instantiate(projectileRB, projectileOrigin.position, projectileOrigin.rotation);

        //Add force
        _projectileRB.AddForce(direction * projectileSpeed, ForceMode.Impulse);

        //Ignore collisions with owner
        IgnoreCollisionSelf(projectileRB.GetComponent<Collider>());
    }
    #endregion
}