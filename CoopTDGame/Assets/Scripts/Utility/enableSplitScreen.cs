using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableSplitScreen : MonoBehaviour
{
    public bool _enableSplitScreen;
    public Camera cam;
    public GameObject otherPlayer;

    void Start()
    {
        if (cam == null) {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        if (_enableSplitScreen) {
            
            cam.rect = new Rect(-0.5f, 0.0f, 1.0f, 1.0f);

            if (otherPlayer == null) {
                GameObject otherPlayer = GameObject.FindGameObjectWithTag("Player2");
            }
            otherPlayer.gameObject.SetActive(true);
        }
        else
        {
            cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

            if (otherPlayer == null) {
                GameObject otherPlayer = GameObject.FindGameObjectWithTag("Player2");
            }
            otherPlayer.gameObject.SetActive(false);
        }
    }
}
