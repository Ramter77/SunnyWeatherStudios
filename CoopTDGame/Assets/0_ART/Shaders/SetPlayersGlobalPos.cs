using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetPlayersGlobalPos : MonoBehaviour
{
    [SerializeField]
    private int player = 1;
    private string parameter;

    void Start()
    {
        if (player == 1) {
            parameter = "_Player1";
        }
        else
        {
            parameter = "_Player2";
        }
    }

    void Update()
    {
        Shader.SetGlobalVector(parameter, transform.position);
    }
}
