using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassTwo : MonoBehaviour
{

    private Animator playerAnim;

    [SerializeField]
    private float specialAbilityCooldown = 0.1f; 

    private float abilityRechardgeSpeed; // ability cooldown time

    public GameObject specialAbilityPrefab;

    public Transform SpellcastPoint;

    //public Transform Camera;

    [SerializeField]private KeyCode specialAbilityHotkey = KeyCode.Q;

    private GameObject Player;

    public Vector3 knockbackForce;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        Player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        #region Input
        if (Input.GetKeyDown(specialAbilityHotkey))
        {
            //If cooldown is low enough: shoot
            if (Time.time > abilityRechardgeSpeed)
            {
                abilityRechardgeSpeed = Time.time + specialAbilityCooldown;
                Instantiate(specialAbilityPrefab, SpellcastPoint.position, transform.rotation);
                //Start animation which shoots projectile on event

                playerAnim.SetTrigger("MagicAttack");
            }
        }
        #endregion
    }


}
