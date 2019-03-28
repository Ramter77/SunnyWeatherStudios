using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTrapSlow : MonoBehaviour
{
    public float reducedMovementSpeed = 2f;



    //* WORKS NICELY! :)  */
    //* How about multiplying with a value like a percentage? * 0.5 would be half movespeed? */    
    //* ensures movespeed is never 0 and we could also use it to speed up */  
    private float moveSpeedDefault = 5;
    public float moveSpeedMultiplier = 0.5f;

    /* private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<BasicEnemy>().agent.speed = reducedMovementSpeed;
        }
    } */

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<BasicEnemy>().agent.speed *= moveSpeedMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<BasicEnemy>().agent.speed = moveSpeedDefault;
        }
    }
}
