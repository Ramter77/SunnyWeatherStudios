using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    private SphereCollider damageCollider;
    [SerializeField] private float damageRadius;
    [SerializeField] private float damage = 100f;
    ParticleSystem myParticleSystem;

    private bool invoked = false;

    // Start is called before the first frame update
    void Start()
    {
        damageCollider = GetComponent<SphereCollider>();
        damageCollider.radius = damageRadius;
        myParticleSystem = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(other.GetComponent<LifeAndStats>() != null)
            {
                other.GetComponent<LifeAndStats>().TakeDamage(damage);
            }
        }
    }
    private void setDeactive()
    {
        gameObject.SetActive(false);
        invoked = false;
    }

    private void Update()
    {
        if(myParticleSystem.isPlaying == false && invoked == false)
        {
            invoked = true;
            Invoke("setDeactive", 1f);
        }
    }
}
