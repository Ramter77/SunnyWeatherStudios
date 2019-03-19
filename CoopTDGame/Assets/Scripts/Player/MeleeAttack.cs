using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    # region Variables and References
    
    [Header("References for components and gameObjects")]
    [SerializeField] private Animator playerAnim; // player animator
    [SerializeField] private Transform sphereSpawnPoint;
    //[SerializeField] private int enemyLayer;

    [Header("Key references for attacking")]
    [Tooltip("HotKey to trigger attack")]
    [SerializeField] private KeyCode hotkey = KeyCode.Mouse0; // key that triggers the attack
    [SerializeField] private string fire;

    [Header("Attack Settings")] 
    [SerializeField] private float damageSphereRadius; // radius to check for collision
    [SerializeField] private float damageDelay = 1f; // delay till damage gets applied
    [SerializeField] private float attackDamage; // damage to apply
    [SerializeField] private float attackSpeed = 0.0f;
    [SerializeField] private float attackCD = 1f;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
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
                attackSpeed = Time.time + attackCD;
                //Start animation & delay damage output
                playerAnim.SetTrigger("Attack");
                StartCoroutine(applyDamage());
            }
        }
        #endregion
    }

    #region damage Sphere
    /// <summary>
    /// Create a sphere and apply damage to all enemies in radius of that sphere
    /// </summary>
    void drawDamageSphere()
    {
        Vector3 centerOfSphere = sphereSpawnPoint.position; // postion for the center of the sphere
        Collider[] col = Physics.OverlapSphere(centerOfSphere, damageSphereRadius); // draw a sphere at desire point based on player pos + offset and desired radius of effect
        if (col.Length > 0)
        {
            foreach (Collider hit in col) // checks each object hit
            {
                if (hit.tag == "Enemy") // if hit object has equal tag to enemy tag
                {
                    // apply damage to enemy
                    hit.gameObject.GetComponent<LifeAndStats>().health -= attackDamage;
                    //Debug.Log(hit.gameObject.GetComponent<LifeAndStats>().health);
                }
            }
        }
    }
    #endregion


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(sphereSpawnPoint.position, damageSphereRadius);
    }

    IEnumerator applyDamage()
    {
        yield return new WaitForSeconds(damageDelay);
        drawDamageSphere();

    }
}
