using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponDamage : MonoBehaviour
{
    public float attackDamage = 0f;
    
    void Start()
    {
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Weapon hit enemy");
            other.gameObject.GetComponent<LifeAndStats>().TakeDamage(attackDamage); //!public functions pls
            //Destroy(gameObject);
        }
    }
}
