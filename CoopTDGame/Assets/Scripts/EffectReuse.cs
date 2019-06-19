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
        PlayImpactSound();
    }

    public void PlayImpactSound()
    {
        if (_source == null) {
            _source = GetComponent<MultiAudioSource>();
        }
        else
        {
            if (_source.gameObject.activeSelf) {
                //if (_source != null)
                //{
                    if (explosionIndex == 0)
                        AudioManager.Instance.PlaySound(_source, Sound.towerDefaultImpact, true);
                    if (explosionIndex == 1)
                        AudioManager.Instance.PlaySound(_source, Sound.towerFireImpact, true);
                    if (explosionIndex == 2)
                        AudioManager.Instance.PlaySound(_source, Sound.towerIceImpact, true);
                    if (explosionIndex == 3)
                        AudioManager.Instance.PlaySound(_source, Sound.towerBlastImpact, true);
                //}
            }
        }  
    }

    public override void OnObjectReuse()
    {
        if(myParticleSystem)
        {
            myParticleSystem.Simulate(0.0f, true, true);
            myParticleSystem.Play();
            PlayImpactSound();
            //Invoke("HideSelf", 2);
        }
    }

    void HideSelf()
    {
        gameObject.SetActive(false);
    }

}
