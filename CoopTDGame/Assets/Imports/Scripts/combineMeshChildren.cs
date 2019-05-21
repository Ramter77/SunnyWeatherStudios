using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combineMeshChildren : MonoBehaviour
{
    public bool combineMeshesOnStart;
    public bool addMeshCollider;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if (combineMeshesOnStart) { combineMeshes(gameObject); }
    }

    void combineMeshes(GameObject obj)
    {
        //Zero transformation is needed because of localToWorldMatrix transform
        Vector3 position = obj.transform.position;
        obj.transform.position = Vector3.zero;
        
        //whatever man
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        obj.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        obj.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true);
        
        //! Later destroy them here
        obj.transform.gameObject.SetActive(true);
        
        //Reset position
        obj.transform.position = position;

        //Adds collider to mesh
        if (addMeshCollider) { obj.AddComponent<MeshCollider> (); }
    }
}
