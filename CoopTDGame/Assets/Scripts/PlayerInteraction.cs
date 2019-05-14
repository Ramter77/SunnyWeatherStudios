using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : Singleton<PlayerInteraction>
{
    public GameObject Player_1InteractionText;
    public GameObject Player_2InteractionText;

    public bool Player_1InRange = false;
    public bool Player_2InRange = false;

    // Start is called before the first frame update
    void Start()
    {
        Player_1InRange = false;
        Player_2InRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player_1InRange)
        {
            Player_1InteractionText.SetActive(true);
        }
        else
        {
            Player_1InteractionText.SetActive(false);
        }



        if (Player_2InRange)
        {
            Player_2InteractionText.SetActive(true);
        }
        else
        {
            Player_2InteractionText.SetActive(false);
        }
    }
}
