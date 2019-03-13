﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulStorage : MonoBehaviour
{
    [Header("Soul storage")]
    [Tooltip("current ammount of souls")]
    public int soulCount = 0;

    [Tooltip("Amount that the players receive at the start of the Game")]
    public int amountOfSoulsAtStart = 0;

    [Tooltip("Amount that the players pay for a certain building")]
    public int costToBuild = 0;

    [Tooltip("Amount that the players pay for a certain upgrade")]
    public int costToUpgrade = 0;

    [Tooltip("Amount that the players pay for reviving a teammate")]
    public int costToRevive = 0;

    // Start is called before the first frame update
    void Start()
    {
        soulCount = amountOfSoulsAtStart;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("SoulCount:" + soulCount);
    }

    public void substractCostsToBuild()
    {
        soulCount -= costToBuild;
    }
    
    public void substactCostsToUpgrade()
    {
        soulCount -= costToUpgrade;
    }


    public void substractCostToRevive()
    {
        soulCount -= costToRevive;
    }

}
