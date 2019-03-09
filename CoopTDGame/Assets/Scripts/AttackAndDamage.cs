using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAndDamage : MonoBehaviour
{
    [Header("Attack Stats")]
    public float damage = 20f; // flat damage amount
    private float attackSpeed = 2f; // 1f = 1 attack per second ; 2f = 1 attack every 2 seconds ; ...
    private float penetrationFactor = 3f; // factor that recudes the amount of damage reduction the target recives through its defense stat

    [Header("Target")]
    public GameObject Target = null;
    public bool enableAttack = true;

    [Header("Damage Calculation")]
    public float targetDefense = 0f;


    // Start is called before the first frame update
    void Start()
    {
        enableAttack = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void performAttack()
    {
        if (enableAttack)
        {
            targetDefense = Target.GetComponent<LifeAndStats>().defense;
            float applyingDamage = damage - targetDefense / penetrationFactor; // calculates the damage for the 
            Target.GetComponent<LifeAndStats>().health -= applyingDamage;
            //Debug.Log("AI: apply damage amount" + Target.GetComponent<LifeAndStats>().health);
            enableAttack = false;
            StartCoroutine(resetAttackCooldown());

            //AttackAnimation
            //SetTrigger("Attack");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("I see the player");

        }
    }
    IEnumerator resetAttackCooldown()
    {
        yield return new WaitForSeconds(attackSpeed);
        enableAttack = true;
    }

}
