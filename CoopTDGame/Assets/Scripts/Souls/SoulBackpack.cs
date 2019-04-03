using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulBackpack : Singleton<SoulBackpack>
{
    public int sharedSoulAmount;
    public Text soulBackpackText; // displays the amount of shared souls in the ui

    private void Update()
    {
        soulBackpackText.text = ("" + sharedSoulAmount);
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
            SoulStorage.Instance.soulCount += amount;
        }
        else
        {
            Debug.Log("You do not have enough Souls to do this action");
        }
    }
}
