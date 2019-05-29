using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MultiAudioListener;

public class projectileVelocity : MonoBehaviour
{
    
    
    public float speed;
    private float lifetime = 4f;
    private bool appliedDamage = false;
    [SerializeField]
    private bool allowMovement = true;

    [SerializeField]
    private bool allowCollision = true;
    [SerializeField]
    private bool enableAndDetachImpactVFX;
    [SerializeField]
    private GameObject impactVFX;

    private bool allowDestroying;

    #region Sound
    public bool playSoundOnStart = true;
    public Sound sound;
    private MultiAudioSource audioSource;
    #endregion

    void Start()
    {
        if (GetComponent<Light>() == null) {
            if (transform.childCount > 0) {
                allowDestroying = true;
                Destroy(gameObject, lifetime);
            }
        }

        audioSource = GetComponent<MultiAudioSource>();

        if (playSoundOnStart) {
            if (sound != Sound.empty) {
                AudioManager.Instance.PlaySound(audioSource, sound);
            }
        }
    }

    void Update()
    {
        if (allowMovement) {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (allowCollision) {
        if (other.gameObject.tag == "Enemy" && appliedDamage == false)
        {
            if (allowDestroying) {
                Destroy(gameObject);
            }
        }

        else
        {
            if (allowDestroying) {
                if (enableAndDetachImpactVFX) {
                    //Enable impact VFX & unparent (the bowProjectile impact destroys after 1 sec)
                    impactVFX.SetActive(true);
                    impactVFX.transform.parent = null;
                }

                Destroy(gameObject);
            }
        }
        }
    }
}
