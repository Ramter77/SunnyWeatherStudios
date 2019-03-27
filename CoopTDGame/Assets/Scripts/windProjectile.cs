using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windProjectile : MonoBehaviour
{
    public float windDamage = 0f;
    public Vector3 knockbackForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<LifeAndStats>().health -= windDamage;
            collision.gameObject.GetComponent<Rigidbody>().velocity = knockbackForce;
        }
    }
}
