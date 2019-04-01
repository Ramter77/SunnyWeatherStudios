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
}
