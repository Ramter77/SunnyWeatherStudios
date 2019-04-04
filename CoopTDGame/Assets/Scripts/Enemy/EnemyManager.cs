using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public GameObject[] Enemies;
    public List<GameObject> Targets;
    public List<GameObject> checkArray;

    public int amountOfEnemiesAttackingPlayer;
    public float scanTime = 2f;

    private void Start()
    {
        StartCoroutine(scanCycle());
        Targets = new List<GameObject>();
        checkArray = new List<GameObject>();
    }

    private IEnumerator scanCycle()
    {
        yield return new WaitForSeconds(1);
        scanForEnemies();
    }


    void scanForEnemies()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (Enemies.Length > 1) // if there is an enemy in the scene "do this"
        {
            foreach (GameObject en in Enemies)
            {
                if (en.GetComponent<BasicEnemy>().Target != null && en.GetComponent<BasicEnemy>().checkedForTarget == false)
                {
                    Targets.Add(en.GetComponent<BasicEnemy>().Target);
                    en.GetComponent<BasicEnemy>().checkedForTarget = true;
                }
            }
        }
        StartCoroutine(scanCycle());
    }
}


