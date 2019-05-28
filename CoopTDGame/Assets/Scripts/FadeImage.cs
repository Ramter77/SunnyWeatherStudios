using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeImage : MonoBehaviour
{
    public GameObject MenuCanvas;


    public void prepareGameToPlay()
    {
        MenuCanvas.GetComponent<MainMenu>().InGameSpawn();
    }


}
