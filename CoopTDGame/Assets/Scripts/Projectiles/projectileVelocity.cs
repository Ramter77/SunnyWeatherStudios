using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileVelocity : MonoBehaviour
{
    public float speed;
    private float lifetime = 4f;
    private bool appliedDamage = false;
    [SerializeField]
    private bool allowMovement = true;

    [SerializeField]
    private bool allowCollision = true;
    [SerializeField]
    private bool enableAndDetachImpactVFX;
    [SerializeField]
    private GameObject impactVFX;

    private bool allowDestroying;
    

    void Start()
    {
        if (GetComponent<Light>() == null) {
            if (transform.childCount > 0) {
                allowDestroying = true;
                Destroy(gameObject, lifetime);
            }
        }
    }

    void Update()
    {
        if (allowMovement) {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (allowCollision) {
        if (other.gameObject.tag == "Enemy" && appliedDamage == false)
        {
            if (allowDestroying) {
                Destroy(gameObject);
            }
        }

        else
        {
            if (allowDestroying) {
                if (enableAndDetachImpactVFX) {
                    //Enable impact VFX & unparent (the bowProjectile impact destroys after 1 sec)
                    impactVFX.SetActive(true);
                    impactVFX.transform.parent = null;
                }

                Destroy(gameObject);
            }
        }
        }
    }
}
