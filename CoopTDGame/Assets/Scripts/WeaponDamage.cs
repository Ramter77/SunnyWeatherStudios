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
                _WeaponDamage(other.gameObject);
            }
        }
        else {
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2") {
                _WeaponDamage(other.gameObject);
            }
            if(other.gameObject.tag == "Sphere")
            {
                _WeaponDamage(other.gameObject);
            }
        }
    }

    void _WeaponDamage(GameObject other) {
        lastHitEnemy = other;
        other.GetComponent<LifeAndStats>().TakeDamage(attackDamage);

        if (!isProjectile) {
            AudioManager.Instance.PlaySound(audioSource, Sound.meleeImpact);
        }

        StartCoroutine(resetLastHitGO());
    }

    IEnumerator resetLastHitGO()
    {
        yield return new WaitForSeconds(resetLastHitGOdelay);
        lastHitEnemy = null;
    }
}
