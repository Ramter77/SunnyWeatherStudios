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
    private bool isTrap, isTower;
    private bool fireActive, iceActive, blastActive;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
        matArray = meshRend.materials;

        holderTransform = transform.parent.parent;

        if (holderTransform.GetComponent<BasicTower>() != null) {
            basicTowerScript = holderTransform.GetComponent<BasicTower>();
            isTower = true;
        }
        else
        {
            isTrap = true;
        }

        activatePrefabScript = holderTransform.GetComponent<ActivatePrefab>();
        crystalMeshRenderer = crystalObject.GetComponent<MeshRenderer>();
        baseMat = crystalMeshRenderer.material;
    }

    private void Update() {
        if (startTowerCD) {
            towerDuration -= Time.deltaTime;
            if (towerDuration <= 0.0f) {
                towerDuration = towerCD;
                startTowerCD = false;
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

                matArray[1] = blastMat;
            }
            else if (elem == Element.Fire) {
                trapFireVFX.SetActive(true);
            
                matArray[1] = fireMat;
            }
            else if (elem == Element.Ice) {
                trapIceVFX.SetActive(true);
            
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
            //Blast
            if (fireActive || iceActive) {
                Combine(Element.Blast);

                Debug.Log("BLASSSSSSSSSSSSSSSSSST");
            }
            //Fire&Ice
            else
            {
                if (element == 1) {
                    Combine(Element.Fire);
                }
                else if (element == 2)
                {
                    Combine(Element.Ice);
                }
            }
        }
    }

    public void _SwitchBack()
    {
        if (isTrap) {
            if (trapFireVFX) {
                trapFireVFX.SetActive(false);
            }
            if (trapIceVFX) {
                trapIceVFX.SetActive(false);
            }
            if (trapBlastVFX) {
                trapBlastVFX.SetActive(false);
            }

            matArray[1] = baseMat;
            meshRend.materials = matArray;
        }
        else
        {
            basicTowerScript.changeProjectile(0);
        }
        crystalMeshRenderer.material = baseMat;

        fireActive = false;
        iceActive = false;
        blastActive = false;
    }
}
