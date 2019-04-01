using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBackpack : Singleton<SoulBackpack>
{
    public int sharedSoulAmount;

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
