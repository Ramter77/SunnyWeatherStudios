using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    [Header("Effect Settings")]
    [Tooltip("Type of Element that is being used")]
    public Element usedElement = Element.NoElement;

    public int effectIndex = 0;

    private ParticleSystem myParticleSystem;

    private bool exploded = false;

    private void Start()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
        exploded = false;
        if (usedElement == Element.NoElement)
            effectIndex = 0;
        else if (usedElement == Element.Fire)
            effectIndex = 1;
        else if (usedElement == Element.Ice)
            effectIndex = 2;
        else if (usedElement == Element.Blast)
            effectIndex = 3;    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(exploded == false)
            {
                Vector3 offset = new Vector3(0,1,0);
                Vector3 spawnPoint = transform.position + offset;

                ExplosionManager.Instance.displayEffect(effectIndex, spawnPoint, this.transform.rotation);
                exploded = true;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (exploded == false)
        {
            Vector3 offset = new Vector3(0, 1, 0);
            Vector3 spawnPoint = transform.position + offset;
            ExplosionManager.Instance.displayEffect(effectIndex, spawnPoint, Quaternion.Euler(90f,0f,0f));
            exploded = true;
        }
    }

}
