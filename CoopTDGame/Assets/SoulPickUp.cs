using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPickUp : MonoBehaviour
{
    private GameObject manager;
    private bool pickedUp = false;
    [SerializeField] private int soulValue = 0;
    [SerializeField] private int minValue = 0;
    [SerializeField] private int maxValue = 0;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager");
        soulValue = Random.Range(minValue, maxValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && pickedUp == false)
        {
            pickedUp = true;
            manager.GetComponent<SoulStorage>().soulCount += soulValue;
            Destroy(gameObject);
        }
    }

}
