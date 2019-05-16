using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileVelocity : MonoBehaviour
{
    public float speed;
    private float lifetime = 4f;
    public float damage = 20f;
    public float penetrationFactor = 10f;
    private float targetDefense;
    private bool appliedDamage = false;

    private bool allowDestroying;
    

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Light>() == null) {
            if (transform.childCount > 0) {
                allowDestroying = true;
                Destroy(gameObject, lifetime);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && appliedDamage == false)
        {
            targetDefense = other.GetComponent<LifeAndStats>().defense;
            float applyingDamage = damage - targetDefense / penetrationFactor; // calculates the damage for the 
            other.GetComponent<LifeAndStats>().TakeDamage(applyingDamage);
            appliedDamage = true;
            if (allowDestroying) {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.tag == "Environment")
        {
            if (allowDestroying) {
                Destroy(gameObject);
            }
        }
    }
}
