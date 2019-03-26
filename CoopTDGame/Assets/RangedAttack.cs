using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
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
    public float projectileSpeed;


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
                playerAnim.SetTrigger("MagicAttack");
                StartCoroutine(shootProjectile(shootDelay));
            }
        }    
        #endregion

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 100;
        Debug.DrawRay(transform.position, forward, Color.green);
        //Debug.DrawRay(projectileOrigin.transform.position, Camera.main.transform.forward * 100, Color.green);
    }

    #region shootProjectile
    IEnumerator shootProjectile(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        //projectileRB.AddForce(projectileOrigin.forward * projectileSpeed, ForceMode.Impulse);

//This will send a raycast straight forward from your camera centre.
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        //check for a hit
        if (Physics.Raycast(ray, out hit))
        {
            // take the point of collision (make sure all objects have a collider)
            Vector3 colisionPoint = hit.point;

            //Create a vector for the path of the bullet from the 'gun' to the target
            Vector3 bulletVector = colisionPoint - projectileOrigin.transform.position;

            //GameObject bulletInstance = Instantiate(bullet, bulletTransform) as GameObject;
            Rigidbody projectileRB = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);

            //See it on it's way
            projectileRB.AddForce(bulletVector * projectileSpeed);

        }




        //RaycastHit hit;
        //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100))



/*
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint (new Vector3 (.5f, .5f, 0));
        Vector3 targetPos = rayOrigin - transform.position;

        projectileRB.AddForce(targetPos * projectileSpeed, ForceMode.Impulse);
        Debug.DrawRay(rayOrigin, targetPos, Color.yellow);


        RaycastHit hit;
        if (Physics.Raycast(rayOrigin,Camera.main.transform.forward, out hit, 100000)) {
            
            //hit.rigidbody.AddForce (-hit.normal * hitForce);

            //projectileRB.AddForce(hit.transform.position * projectileSpeed, ForceMode.Impulse);
            Debug.DrawRay(rayOrigin, hit.transform.position * 100000, Color.red);
        }
        //projectileRB.AddForce(rayOrigin * projectileSpeed, ForceMode.Impulse);
        Debug.DrawRay(rayOrigin, Camera.main.transform.forward * 100000, Color.green);
*/

/*
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        if (Physics.Raycast (rayOrigin, Camera.main.transform.forward, out hit, 100))
        {
            Debug.DrawRay(rayOrigin, Camera.main.transform.forward * 100, Color.yellow);

            Debug.Log("Ray hit: "+hit.distance);
            //hit.rigidbody.AddForce (-hit.normal * hitForce);

            Rigidbody projectileRB = Instantiate(projectile, projectileOrigin.position, transform.rotation);
            projectileRB.transform.LookAt(hit.point);



        //transform.TransformDirection(Vector3.forward) * 100;
            //Debug.DrawRay(projectileOrigin.position, Vector3.forward * 100, Color.green);
            projectileRB.AddForce(projectileOrigin.TransformDirection(Vector3.forward) * projectileSpeed, ForceMode.Impulse);
        }
*/

        



/* 
        Vector3 mousePos = Input.mousePosition;
        mousePos += Camera.main.transform.forward * 10f ; // Make sure to add some "depth" to the screen point 

        Vector3 mousePosition1 = Camera.main.ScreenToWorldPoint(mousePos);  
        //mousePosition.z = 50;      
        Vector3 directionVector = (mousePosition1 - transform.position).normalized;

        Debug.DrawLine (projectileOrigin.transform.position, mousePosition1);
        projectileRB.AddForce(directionVector * projectileSpeed, ForceMode.Force);
        */

/*
             var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
     var hit : RaycastHit;
     if (Physics.Raycast (ray, hit, 100)){
       Debug.DrawLine (character.position, hit.point);
       // cache oneSpawn object in spawnPt, if not cached yet
       if (!spawnPt) spawPt = GameObject.Find("oneSpawn");
       var projectile = Instantiate(bullet, spawnPt.transform.position, Quaternion.identity); 
       // turn the projectile to hit.point
       projectile.transform.LookAt(hit.point); 
       // accelerate it
       projectile.rigidbody.velocity = projectile.transform.forward * 10;
     }
*/
/*
        var position = Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
         position = Camera.main.ScreenToWorldPoint(position);
         var go = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
         go.transform.LookAt(position);    
         Debug.Log(position);    
         go.rigidbody.AddForce(go.transform.forward * 1000);
         */
    }
    #endregion
}
