using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineElements : MonoBehaviour
{
    [Tooltip("Tag used for the projectiles of players")]
    [SerializeField] private string projectileTag = "ElementProjectile";

    [Header ("VFX")]
    [SerializeField]
    private GameObject trapFireVFX;
    [SerializeField]
    private GameObject trapIceVFX;

    [Header ("Materials")]
    [SerializeField]
    private GameObject crystalObject;
    private MeshRenderer crystalMeshRenderer;
    [SerializeField]
    private Material fireMat;
    [SerializeField]
    private Material iceMat;


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
            }
            else if (isTower) {
                basicTowerScript.changeProjectile(1);
            }
            crystalMeshRenderer.material = fireMat;
        }
        #endregion

        #region Combine with ICE
        else if (element == 2) {
            if (isTrap) {
                trapIceVFX.SetActive(true);
            }
            else if (isTower) {
                basicTowerScript.changeProjectile(2);
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

    public void disableVFX() {
        if (trapFireVFX) {
            trapFireVFX.SetActive(false);
        }
        if (trapIceVFX) {
            trapIceVFX.SetActive(false);
        }
    }
}
