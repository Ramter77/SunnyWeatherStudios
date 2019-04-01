using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soulTransfer : MonoBehaviour
{
    private bool enableSoulTransfer = false;
    [SerializeField] private KeyCode transferKey = KeyCode.C;
    [SerializeField] private int amountOfSoulsToTransfer = 10;
    [SerializeField] private float timeToTransferAmount = .2f;
    private float fallbackTime = 0f;

    private void Start()
    {
        fallbackTime = timeToTransferAmount;
    }

    private void Update()
    {
        if(enableSoulTransfer == true && Input.GetKey(transferKey))
        {
            tranferProcess();
            Debug.Log("Im channeling the souls");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Player2")
        {
            enableSoulTransfer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2")
        {
            enableSoulTransfer = false;
        }
    }

    void tranferProcess()
    {
        timeToTransferAmount -= Time.deltaTime;
        if(timeToTransferAmount <= 0)
        {
            SoulBackpack.Instance.transferSoulsIntoSphere(amountOfSoulsToTransfer);
            timeToTransferAmount = fallbackTime;
        }
    }

}
