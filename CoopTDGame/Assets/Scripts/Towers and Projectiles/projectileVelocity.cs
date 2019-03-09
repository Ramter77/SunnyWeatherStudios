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
    

    // Start is called before the first frame update
    void Start()
    {
        lifetime = 4f;
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            targetDefense = other.GetComponent<LifeAndStats>().defense;
            float applyingDamage = damage - targetDefense / penetrationFactor; // calculates the damage for the 
            other.GetComponent<LifeAndStats>().health -= applyingDamage;
            Destroy(gameObject);
        }
    }
}
