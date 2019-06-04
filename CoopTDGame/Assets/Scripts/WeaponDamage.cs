using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MultiAudioListener;

public class WeaponDamage : MonoBehaviour {
    [SerializeField]
    private bool isFriendly;
    [SerializeField]
    private bool isProjectile;
    public float attackDamage = 10f;
    [SerializeField]
    private float resetLastHitGOdelay = 0.25f;
    private GameObject lastHitEnemy = null;
    private MultiAudioSource audioSource;

    void Start()
    {
        lastHitEnemy = null;

        if (!isProjectile) {
            audioSource = GetComponent<MultiAudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isFriendly) {
            if (other.gameObject.tag == "Enemy" && other.gameObject != lastHitEnemy)
            {
                _WeaponDamage(other.gameObject, false);
            }
            
        }
        else {
            if (other.gameObject.layer == 21)
            {
                _WeaponDamage(other.gameObject, true);
            }
            else if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2") {
                _WeaponDamage(other.gameObject, false);
            }
            else if (other.gameObject.tag == "Sphere")
            {
                _WeaponDamage(other.gameObject, false);
            }
        }
    }

    void _WeaponDamage(GameObject other, bool toTower) {
        lastHitEnemy = other;

        if (toTower) {
            other.transform.parent.transform.parent.GetComponent<LifeAndStats>().TakeDamage(attackDamage);
        }
        else
        {
            other.GetComponent<LifeAndStats>().TakeDamage(attackDamage);
        }

        if (!isProjectile) {
            AudioManager.Instance.PlaySound(audioSource, Sound.meleeImpact, true);
        }

        StartCoroutine(resetLastHitGO());
    }

    IEnumerator resetLastHitGO()
    {
        yield return new WaitForSeconds(resetLastHitGOdelay);
        lastHitEnemy = null;
    }
}
