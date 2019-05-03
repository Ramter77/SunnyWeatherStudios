using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveDelay : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer weaponRenderer; 

    [SerializeField]
    private float startDelay = 0f;
    [SerializeField]
    private float delay = 4f;
    private float lerp;

    [SerializeField]    
    private bool useMeshRenderer = true;
    [SerializeField]    
    private bool dissolveOnStart;
    [SerializeField]    
    private bool sinkWhileDissolving;
    [SerializeField]    
    private float sinkAmount = 1f;
    private bool startDissolving;

    private MeshRenderer meshRenderer;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    void Start()
    {
        if (useMeshRenderer) {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        else {
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }
        
        /* skinnedMeshRenderer.material.SetFloat("_Dissolve", 0);

        if (weaponRenderer != null) {
            weaponRenderer.material.SetFloat("_Dissolve", 0);
        } */


        if (dissolveOnStart) {
            DissolveCoroutine();
        }
    }

    void Update() {
        if (startDissolving) {
            if (useMeshRenderer) {
                meshRenderer.material.SetFloat("_Dissolve", Mathf.Lerp(0, 1, lerp));
            }
            else {
                skinnedMeshRenderer.material.SetFloat("_Dissolve", Mathf.Lerp(0, 1, lerp));
            }

            if (weaponRenderer != null) {
                weaponRenderer.material.SetFloat("_Dissolve", Mathf.Lerp(0, 1, lerp));
            }

            //transform.position = new Vector3(transform.position.x, Mathf.Lerp(0, sinkAmount, lerp), transform.position.z);

            if (lerp < 1) { //While lerp is below the end limit
                //Increment it at the desired rate every frame
                lerp += Time.deltaTime/delay;
            }
        }
    }

    public void DissolveCoroutine() {
        StartCoroutine(Dissolve());
    }

    private IEnumerator Dissolve() {
        yield return new WaitForSeconds(startDelay);
        startDissolving = true;

        //Disable colliders
        if (GetComponent<Ragdoll>() != null) {
            GetComponent<Ragdoll>().disableAllColliders();
        }

        //Destroy after delay time when completely dissolved
        if (weaponRenderer != null) {
            Destroy(weaponRenderer.gameObject, delay + 0.1f);
        }
        Destroy(gameObject, delay + 0.1f);
    }
}
