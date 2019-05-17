using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetPlayersGlobalPos : MonoBehaviour
{
    [SerializeField]
    private int player = 1;
    private string parameter;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
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
