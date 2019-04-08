using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulBackpack : Singleton<SoulBackpack>
{
    public int sharedSoulAmount;
    public Text soulBackpackText; // displays the amount of shared souls in the ui
    private GameObject sphere;

    private void Start() {
        GameObject soulBackpackTextParent = GameObject.FindGameObjectWithTag("SoulCounter");
        soulBackpackText = soulBackpackTextParent.GetComponentInChildren<Text>();
        sphere = GameObject.FindGameObjectWithTag("Sphere");
    }

    private void Update()
    {
        if (soulBackpackText != null) {
            soulBackpackText.text = ("" + sharedSoulAmount);
        }
    }


    public void reduceSoulsByCost(int cost)
    {
        sharedSoulAmount -= cost;
    }

    public void transferSoulsIntoSphere(int amount)
    {
        if(sharedSoulAmount >= amount)
        {
            sharedSoulAmount -= amount;
            sphere.GetComponent<LifeAndStats>().health += amount;
        }
        else
        {
            Debug.Log("You do not have enough Souls to do this action");
        }
    }
}
