using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeImage : MonoBehaviour
{
    public GameObject MenuCanvas;


    public void prepareGameToPlay()
    {
        if(MenuCanvas != null)
        MenuCanvas.GetComponent<MainMenu>().InGameSpawn();
    }


}
