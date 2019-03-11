using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Object Ref")]
    public Transform spawnPoint;
    public GameObject[] EnemyPrefabs;

    [Header("Spawnrate")]
    public float spawnrate;
    private float fallbackSpawnrate;
    public int amountOfEnemysToSpawn = 10;

    [Header("Spawn position")]
    public float MinX;
    public float MaxX;


    public float MinZ;
    public float MaxZ;


    // Start is called before the first frame update
    void Start()
    {
        spawnEnemy();
        fallbackSpawnrate = spawnrate;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer();
        
    }

    public void spawnEnemy()
    {
        for(int i = 0; i < amountOfEnemysToSpawn; i++)
        {
            Vector3 centerPos = spawnPoint.position;
            Vector3 pos = new Vector3(Random.Range(MinX, MaxX), 0, Random.Range(MinZ, MaxZ));
            Vector3 spawnPos = centerPos - pos;
            int random = Random.Range(0, EnemyPrefabs.Length);
            Instantiate(EnemyPrefabs[random], spawnPos, Quaternion.identity);
        }
        
    }

    void spawnTimer()
    {
        spawnrate -= Time.deltaTime; 
        if(spawnrate <= 0)
        {
            spawnEnemy();
            spawnrate = fallbackSpawnrate;
        }
    }
    
}
