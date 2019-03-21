using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSoulContainer : MonoBehaviour
{

    [Header("Player shared Soul storage")]
    [Tooltip("current shared souls")]
    public int playerSoulCount;
    public Text playerSoulDountDisplayText;


    [Tooltip("Amount that the players pays for using ability 1")]
    public int Ability1Cost = 0;
    public int Ability2Cost = 0;
    public int Ability3Cost = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
