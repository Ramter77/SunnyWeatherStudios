using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MultiAudioListener;

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
    private GameObject trapDefaultVFX;
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
    private MultiAudioSource audioSource;
    private MultiAudioSource ownAudioSource;
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

        audioSource = holderTransform.GetComponent<MultiAudioSource>();
        ownAudioSource = GetComponent<MultiAudioSource>();
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

    public void TryCombine(GameObject go) {
        //only when already activated
        if (activatePrefabScript.trapActive || activatePrefabScript.towerActive) {
            if (go.tag == projectileTag)
            {
                if (go.GetComponent<EffectHandler>() != null)
                {
                    int otherProjectileElementIndex = go.GetComponent<EffectHandler>().effectIndex;
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
                trapDefaultVFX.SetActive(false);
                trapFireVFX.SetActive(false);
                trapIceVFX.SetActive(false);
                trapBlastVFX.SetActive(true);

                AudioManager.Instance.PlaySound(audioSource, Sound.trapBlast, false);

                matArray[1] = blastMat;
            }
            else if (elem == Element.Fire) {
                trapDefaultVFX.SetActive(false);
                trapFireVFX.SetActive(true);

                AudioManager.Instance.PlaySound(audioSource, Sound.trapFire, false);
            
                matArray[1] = fireMat;
            }
            else if (elem == Element.Ice) {
                trapDefaultVFX.SetActive(false);
                trapIceVFX.SetActive(true);

                AudioManager.Instance.PlaySound(audioSource, Sound.trapIce, false);
            
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

            AudioManager.Instance.PlaySound(ownAudioSource, Sound.playerCombineBuildingBlast, false);
        }
        else if (elem == Element.Fire) {
            fireActive = true;
            crystalMeshRenderer.material = fireMat;

            AudioManager.Instance.PlaySound(ownAudioSource, Sound.playerCombineBuildingFire, false);
        }
        else if (elem == Element.Ice) {
            iceActive = true;
            crystalMeshRenderer.material = iceMat;

            AudioManager.Instance.PlaySound(ownAudioSource, Sound.playerCombineBuildingIce, false);
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
            if (trapDefaultVFX) {
                trapDefaultVFX.SetActive(true);
            }
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
