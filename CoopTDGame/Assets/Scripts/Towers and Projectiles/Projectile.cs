using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.VFX;

public class Projectile : MonoBehaviour
{   
    //[SerializeField]
    private float speed = 10;

    [Tooltip ("Seconds before destroying game object")]
    [SerializeField]
    private float destroyTime = 5;

    [Header ("On Collision")]
    [Tooltip ("Disable the damage script on contact")]
    [SerializeField]
    private bool disableDamageScriptOnContact;    
    [Tooltip ("Parent to colliders on contact")]
    [SerializeField]
    private bool parentOnContact;
    [Tooltip ("Adjust position, rotation & scale when parenting to colliders on contact")]
    [SerializeField]
    private bool adjustWhenParentingOnContact;
    [Tooltip ("Destroy game object on contact")]
    [SerializeField]
    private bool destroyOnContact;
    [Tooltip ("Set the velocity of the game object's rigidbody to zero on contact")]
    [SerializeField]
    private bool stopVelocityOnContact;
    [Tooltip ("Set the game object's rigidbody to kinematic")]
    [SerializeField]
    private bool kinematicOnContact;
    [Tooltip ("Uparent game object's child (if there is one) before destroying")]
    [SerializeField]
    private bool unparentChildOnContact;
    [Tooltip ("Destroy the game object's child")]
    [SerializeField]
    private bool destroyChildOnContact;
    [Tooltip ("Seconds before destroying the game object's child")]
    [SerializeField]
    private float childDestroyTime = 1;
    [Tooltip ("Name of VFX spawn rate property")]
    public static readonly string SPAWN_RATE_NAME = "SpawnRate";
    [Tooltip ("Name of VFX lifetime property")]
    [SerializeField]
    public static readonly string LIFETIME_RATE_NAME = "LifeTimeMinMax";
    [Tooltip ("Min & Max lifetime of child VFX in seconds (max should be lower than destroy time)")]
    [SerializeField]
    private Vector2 lifetimeMinMax = new Vector2(4,4);
    

    private Light lightSource;
    


    /* public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    } */

    void Start()
    {
        if (transform.childCount > 0) {
            if (transform.GetChild(0) != null) {
                //Set VFX lifetime
                GameObject child = transform.GetChild(0).gameObject;
                child.GetComponent<VisualEffect>().SetVector2(LIFETIME_RATE_NAME, lifetimeMinMax);
            }
        }



        //!NOT NEEDED AFTER ALL????? First disable light just before destroying object to stop TLA _DEBUG_STACK_LEAK
        lightSource = GetComponent<Light>();
        StartCoroutine(DisableLight(destroyTime));

        //Then destroy in destroyTime amout of seconds
        Destroy(gameObject, destroyTime);
    }

    IEnumerator DisableLight(float delay) {
        yield return new WaitForSeconds(delay);
        lightSource.enabled = false;
    }

    void Update()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        #region Child (Stop VFX spawnRate & maybe unparent child)
        if (transform != null) {
            if (transform.childCount > 0) {
                GameObject child = transform.GetChild(0).gameObject;
                if (unparentChildOnContact) {
                    #region Destroy Child
                    if (destroyChildOnContact) {
                        Destroy(child, destroyTime);
                    }
                    #endregion

                    child.transform.parent = null;
                }

                child.GetComponent<VisualEffect>().SetFloat(SPAWN_RATE_NAME, 0);
                child.GetComponent<VisualEffect>().SetVector2(LIFETIME_RATE_NAME, new Vector2(0,0));
            }
        }
        #endregion

        #region Destroy
        if (destroyOnContact) {
            Destroy(gameObject);
        }
        #endregion
        else {
            #region Turn off damage script on contact
            if (disableDamageScriptOnContact) {
                if (GetComponent<PlayerWeaponDamage>() != null) {
                    GetComponent<PlayerWeaponDamage>().enabled = false;
                }
            }
            #endregion

            #region Rigidbody
            if (GetComponent<Rigidbody>() != null) {
                Rigidbody rb = GetComponent<Rigidbody>();
                if (stopVelocityOnContact) {
                    rb.velocity = Vector3.zero;
                }
                if (kinematicOnContact) {
                    rb.isKinematic = true;
                }
            }
            #endregion

            #region Parent on contact
            if (parentOnContact) {
                gameObject.transform.SetParent(other.transform, adjustWhenParentingOnContact);
            }
            #endregion
        }
    }
}
