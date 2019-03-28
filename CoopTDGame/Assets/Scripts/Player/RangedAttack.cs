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
    [Tooltip("Delay of instantiating projectile")]
    [SerializeField]
    private float shootDelay = 0.3f;

    [Tooltip("Attack cooldown")]
    [SerializeField]
    private float attackCD = 0.1f;
    private float attackSpeed;

    [Header ("RayCast")]
    public LayerMask mask;
    public float maxDistance = 100000;
    [SerializeField]
    private float defaultDistance = 100.0f;
    private Vector3 collisionPoint, direction;

    #region Internal variables
    private PlayerCont playC;
    private Camera CameraM;
    private Animator playerAnim;
    private bool _input;
    #endregion
    #endregion

    void Start() {        
        playerAnim = GetComponent<Animator>();

        playC = GetComponent<PlayerCont>();
        if (playC.Player_ == 1) {
            CameraM = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        else {
            CameraM = GameObject.FindGameObjectWithTag("MainCamera2").GetComponent<Camera>();
        }
    }

    void Update()
    {
        //* Player 1 input */
        if (playC.Player_ == 1)
        {
            _input = InputManager.Instance.Fire1;
        }

        //*Player 2 input */
        else {
            //Convert fire float to bool
            if (InputManager.Instance.Fire12 > 0) {
                _input = true;
            }
            else {
                _input = false;
            }
        }
        
        #region Input
        if (_input) {
            _RangedAttack();
        }    
        #endregion
    }

    private void _RangedAttack() {
        //If cooldown is low enough: shoot
        if (Time.time > attackSpeed) {
            attackSpeed = Time.time + attackCD;

            //Start animation which shoots projectile on event
            playerAnim.SetTrigger("RangedAttack");
        }
    }

    #region Public ShootProjectile
    public void ShootProjectile()
    {
        //Raycast from screen center
        Ray ray = CameraM.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //if raycast hits then send the projectile to the collision point
        if (Physics.Raycast(ray, out hit, maxDistance, mask))
        {
            // take the intersection
            collisionPoint = hit.point;

            //Create a NORMALIZED vector for the path of the projectile from the projectileOrigin to the intersection
            direction = (collisionPoint - projectileOrigin.position).normalized;            
        }

        //else send it to the GetPoint(defaultDistance) intersection
        else {
            direction = (ray.GetPoint(defaultDistance) - projectileOrigin.position).normalized;
        }

        //SEND PROJECTILE
        Rigidbody projectileRB = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
        projectileRB.AddForce(direction * projectileSpeed, ForceMode.Impulse);
    }
    #endregion
}