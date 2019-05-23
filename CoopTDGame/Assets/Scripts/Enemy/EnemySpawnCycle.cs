﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnCycle : Singleton<EnemySpawnCycle>
{
    #region Variables

    [Header("References")]
    public GameObject[] Spawners;
    public GameObject[] Enemies;

    [Header("Spawncycle")]

    [Tooltip("Maximum amount of enemies in current wave")]
    public int maximumEnemies = 0;

    [Tooltip("The amount of enemies that get added during each wave")]
    public int addedEnemiesPerWave = 0;

    [Tooltip("Amount of enemies that already spawned")]
    public int spawnedEnemies = 0;

    [Tooltip("Amount of enemies that are in the scene")]
    public int enemiesInScene = 0;

    [Tooltip("Number of wave")]
    public int Wave = 0;

    [Tooltip("True = Wave is live; False = Wave over")]
    public bool waveLive = false;

    [Tooltip("Restarting the wave")]
    public bool newWavePending = false;

    [Tooltip("Time between each wave")]
    public float timeBetweenWave = 5f;

    [Tooltip("Time between each check for wave status")]
    public float timeBetweenCheck = 2f;
    private float fallbackCheckTime = 0f;

    public GameObject annoucementMessageHolder;
    public Text annoucementText;
    public float disableTime = 5f;
    public int BossWave;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Spawners = GameObject.FindGameObjectsWithTag("Spawner");
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        fallbackCheckTime = timeBetweenCheck;
        startNewWave();
        annoucementText.text = ("The game starts now! You need to defend the Sphere!");
        StartCoroutine(disableAnnoucment());
        Wave = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CheckForEnemiesInScene()
    {
        enemiesInScene = 0;
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(Enemies.Length > 1) // if there is an enemy in the scene "do this"
        {
            foreach (GameObject en in Enemies)
            {
                enemiesInScene += 1;
            }
        }
        else // no enemy in scene
        {
            if(!newWavePending)
            {
                endWave();
                newWavePending = true;
            }
        }
    }

    /// <summary>
    /// checks for the amount of enemies that got spawned during this wave and enables/diables spawners
    /// </summary>

    public void CheckWaveStatus()
    {
        if (spawnedEnemies < maximumEnemies) // more enemies available to spawn
        {
            timeBetweenCheck = fallbackCheckTime;
            StartCoroutine(checkWaveStatusCooldown());
            foreach (GameObject go in Spawners)
            {
                go.GetComponent<EnemySpawner>().enableSpawn = true;
            }
        }
        else 
        {
            foreach (GameObject go in Spawners)
            {
                go.GetComponent<EnemySpawner>().enableSpawn = false;
            }
            StartCoroutine(checkWaveStatusCooldown());
            CheckForEnemiesInScene();
        }
    }


    public void startNewWave()
    {
        maximumEnemies += addedEnemiesPerWave;
        spawnedEnemies = 0;
        //Debug.Log("Cycle: NewWave");
        waveLive = true;
        newWavePending = false;
        StartCoroutine(checkWaveStatusCooldown());
    }

    public void endWave()
    {
       
        if(Wave == BossWave)
        {
            int a;
            a = Random.Range(0, Spawners.Length);
            Spawners[a].GetComponent<EnemySpawner>().spawnBoss();
        }
        else
        {
            annoucementMessageHolder.SetActive(true);
            annoucementText.text = ("New Wave coming in " + timeBetweenWave + " seconds");
            StartCoroutine(betweenWaveTime());
        }
    }

    public void callnewWave()
    {
        annoucementMessageHolder.SetActive(true);
        annoucementText.text = ("New Wave coming in " + timeBetweenWave + " seconds");
        StartCoroutine(betweenWaveTime());
    }

    /// <summary>
    /// cooldwon between each wave - then starts new wave
    /// </summary>
    /// <returns></returns>
    public IEnumerator betweenWaveTime()
    {
        yield return new WaitForSeconds(timeBetweenWave);
        Wave += 1;
        annoucementText.text = ("Wave " + Wave + " approaching");
        StartCoroutine(disableAnnoucment());
        startNewWave();
    }

    IEnumerator checkWaveStatusCooldown()
    {
        yield return new WaitForSeconds(timeBetweenCheck);

        CheckWaveStatus();

    }

    IEnumerator disableAnnoucment()
    {
        yield return new WaitForSeconds(disableTime);
        annoucementMessageHolder.SetActive(false);
    }
}
