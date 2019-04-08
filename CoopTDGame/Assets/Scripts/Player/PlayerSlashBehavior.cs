using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashBehavior : MonoBehaviour
{
    public float slashDamage;
    public float lifetime = 5f;
    public float speed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<LifeAndStats>().health -= slashDamage;
        }
        if(other.gameObject.tag == "Environment")
        {
            Destroy(gameObject);
        }
    }
}
