using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectReuse : PoolObject
{
    ParticleSystem myParticleSystem;

    private void Start()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
    }

    public override void OnObjectReuse()
    {
        if(myParticleSystem)
        {
            myParticleSystem.Simulate(0.0f, true, true);
            myParticleSystem.Play();
            //Invoke("HideSelf", 2);
        }
    }

    void HideSelf()
    {
        gameObject.SetActive(false);
    }

}
