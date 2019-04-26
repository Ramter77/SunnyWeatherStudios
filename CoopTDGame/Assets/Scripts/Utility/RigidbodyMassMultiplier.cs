using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMassMultiplier : MonoBehaviour
{
    [Header ("Use context menu to multiply & revert")]
    [Tooltip ("Don't change multiplier to revert correctly (1/multiplier)")]
    [SerializeField]
    private float multiplier = 1f;

    private Rigidbody rb;
    private Component[] rbs;
    
    [ContextMenu("Multiply mass of rigidbodies with multiplier")]
    void Mutiply()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass *= multiplier;

        rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody _rb in rbs)
            _rb.mass *= multiplier;
    }

    [ContextMenu("REVERT multiplying mass of rigidbodies with multiplier")]
    void Revert()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass *= 1/multiplier;

        rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody _rb in rbs)
            _rb.mass *= 1/multiplier;
    }
}
