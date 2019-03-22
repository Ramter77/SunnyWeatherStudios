﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCont : MonoBehaviour
{
    public void Move(Vector2 direction) {
        transform.position += transform.forward * direction.x * Time.deltaTime + transform.right * direction.y * Time.deltaTime;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
