using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
public class AddMaterialToSelectedGameObjectsScript : MonoBehaviour
{
    //Script gets executed on button press of the AddMaterialToSelectedGameObjectsEditor script
    [Header ("Material")]
    public Material material;
    public bool addMeshRenderer = true;
    private MeshRenderer meshRenderer;

    public void AddMaterialToSelectedGameObjects()
    {
        GameObject[] gameObjects = Selection.gameObjects;
        foreach (GameObject go in gameObjects)
        {
            #region MeshRenderer & Material
            meshRenderer = go.GetComponent<MeshRenderer>();
            //If no meshRenderer then add one if enabled
            if (!meshRenderer) {
                if (addMeshRenderer) {
                    meshRenderer = go.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
                }
            }

            if (material) {
                meshRenderer.material = material;
            }
            #endregion

            //Remove this script
            DestroyImmediate(go.GetComponent<AddMaterialToSelectedGameObjectsScript>());
        }
    }  
}