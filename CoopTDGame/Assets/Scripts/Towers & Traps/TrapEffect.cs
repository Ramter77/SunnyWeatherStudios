using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEffect : MonoBehaviour
{
    public float radius;
    public Element elem;
    private SphereCollider coll;
    public float damageTime = 5f;
    private float fallbackTime = 1f;
    public float damage;
    public bool applyDamage = false;
    public bool started = false;

    void Start()
    {
        coll = GetComponent<SphereCollider>();
        coll.radius = radius;
        fallbackTime = damageTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy")
        {
            if (elem == Element.Fire) {
                other.gameObject.GetComponent<StatusEffect>().BurnCoroutine();
            }

            else if (elem == Element.Ice) {
                other.gameObject.GetComponent<StatusEffect>().FreezeCoroutine();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && applyDamage == true)
        {
            other.gameObject.GetComponent<LifeAndStats>().TakeDamage(damage);
        }
    }



    private void Update()
    {
        if(elem == Element.NoElement)
        {
            damageTime -= Time.deltaTime;
            if (damageTime <= 0 && started == false)
            {
                applyDamage = true;
                StartCoroutine(resetDamageGiven());
                started = true;
            }
        }
    }
    IEnumerator resetDamageGiven()
    {
        yield return new WaitForSeconds(.1f);
        damageTime = fallbackTime;
        applyDamage = false;
        started = false;
    }


}
