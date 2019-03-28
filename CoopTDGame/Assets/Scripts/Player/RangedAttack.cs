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
    
    /* [SerializeField]
    private KeyCode hotkey = KeyCode.Mouse0; */

    [SerializeField]
    private string button = "Fire1";

    [Tooltip("Delay of instantiating projectile")]
    [SerializeField]
    private float shootDelay = 0.3f;

    [Tooltip("Attack cooldown")]
    [SerializeField]
    private float attackCD = 0.1f;
    private float attackSpeed;

    private Camera CameraM;
    private Animator playerAnim;



    private PlayerCont playC;
    bool _input;
    public LayerMask mask;
    public float maxDistance = 100000;

    Vector3 collisionPoint;
        Vector3 bulletVector;
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
        //Ray ray2 = CameraM.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //Vector3 direction = (ray.GetPoint(100000.0f) - projectileOrigin.transform.position).normalized;
        //Debug.DrawLine(projectileOrigin.position, ray2.GetPoint(10.0f), Color.green, 10000);


        //* Player 1 input */
        if (playC.Player_ == 1)
        {
            _input = Input.GetButtonDown(button);
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
        }




        #region Input
        //if (InputManager.Instance.Fire1)) {            
        if (_input) {
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

        


        //if raycast hits then send the projectile to the collision point
        if (Physics.Raycast(ray, out hit, maxDistance, mask))
        {
            // take the point of collision (make sure all objects have a collider)
            collisionPoint = hit.point;

            //Create a NORMALIZED vector for the path of the bullet from the 'gun' to the target
            bulletVector = (collisionPoint - projectileOrigin.position).normalized;            
        }
        //else send it to the GetPoint(defaultDistance) intersection
        else {
            bulletVector = (ray.GetPoint(100.0f) - projectileOrigin.position).normalized;
        }
        //SEND PROJECTILE
        Rigidbody projectileRB = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);

        //Add Force
        projectileRB.AddForce(bulletVector * projectileSpeed, ForceMode.Impulse);


        //Ray ray = Camera.main.ScreenPointToRay(crossHair.transform.position);

    //GameObject bullet = Instantiate(bullet_prefab, bullet_spawn.transform.position, bullet_spawn.transform.rotation) as GameObject;


    //Rigidbody projectileRB = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
    //Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

    ///Vector3 direction = (ray.GetPoint(100000.0f) - projectileOrigin.position).normalized;

    //projectileRB.AddForce(direction * projectileSpeed, ForceMode.Impulse);
    }
    #endregion
}
