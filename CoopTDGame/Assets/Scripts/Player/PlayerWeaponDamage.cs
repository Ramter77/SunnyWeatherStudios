using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponDamage : MonoBehaviour
{
    public float attackDamage = 0f;
    private GameObject lastHitEnemy = null;

    void Start()
    {
        lastHitEnemy = null;
    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.gameObject.tag == "Enemy" && other.gameObject != lastHitEnemy)
        {
            //Debug.Log("Weapon hit enemy");
            lastHitEnemy = other.gameObject;
            other.gameObject.GetComponent<LifeAndStats>().TakeDamage(attackDamage);
            StartCoroutine(resetLastHitGO());
        }
    }

    IEnumerator resetLastHitGO()
    {
        yield return new WaitForSeconds(.25f);
        lastHitEnemy = null;
    }
}
