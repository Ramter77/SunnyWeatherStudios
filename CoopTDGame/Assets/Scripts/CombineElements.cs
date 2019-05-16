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


    private MeshRenderer meshRend;
    private Transform holderTransform;
    private BasicTower basicTowerScript;
    private ActivatePrefab activatePrefabScript;
    private bool isTrap, isTower;
    

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
                _SwitchBack(false);
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

    void _CombineElements(int element) {
        #region Combine with FIRE
        if (element == 1) {
            if (isTrap) {
                trapFireVFX.SetActive(true);

                matArray[1] = fireMat;
                meshRend.materials = matArray;
            }
            else if (isTower) {
                basicTowerScript.changeProjectile(1);

                startTowerCD = true;
            }
            crystalMeshRenderer.material = fireMat;
        }
        #endregion

        #region Combine with ICE
        else if (element == 2) {
            if (isTrap) {
                trapIceVFX.SetActive(true);

                matArray[1] = iceMat;
                meshRend.materials = matArray;
            }
            else if (isTower) {
                basicTowerScript.changeProjectile(2);

                startTowerCD = true;
            }
            crystalMeshRenderer.material = iceMat;
        }
        #endregion

        /* #region Combine with BLAST
        else if (element == 3) {
            if (isTrap) {
                blastVFX.SetActive(true);
            }
            else if (isTower) {
                basicTowerScript.changeProjectile(3);
            }
        }
        #endregion */
    }

    public void _SwitchBack(bool isTrap)
    {
        if (isTrap) {
            matArray[1] = baseMat;
            meshRend.materials = matArray;
        }
        else
        {
            basicTowerScript.changeProjectile(0);
        }
        crystalMeshRenderer.material = baseMat;
    }

    public void disableVFX() {
        if (trapFireVFX) {
            trapFireVFX.SetActive(false);
        }
        if (trapIceVFX) {
            trapIceVFX.SetActive(false);
        }
    }
}
