using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.VFX;

public class Projectile : MonoBehaviour
{   
    /* [SerializeField]
    private float speed = 10; */

    [Header ("Status effects")]
    [Tooltip ("Applies the burning status effect to enemies on contact")]
    [SerializeField]
    private bool burnEnemiesOnContact;
    [Tooltip ("Applies the freezing status effect to enemies on contact")]
    [SerializeField]
    private bool freezeEnemiesOnContact;

    

    [Header ("GetChild(0)")]
    [Header ("On Collision")]
    [Space (10)]
    [Tooltip ("Unparent game object's child (if there is one) before destroying")]
    [SerializeField]
    private bool unparentChildOnContact = false;
    [Tooltip ("Destroy the game object's child")]
    [SerializeField]
    private bool destroyChildOnContact = false;
    [Tooltip ("Seconds before destroying the game object's child")]
    [SerializeField]
    private float childDestroyTime = 1;
    private VisualEffect vfxChild;
    [Tooltip ("Name of VFX spawn rate property")]
    public static readonly string SPAWN_RATE_NAME = "SpawnRate";
    [Tooltip ("Name of VFX lifetime property")]
    [SerializeField]
    public static readonly string LIFETIME_RATE_NAME = "LifeTimeMinMax";
    [Tooltip ("Min & Max lifetime of child VFX in seconds (max should be lower than destroy time)")]
    [SerializeField]
    private Vector2 vfxLifetimeMinMax = new Vector2(4,4);

    [Header ("This gameObject")]
    [Tooltip ("Destroy game object on contact (If 'destroyOnContact' is enabled then the following options on collision are ignored)")]
    [SerializeField]
    private bool destroyOnContact = false;
    [Space (10)]
    [Tooltip ("Seconds before destroying game object")]
    [SerializeField]
    private float destroyTime = 5;
    [Tooltip ("Disable the damage script on contact")]
    [SerializeField]
    private bool disableDamageScriptOnContact = false;  
    [Tooltip ("Set the velocity of the game object's rigidbody to zero on contact")]
    [SerializeField]
    private bool stopVelocityOnContact = false;
    [Tooltip ("Set the game object's rigidbody to kinematic")]
    [SerializeField]
    private bool kinematicOnContact = false;
    [Tooltip ("Parent to colliders on contact")]
    [SerializeField]
    private bool parentOnContact = false;
    [Tooltip ("Adjust position, rotation & scale when parenting to colliders on contact")]
    [SerializeField]
    private bool adjustWhenParentingOnContact = false;
    

    
    

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
                child.GetComponent<VisualEffect>().SetVector2(LIFETIME_RATE_NAME, vfxLifetimeMinMax);
            }
        }



        //!NOT NEEDED AFTER ALL????? First disable light just before destroying object to stop TLA _DEBUG_STACK_LEAK
        lightSource = GetComponent<Light>();
        StartCoroutine(DisableLight(destroyTime));

        //Then destroy in destroyTime amout of seconds
        StartCoroutine(DestroyGO(destroyTime));
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
                
                if (child.GetComponent<VisualEffect>() != null) {
                    vfxChild = child.GetComponent<VisualEffect>();

                    vfxChild.SetFloat(SPAWN_RATE_NAME, 0);
                    vfxChild.SetVector2(LIFETIME_RATE_NAME, new Vector2(0,0));
                }
                
                if (unparentChildOnContact) {
                    #region Destroy Child
                    if (destroyChildOnContact) {
                        Destroy(child, childDestroyTime);
                    }
                    #endregion

                    child.transform.parent = null;
                }
            }
        }
        #endregion



        #region On contact with an ENEMY
        if (other.gameObject.tag == "Enemy") {
            //References & allow interaction
            StatusEffect statusEffect = other.gameObject.GetComponent<StatusEffect>();
            ElementInteractor elemInteraction = other.gameObject.GetComponent<ElementInteractor>();
            elemInteraction.allowInteraction = true;

            #region BURN
            if (burnEnemiesOnContact) {
                //Burn
                statusEffect.BurnCoroutine();

                //Set type
                elemInteraction.elementType = Element.Fire;
            }
            #endregion

            #region FREEZE
            if (freezeEnemiesOnContact) {
                //Freeze
                statusEffect.FreezeCoroutine();

                //Set type
                elemInteraction.elementType = Element.Ice;
            }
            #endregion
        }
        #endregion


        #region Destroy
        if (destroyOnContact) {
            DestroyGO(0.05f);
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

    IEnumerator DestroyGO(float delay) {
        yield return new WaitForSeconds(delay);

        if (transform.childCount > 0) {
            GameObject child = transform.GetChild(0).gameObject;
            child.transform.parent = null;
        }
        
        Destroy(gameObject);
    }
}
