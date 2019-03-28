using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windProjectile : MonoBehaviour
{
    public float windDamage = 0f;
    public Vector3 knockbackForce;
    private float lifetime = 4f;
    public float speed = 20f;

    // Start is called before the first frame update
    void Start()
    {
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
            other.gameObject.GetComponent<LifeAndStats>().health -= windDamage;
            Destroy(gameObject);
        }
    }
}
