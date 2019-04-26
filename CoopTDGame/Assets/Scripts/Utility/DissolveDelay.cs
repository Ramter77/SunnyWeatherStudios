using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveDelay : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer weaponRenderer; 

    [SerializeField]
    private float delay = 4f;
    private float lerp;

    [SerializeField]    
    private bool dissolveOnStart;
    private bool startDissolving;

    private MeshRenderer meshRenderer;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        skinnedMeshRenderer.material.SetFloat("_Dissolve", 0);

        if (weaponRenderer != null) {
            weaponRenderer.material.SetFloat("_Dissolve", 0);
        }


        if (dissolveOnStart) {
            Dissolve();
        }
    }

    void Update() {
        if (startDissolving) {
            skinnedMeshRenderer.material.SetFloat("_Dissolve", Mathf.Lerp(0, 1, lerp));
            if (weaponRenderer != null) {
                weaponRenderer.material.SetFloat("_Dissolve", Mathf.Lerp(0, 1, lerp));
            }

            if (lerp < 1) { //While lerp is below the end limit
                //Increment it at the desired rate every frame
                lerp += Time.deltaTime/delay;
            }
        }
    }

    public void Dissolve() {
        startDissolving = true;

        //Destroy after delay time when completely dissolved
        Destroy(gameObject, delay + 0.1f);
        Destroy(weaponRenderer.gameObject, delay + 0.1f);
    }
}
