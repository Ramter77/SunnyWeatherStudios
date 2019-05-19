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
    private GameObject sphere;

    void Start()
    {
        //GameObject SoulSphere = GameObject.FindGameObjectWithTag("Sphere");
        sphere = GameObject.FindGameObjectWithTag("Sphere");
        soulAmountDisplayText = sphere.transform.parent.GetComponentInChildren<Text>();
        soulCount = amountOfSoulsAtStart;
    }

    void Update()
    {
        soulCount = Mathf.RoundToInt(sphere.GetComponent<LifeAndStats>().health);

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
        sphere.GetComponent<LifeAndStats>().health -= costToBuild;
        Debug.Log(soulCount);
    }
    
    public void substactCostsToUpgrade()
    {
        sphere.GetComponent<LifeAndStats>().health -= costToUpgrade;
    }

    public void substractCostToRevive()
    {
        sphere.GetComponent<LifeAndStats>().health -= costToRevive;
    }
}
