using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{
    public Transform rig;
    public bool ragdollOnStart;
    private float defaultSpeed;
    private bool ragdollToggle = false;


    private Component[] ownColliders;
    private bool disabledRigColliders;
    private Component[] colliders;
    private Component[] rbs;
    private Animator anim;
    private EnemyAnim animScript;
    private BasicEnemy enemyScript;
    private AttackAndDamage dmgScript;
    private LifeAndStats lifeScript;
    private NavMeshAgent agent;
    private Rigidbody rb;



    //Transform[] allChildren;

    void Awake()
    {
        //Disable ragdoll colliders
        colliders = rig.GetComponentsInChildren<Collider>();
        foreach (Collider _col in colliders)
            _col.enabled = false;

        ownColliders = GetComponents<Collider>();
        foreach (Collider _col in ownColliders)
            _col.enabled = true;

        //Reenable needed colliders
        //transform.GetChild(2).GetComponent<SphereCollider>().enabled = true;

        rbs = rig.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody _rb in rbs)
            _rb.isKinematic = true;

        rb = GetComponent<Rigidbody>();
        //Set kinematic of main rigidbody to false after setting all to true
        rb.isKinematic = true;
        Debug.Log("SET TO STATICICIASICIAISFIAKFA SFJAJFMEMUAE");

        


        anim = GetComponent<Animator>();
        animScript = GetComponent<EnemyAnim>();
        enemyScript = GetComponent<BasicEnemy>();
        dmgScript = GetComponent<AttackAndDamage>();
        lifeScript = GetComponent<LifeAndStats>();
        
        agent = GetComponent<NavMeshAgent>();

        defaultSpeed = agent.speed;


        if (ragdollOnStart) {
            toggleRagdoll(true);
        }



        //Transform[] allChildren = GetComponentsInChildren<Transform>();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            ragdollToggle = !ragdollToggle;
            toggleRagdoll(ragdollToggle);
        }

        //rb.isKinematic = false;
    }

    /// <summary>
    /// Sets ragdoll parameters depending on param
    /// </summary>
    /// <param name="ragdoll">Param to enable or disable ragdoll</param>
    public void toggleRagdoll(bool ragdoll) {
        //rig.GetComponent<NavMeshObstacle>().enabled = true;
        
        //if ragdoll is true --> disable scripts..
        anim.enabled = !ragdoll;
        animScript.enabled = !ragdoll;
        enemyScript.enabled = !ragdoll;
        dmgScript.enabled = !ragdoll;
        lifeScript.enabled = !ragdoll;
        agent.enabled = !ragdoll;
        
        //and set components
        rb.useGravity = ragdoll;
        rb.isKinematic = !ragdoll;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        //rb.freezeRotation = ragdoll;

        if (ragdoll) {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        else {
            rb.constraints = RigidbodyConstraints.None;
        }

        if (!disabledRigColliders) {
            foreach (Collider _col in colliders)
                _col.enabled = true;
        }

        foreach (Rigidbody _rb in rbs)
            _rb.isKinematic = false;


        //Disable attack indicator
        if (transform.childCount > 1) {
            transform.GetChild(2).gameObject.SetActive(false);
        }
        //Destroy(transform.GetChild(2).gameObject);


        //ownColliders = GetComponents<Collider>();
        foreach (Collider _col in ownColliders)
            _col.enabled = false;
        /* gameObject.layer = 15;
        foreach (Transform child in allChildren) {
            child.gameObject.layer = 15;
        } */



        /* if (ragdoll) {
            agent.speed = 0;
        }
        else {
            agent.speed = defaultSpeed;
        } */


        //GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
    }


    public void disableAllColliders() {
        /* ownColliders = GetComponents<Collider>();
        foreach (Collider _col in ownColliders)
            _col.enabled = false; */
        
        if (!disabledRigColliders) {
            disabledRigColliders = true;
            foreach (Collider _col in colliders)
                _col.enabled = false;

            /* Component[] capsuleColliders = rig.GetComponentsInChildren<CapsuleCollider>();
            foreach (CapsuleCollider _col in capsuleColliders)
            {
                //if (_col is BoxCollider) {} else {
                //    _col.enabled = false;
                //} 

                _col.radius *= 0.25f;
                _col.height *= 0.5f;
            }
            Component[] sphereColliders = rig.GetComponentsInChildren<SphereCollider>();
            foreach (SphereCollider _col in sphereColliders)
            {
                _col.radius *= 0.25f;
            }
            Component[] boxColliders = rig.GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider _col in boxColliders)
            {
                _col.size = new Vector3(_col.size.x*0.5f, _col.size.y*0.5f, _col.size.z*0.5f);
            } */
        }
        //! JUST put on non collision layer
        //foreach (GameObject gameObject in GetComponentInChildren<Transform>())

        /* gameObject.layer = 15;
        foreach (Transform child in allChildren) {
            child.gameObject.layer = 15;
        } */

        BoxCollider boxCollider = rig.GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        //Half torso box collider
        /* boxCollider.size = new Vector3(boxCollider.size.x*0.5f, boxCollider.size.y*0.5f, boxCollider.size.z*0.8f); */        

        /* mainCollider.radius *= 0.25f;
        mainCollider.height *= 0.5f; */
    }
}
