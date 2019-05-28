using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulBackpack : Singleton<SoulBackpack>
{
    public int sharedSoulAmount;
    public Text soulBackpackText; // displays the amount of shared souls in the ui
    private GameObject sphere;

    public KeyCode SoulHotkey;

    private void Start() {
        GameObject soulBackpackTextParent = GameObject.FindGameObjectWithTag("SoulCounter");
        if(soulBackpackTextParent != null)
        {
            soulBackpackText = soulBackpackTextParent.GetComponentInChildren<Text>();
        }
        sphere = GameObject.FindGameObjectWithTag("Sphere");
    }

    private void Update()
    {
        if (soulBackpackText != null) {
            soulBackpackText.text = ("" + sharedSoulAmount);
        }

        if (Input.GetKeyDown(SoulHotkey))
        {
            sharedSoulAmount += 300;
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
