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
    [SerializeField]
    private bool allowMovement = true;

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
        if (other.gameObject.tag == "Enemy" && appliedDamage == false)
        {
            if (allowDestroying) {
                Destroy(gameObject);
            }
        }

        else
        {
            if (allowDestroying) {
                Destroy(gameObject);
            }
        }
    }
}
