using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AbilityDrop : MonoBehaviour
{
    [Header("Abilities")]
    [Tooltip("List of all Abilities available")]
    public GameObject[] Abilities = null;

    [Header("Spawn Loaction")]
    [Tooltip("Center point used to calculate the new spawn location")]
    [SerializeField] private Vector3 centerPoint = Vector3.zero;
    [Tooltip("temporary location from which the ray will check for the spawn location")]
    [SerializeField] private Vector3 checkPos = Vector3.zero;
    [Space(10)]
    [Tooltip("Minimum coordinate on X-Axis that the ability can spawn on")]
    [SerializeField] private float xMin = 0f;
    [Tooltip("Maximum coordinate on X-Axis that the ability can spawn on")]
    [SerializeField] private float xMax = 0f;
    [Tooltip("Minimum coordinate on Z-Axis that the ability can spawn on")]
    [SerializeField] private float zMin = 0f;
    [Tooltip("Maximum coordinate on Z-Axis that the ability can spawn on")]
    [SerializeField] private float zMax = 0f;
    [Space(20)]
    [Tooltip("Offset for the actual spawn of the ability (so it does not spawn inside the ground)")]
    [SerializeField] private Vector3 spawnOffset = Vector3.zero;
    [Space(10)]
    [Tooltip("returns true when the ray found a possible Spawn-Location")]
    [SerializeField] private bool possibleSpawnpointFound = false;
    [SerializeField] private Vector3 possibleSpawnpoint = Vector3.zero;

    [Header("Spawn Cycle")]
    [Tooltip("Time between each spawn")]
    public float abilitySpawnTime = 5f;
    [Tooltip("How many abilities should appear at the same time")]
    public int amountOfAbilitiesToSpawn = 1;
    private float fallbackAbilitySpawnTime = 5f; // used to reset the spawn time after each spawn

    [Header("General Settings")]
    [Tooltip("Layer on which you want to spawn the ability")]
    [SerializeField] private LayerMask groundLayer = 0;
    [SerializeField] private float rayCheckLength = 10f;

    [Header("Gizmo settings")]
    [Tooltip("To represent the area in which an ability can spawn, you need to multiply the values of xMax or zMax")]
    [SerializeField] private Vector3 gizmoCubeSize = Vector3.zero;
    

    private void Start()
    {
        // assign objects / equal floats etc
        centerPoint = this.transform.position;
        fallbackAbilitySpawnTime = abilitySpawnTime;
        // call functions
        FindNewSpawnLocation();
        StartCoroutine(AbilitySpawnCycle());
        // gizmos
        gizmoCubeSize = new Vector3(xMax * 2, 1f, zMax * 2);

    }

    /// <summary>
    /// finds a random loaction on the wished ground layer that can then be used to 
    /// spawn the abilities
    /// </summary>
    public void FindNewSpawnLocation()
    {
        if(possibleSpawnpoint == Vector3.zero)
        {
            float xPos = Random.Range(xMin, xMax);
            float zPos = Random.Range(zMin, zMax);
            Vector3 posOffset = new Vector3(xPos, 0, zPos);
            checkPos = centerPoint + posOffset;
            // raycasting
            RaycastHit hit;
            if (Physics.Raycast(checkPos, Vector3.down, out hit, rayCheckLength, groundLayer))
            {
                possibleSpawnpointFound = true;
                possibleSpawnpoint = hit.point;
                //Debug.Log("hit something");
            }
            else
            {
                Debug.Log("Found no possible spawnPoint");
                possibleSpawnpointFound = false;
                FindNewSpawnLocation();
            }
        }
    }

    /// <summary>
    /// spawns a certain amount of abilities at found spawn location
    /// </summary>
    public void spawnAbility()
    {
        for(int i = 0; i < amountOfAbilitiesToSpawn; i++)
        {
            int randomAbilityIndex = Random.Range(0, Abilities.Length);
            Instantiate(Abilities[randomAbilityIndex], possibleSpawnpoint + spawnOffset, Quaternion.identity);
            possibleSpawnpoint = Vector3.zero;
            FindNewSpawnLocation();
        }
        StartCoroutine(AbilitySpawnCycle());
    }

    /// <summary>
    /// the cycle in which the abilities spawn
    /// </summary>
    /// <returns></returns>
    IEnumerator AbilitySpawnCycle()
    {
        yield return new WaitForSeconds(abilitySpawnTime);
        spawnAbility();
    }


    #region Gizmos
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(possibleSpawnpointFound)
        {
            Gizmos.color = Color.blue;
            Debug.DrawLine(possibleSpawnpoint, checkPos);
        }
        Handles.DrawWireCube(transform.position, gizmoCubeSize);
    }
#endif
    #endregion
}
