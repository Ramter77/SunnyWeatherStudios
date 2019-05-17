using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebuffTrapSlow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<StatusEffect>().FreezeCoroutine();
        }
    }
}
