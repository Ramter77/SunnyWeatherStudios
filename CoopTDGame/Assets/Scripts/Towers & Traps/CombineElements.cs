using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineElements : MonoBehaviour
{
    [Tooltip("Tag used for the projectiles of players")]
    [SerializeField] private string projectileTag = "ElementProjectile";

    [Header ("Parameters")]
    public float towerCD = 10;
    private bool startTowerCD;
    private float towerDuration;

    [Header ("VFX")]
    [SerializeField]
    private GameObject trapFireVFX;
    [SerializeField]
    private GameObject trapIceVFX;
    [SerializeField]
    private GameObject trapBlastVFX;

    [Header ("Materials")]
    [SerializeField]
    private GameObject crystalObject;
    private MeshRenderer crystalMeshRenderer;
    private Material[] matArray;
    private Material baseMat;
    [SerializeField]
    private Material fireMat;
    [SerializeField]
    private Material iceMat;
    [SerializeField]
    private Material blastMat;


    private MeshRenderer meshRend;
    private Transform holderTransform;
    private BasicTower basicTowerScript;
    private ActivatePrefab activatePrefabScript;
    private AudioSource audioSource;
    private bool isTrap, isTower;
    private bool fireActive, iceActive, blastActive;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        holderTransform = transform.parent.parent;

        if (holderTransform.GetComponent<BasicTower>() != null) {
            basicTowerScript = holderTransform.GetComponent<BasicTower>();
            isTower = true;
        }
        else
        {
            isTrap = true;
        }

        audioSource = holderTransform.GetComponent<AudioSource>();
        activatePrefabScript = holderTransform.GetComponent<ActivatePrefab>();
        crystalMeshRenderer = crystalObject.GetComponent<MeshRenderer>();
        meshRend = GetComponent<MeshRenderer>();
        matArray = meshRend.materials;
        baseMat = crystalMeshRenderer.material;
    }

    private void Update() {
        if (startTowerCD) {
            towerDuration -= Time.deltaTime;
            if (towerDuration <= 0.0f) {
                _SwitchBack();
            }
        }
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        //only when already activated
        if (activatePrefabScript.trapActive || activatePrefabScript.towerActive) {
            if (other.gameObject.tag == projectileTag)
            {
                if (other.gameObject.GetComponent<EffectHandler>() != null)
                {
                    int otherProjectileElementIndex = other.gameObject.GetComponent<EffectHandler>().effectIndex;
                    _CombineElements(otherProjectileElementIndex);
                }
            }
        }
    }

    void Combine(Element elem) {
        //Trap
        if (isTrap) {
            if (elem == Element.Blast) 
            {
                trapFireVFX.SetActive(false);
                trapIceVFX.SetActive(false);
                trapBlastVFX.SetActive(true);

                AudioManager.Instance.PlaySound(audioSource, AudioManager.Instance.trapBlast);

                matArray[1] = blastMat;
            }
            else if (elem == Element.Fire) {
                trapFireVFX.SetActive(true);

                AudioManager.Instance.PlaySound(audioSource, AudioManager.Instance.trapFire);
            
                matArray[1] = fireMat;
            }
            else if (elem == Element.Ice) {
                trapIceVFX.SetActive(true);

                AudioManager.Instance.PlaySound(audioSource, AudioManager.Instance.trapIce);
            
                matArray[1] = iceMat;
            }

            meshRend.materials = matArray;
        }
        //Tower
        else
        {
            if (elem == Element.Blast) 
            {
                basicTowerScript.changeProjectile(3);
            }
            else if (elem == Element.Fire) {
                basicTowerScript.changeProjectile(1);
            }
            else if (elem == Element.Ice) {
                basicTowerScript.changeProjectile(2);
            }

            startTowerCD = true;
        }

        //GENERAL (crystal mesh material & bools)
        if (elem == Element.Blast) {
            blastActive = true;
            crystalMeshRenderer.material = blastMat;
        }
        else if (elem == Element.Fire) {
            fireActive = true;
            crystalMeshRenderer.material = fireMat;
        }
        else if (elem == Element.Ice) {
            iceActive = true;
            crystalMeshRenderer.material = iceMat;
        }
    }

    void _CombineElements(int element) {
        if (!blastActive) {
            //Fire
            if (element == 1) {
                if (iceActive) {
                    Combine(Element.Blast);
                }
                else
                {
                    Combine(Element.Fire);
                }
            }
            //Ice
            else if (element == 2) {
                if (fireActive) {
                    Combine(Element.Blast);
                }
                else
                {
                    Combine(Element.Ice);
                }
            }
        }
    }

    public void _SwitchBack()
    {
        audioSource.Stop();

        if (isTrap) {
            //disable all vfx
            if (trapFireVFX) {
                trapFireVFX.SetActive(false);
            }
            if (trapIceVFX) {
                trapIceVFX.SetActive(false);
            }
            if (trapBlastVFX) {
                trapBlastVFX.SetActive(false);
            }

            //switch back to baseMat
            matArray[1] = baseMat;
            meshRend.materials = matArray;
        }
        else
        {
            //switch back to default projectile & reset towerCD
            basicTowerScript.changeProjectile(0);

            towerDuration = towerCD;
            startTowerCD = false;
        }
        //switch crystal back to baseMat
        crystalMeshRenderer.material = baseMat;

        fireActive = false;
        iceActive = false;
        blastActive = false;
    }
}
