using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnCycle : MonoBehaviour
{
    [Header("References")]
    public GameObject[] Spawners;

    [Header("Spawncycle")]
    [Tooltip("Maximum amount of enemies in current wave")] public int maximumEnemies = 0;
    [Tooltip("The amount of enemies that get added during each wave")] public int addedEnemiesPerWave = 0;
    [Tooltip("Number of wave")] public int Wave = 0;
    [Tooltip("Amount of enemies that already spawned")] public int currentEnemies = 0;

    // Start is called before the first frame update
    void Start()
    {
        Spawners = GameObject.FindGameObjectsWithTag("Spawner");
    }

    // Update is called once per frame
    void Update()
    {
        if(currentEnemies < maximumEnemies)
        {
            foreach (GameObject go in Spawners)
            {
                go.GetComponent<EnemySpawner>().enableSpawn = true;
            }
        }
        if(currentEnemies >= maximumEnemies)
        {
            foreach (GameObject go in Spawners)
            {
                go.GetComponent<EnemySpawner>().enableSpawn = false;
            }
        }
    }

    public void startNewWave()
    {
        maximumEnemies += addedEnemiesPerWave;
        currentEnemies = 0;
        Debug.Log("Cycle: NewWave");
    }
}
