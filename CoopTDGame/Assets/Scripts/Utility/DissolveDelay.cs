using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveDelay : MonoBehaviour
{
    [SerializeField]
    private Material dissolveMaterial;

    [SerializeField]
    private float dissolveDelay = 1f;
    [SerializeField]
    private float destroyDelay = 2f;


    private MeshRenderer meshRenderer;
    private Material currentMaterial;
    

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        currentMaterial = meshRenderer.material;

        //Switch to dissolve material after dissolveDelay
        StartCoroutine(dissolveAfterDelay(dissolveDelay));
    }

    void Update()
    {
        
    }

    IEnumerator dissolveAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);

        meshRenderer.material = dissolveMaterial;

        //Destroy after destroyDelay
        StartCoroutine(destroyAfterDelay(destroyDelay));
    }

    IEnumerator destroyAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
