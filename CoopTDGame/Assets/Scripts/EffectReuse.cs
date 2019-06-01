using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MultiAudioListener;

public class EffectReuse : PoolObject
{
    ParticleSystem myParticleSystem;
    [Tooltip("0 = normal; 1 = Fire; 2 = Ice; 3 = Blast")] public int explosionIndex;
    private MultiAudioSource _source;

    private void Start()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
        _source = GetComponent<MultiAudioSource>();
        if (_source != null)
        {
            if (explosionIndex == 0)
                AudioManager.Instance.PlaySound(_source, Sound.towerDefaultImpact);
            if (explosionIndex == 1)
                AudioManager.Instance.PlaySound(_source, Sound.towerFireImpact);
            if (explosionIndex == 2)
                AudioManager.Instance.PlaySound(_source, Sound.towerIceImpact);
            if (explosionIndex == 3)
                AudioManager.Instance.PlaySound(_source, Sound.towerBlastImpact);
        }
    }

    public void PlaySound()
    {
        _source = GetComponent<MultiAudioSource>();
        if (_source != null)
        {
            if (explosionIndex == 0)
                AudioManager.Instance.PlaySound(_source, Sound.towerDefaultImpact);
            if (explosionIndex == 1)
                AudioManager.Instance.PlaySound(_source, Sound.towerFireImpact);
            if (explosionIndex == 2)
                AudioManager.Instance.PlaySound(_source, Sound.towerIceImpact);
            if (explosionIndex == 3)
                AudioManager.Instance.PlaySound(_source, Sound.towerBlastImpact);
        }
    }

    public override void OnObjectReuse()
    {
        if(myParticleSystem)
        {
            myParticleSystem.Simulate(0.0f, true, true);
            myParticleSystem.Play();
            PlaySound();
            //Invoke("HideSelf", 2);
        }
    }

    void HideSelf()
    {
        gameObject.SetActive(false);
    }

}
