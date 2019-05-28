using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEffect : MonoBehaviour
{
    public float radius;
    public Element elem;
    private SphereCollider coll;

    void Start()
    {
        coll = GetComponent<SphereCollider>();
        coll.radius = radius;
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
}
