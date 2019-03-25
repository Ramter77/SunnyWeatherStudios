using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassSelector : MonoBehaviour
{
    private PlayerClassOne ClassOne;
    private PlayerClassTwo ClassTwo;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ClassOne = player.GetComponent<PlayerClassOne>();
        ClassTwo = player.GetComponent<PlayerClassTwo>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectClassOne()
    {
        Debug.Log("Class 1 selected");
        ClassOne.enabled = true;
        ClassTwo.enabled = false;
    }

    public void selectClassTwo()
    {
        Debug.Log("Class 2 selected");
        ClassOne.enabled = false;
        ClassTwo.enabled = true;
    }

}
