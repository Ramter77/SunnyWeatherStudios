using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPickUp : MonoBehaviour
{
    private bool pickedUp = false;
    [SerializeField] private int soulValue = 0;
    [SerializeField] private int minValue = 0;
    [SerializeField] private int maxValue = 0;

    private void Start()
    {
        soulValue = Random.Range(minValue, maxValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && pickedUp == false)
        {
            pickedUp = true;
            SoulBackpack.Instance.sharedSoulAmount += soulValue;
            Destroy(gameObject);
        }
    }

}
