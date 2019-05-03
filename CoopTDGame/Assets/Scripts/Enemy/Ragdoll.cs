using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{
    public Transform rig;

    public bool ragdollOnStart;

    private Component[] ownColliders;
    private bool disabledRigColliders;
    private Component[] colliders;
    private Component[] rbs;
    


    Animator anim;
    EnemyAnim animScript;
    BasicEnemy enemyScript;
    AttackAndDamage dmgScript;
    LifeAndStats lifeScript;
    NavMeshAgent agent;
    Rigidbody rb;


    private float defaultSpeed;

    // Start is called before the first frame update
    void Start()
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


        rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody _rb in rbs)
            _rb.isKinematic = true;


        anim = GetComponent<Animator>();
        animScript = GetComponent<EnemyAnim>();
        enemyScript = GetComponent<BasicEnemy>();
        dmgScript = GetComponent<AttackAndDamage>();
        lifeScript = GetComponent<LifeAndStats>();
        rb = GetComponent<Rigidbody>();

        agent = GetComponent<NavMeshAgent>();


        defaultSpeed = agent.speed;


        if (ragdollOnStart) {
            toggleRagdoll(true);
        }
    }

    /// <summary>
    /// Sets ragdoll parameters depending on param
    /// </summary>
    /// <param name="ragdoll">Param to enable or disable ragdoll</param>
    public void toggleRagdoll(bool ragdoll) {
        rig.GetComponent<NavMeshObstacle>().enabled = true;
        

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
        transform.GetChild(2).gameObject.SetActive(false);
        //Destroy(transform.GetChild(2).gameObject);
        

        /* if (ragdoll) {
            agent.speed = 0;
        }
        else {
            agent.speed = defaultSpeed;
        } */
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

        BoxCollider boxCollider = rig.GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        //Half torso box collider
        boxCollider.size = new Vector3(boxCollider.size.x*0.5f, boxCollider.size.y, boxCollider.size.z*0.5f);

        

        /* mainCollider.radius *= 0.25f;
        mainCollider.height *= 0.5f; */
    }
}
