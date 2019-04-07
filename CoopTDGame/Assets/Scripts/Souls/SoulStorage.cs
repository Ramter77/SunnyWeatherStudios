using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulStorage : Singleton<SoulStorage>
{
    public Text tempSoulCounter;



    [Header("Soul storage")]
    [Tooltip("current ammount of souls")]
    public int soulCount = 0;
    private Text soulAmountDisplayText;

    [Tooltip("Amount that the players receive at the start of the Game")]
    public int amountOfSoulsAtStart = 0;

    [Tooltip("Amount that the players pay for a certain building")]
    public int costToBuild = 10;

    [Tooltip("Amount that the players pay for a certain upgrade")]
    public int costToUpgrade = 0;

    [Tooltip("Amount that the players pay for reviving a teammate")]
    public int costToRevive = 0;

    void Start()
    {
        GameObject SoulSphere = GameObject.FindGameObjectWithTag("Sphere");
        soulAmountDisplayText = SoulSphere.transform.parent.GetComponentInChildren<Text>();

        soulCount = amountOfSoulsAtStart;
        costToBuild = 10;
    }

    void Update()
    {
        if (soulAmountDisplayText != null) {
            soulAmountDisplayText.text = "Total Souls:" + soulCount;
            //Debug.Log("SoulCount:" + soulCount);
        }

        if (tempSoulCounter != null) {
            tempSoulCounter.text = "Total Souls:" + soulCount;
            //Debug.Log("SoulCount:" + soulCount);
        }
    }

    public void substractCostsToBuild() //give a float and then subtract that?
    {
        Debug.Log("COST: "+costToBuild);
        soulCount -= costToBuild;
        Debug.Log(soulCount);
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
