using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivePlayer : MonoBehaviour
{
    GameObject player1, player2;
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.I)) {
            player1.GetComponent<PlayerController>().isDead = false;
            player1.GetComponent<LifeAndStats>().health = 100;
            player1.GetComponent<Animator>().SetBool("Dead", false);
        }

        else if (Input.GetKey(KeyCode.O)) {
            player2.GetComponent<PlayerController>().isDead = false;
            player2.GetComponent<LifeAndStats>().health = 100;
            player2.GetComponent<Animator>().SetBool("Dead", false);
        }
    }

    public void Revive(GameObject player) {
        player.GetComponent<PlayerController>().isDead = false;
        player.GetComponent<LifeAndStats>().health = 100;
        player.GetComponent<Animator>().SetBool("Dead", false);
        player.GetComponent<PlayerController>().isDead = false;
    }
}
