using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPickUp : MonoBehaviour
{
    private bool pickedUp = false;
    [SerializeField] private int soulValue = 0;
    [SerializeField] private int minValue = 0;
    [SerializeField] private int maxValue = 0;

    [SerializeField] private GameObject particleEffect;

    private void Start()
    {
        soulValue = Random.Range(minValue, maxValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Player2") && pickedUp == false)
        {
            GameAnalytics.Instance.SoulsPickedUp();

            Instantiate(particleEffect, transform.position, Quaternion.identity);
            pickedUp = true;
            SoulBackpack.Instance.addSoulsToCounter(soulValue);

            AudioManager.Instance.PlaySound(null, Sound.playerPickupSoul, false);

            Destroy(gameObject);
        }
    }
}
