using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour {
    [SerializeField]
    private bool isFriendly;
    public float attackDamage = 10f;
    private GameObject lastHitEnemy = null;

    void Start()
    {
        lastHitEnemy = null;
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (isFriendly) {
            if (other.gameObject.tag == "Enemy" && other.gameObject != lastHitEnemy)
            {
                _WeaponDamage(other.gameObject);
            }
        }
        else {
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2") {
                _WeaponDamage(other.gameObject);
            }
        }
    }

    void _WeaponDamage(GameObject other) {
        lastHitEnemy = other;
        other.GetComponent<LifeAndStats>().TakeDamage(attackDamage);
        StartCoroutine(resetLastHitGO());
    }

    IEnumerator resetLastHitGO()
    {
        yield return new WaitForSeconds(.25f);
        lastHitEnemy = null;
    }
}
