using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponDamage : MonoBehaviour
{
    public float attackDamage = 0f;
    private GameObject lastHit = null;

    void Start()
    {
        lastHit = null;
        
    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.gameObject.tag == "Enemy" && other.gameObject != lastHit)
        {
            Debug.Log("Weapon hit enemy");
            lastHit = other.gameObject;
            other.gameObject.GetComponent<LifeAndStats>().TakeDamage(attackDamage); //!public functions pls
            StartCoroutine(resetLastHitGO());
        }
    }

    IEnumerator resetLastHitGO()
    {
        yield return new WaitForSeconds(.1f);
        lastHit = null;
    }
}
