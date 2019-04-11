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


    private bool _input;

    private void Start()
    {
        fallbackTime = timeToTransferAmount;
    }

    public void InputHandler(Animator anim) {
        //_input = true;

        
        if(enableSoulTransfer == true)
        {
            anim.SetBool("Channeling", true);
            tranferProcess();
            Debug.Log("Im channeling the souls");
        }
        else {
            anim.SetBool("Channeling", false);
        }
        
    }

    public void DisableChannelingAnim(Animator anim) {
        anim.SetBool("Channeling", false);
    }

    private void Update()
    {
        //_input = false;


        
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
