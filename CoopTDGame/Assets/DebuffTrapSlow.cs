using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTrapSlow : MonoBehaviour
{
    public float reducedMovementSpeed = 2f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<BasicEnemy>().agent.speed = reducedMovementSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<BasicEnemy>().agent.speed = reducedMovementSpeed;
        }
    }
}
