﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeAndStats : MonoBehaviour
{
    public float health = 100f;
    public float defense = 20f;

    #region Soul
    private bool dropSoul = true;   //(Controls if an object drops souls) //Todo: Randomize?
    private GameObject Soul;
    #endregion

    

    void Start()
    {
        
    }

    void Update()
    {
        if(gameObject.CompareTag("possibleTargets") && health <= 0)
        {
            //Debug.Log("yeah im dead");
            gameObject.tag = "destroyedTarget";
        }
        if (gameObject.CompareTag("Enemy") && health <= 0)
        {
            #region Instantiate Soul & destroy self
            if (dropSoul) {
                GameObject _Soul = Instantiate(Resources.Load("Soul", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
            }
            Destroy(gameObject);
            #endregion
        }
    }
}
