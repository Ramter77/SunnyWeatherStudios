using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassTwo : MonoBehaviour
{
    [Header("References")]
    private Animator playerAnim;
    private GameObject Player;

    [Header("Special Ability Settings")]

    [SerializeField] private KeyCode specialAbilityHotkey = KeyCode.Q;

    [SerializeField] private float specialAbilityCooldown = 0.1f;

    private float abilityRechardgeSpeed; // ability cooldown time

    [SerializeField] private int specialAbilityCost = 10;

    public float healRadius = 7f;

    public bool selfHeal = false;

    [SerializeField] private float healAmount = 25f;

    private float fallbackHealAmount = 25;

    public float maxHealth = 100f;

    [Header("Ultimate Ability Settings")]

    [SerializeField] private KeyCode ultimateAbilityHotkey = KeyCode.E;

    [SerializeField] private float ultimateAbilityCooldown = 0.1f;

    [SerializeField] private int ultimateAbilityCost = 20;

    private float ultimateRechargeSpeed;

    public GameObject ultimateAbilityGameobject;

    public float ultimateAbilityDuration = 10f;


    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        Player = gameObject;
        fallbackHealAmount = healAmount;
        ultimateAbilityGameobject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        #region Input
        if (Input.GetKeyDown(specialAbilityHotkey))
        {
            //If cooldown is low enough: shoot
            if (Time.time > abilityRechardgeSpeed && SoulBackpack.Instance.sharedSoulAmount >= specialAbilityCost)
            {
                abilityRechardgeSpeed = Time.time + specialAbilityCooldown;
                specialAbility();
                //Start animation which displays the healing effect and player anim
                Debug.Log("healing by" + healAmount);
            }
        }

        if (Input.GetKeyDown(ultimateAbilityHotkey))
        {
            //If cooldown is low enough: shoot
            if (Time.time > ultimateRechargeSpeed && SoulBackpack.Instance.sharedSoulAmount >= ultimateAbilityCost)
            {
                ultimateRechargeSpeed = Time.time + ultimateAbilityCooldown;
                ultimateAbility();
                StartCoroutine(disableUltimate());
                //Start animation which displays the ultimate
            }
        }

        #endregion
    }

    void specialAbility()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, healRadius); // draw a sphere at desire point based on player pos + offset and desired radius of effect
        if (col.Length > 0)
        {

            foreach (Collider hit in col) // checks each object hit
            {
                if(selfHeal)
                {
                    if (hit.tag == "Player" || hit.tag == "Player2")
                    {
                        if (hit.gameObject.GetComponent<LifeAndStats>().health <= 75)
                        {
                            hit.gameObject.GetComponent<LifeAndStats>().healHealth(healAmount);
                            healAmount = fallbackHealAmount;
                        }
                        if (hit.gameObject.GetComponent<LifeAndStats>().health > 75)
                        {
                            healAmount = maxHealth - hit.gameObject.GetComponent<LifeAndStats>().health;
                            hit.gameObject.GetComponent<LifeAndStats>().healHealth(healAmount);
                            healAmount = fallbackHealAmount;
                        }
                    }
                }
                if(!selfHeal)
                {
                    if ((hit.tag == "Player" || hit.tag == "Player2") && hit.gameObject != this.gameObject)
                    {
                        if(hit.gameObject.GetComponent<LifeAndStats>().health <= 75)
                        {
                            hit.gameObject.GetComponent<LifeAndStats>().healHealth(healAmount);
                        }
                        if(hit.gameObject.GetComponent<LifeAndStats>().health > 75)
                        {
                            healAmount = maxHealth - hit.gameObject.GetComponent<LifeAndStats>().health;
                            hit.gameObject.GetComponent<LifeAndStats>().healHealth(healAmount);
                            healAmount = fallbackHealAmount;
                        }
                    }
                }
            }
        }
    }

    void ultimateAbility()
    {
        ultimateAbilityGameobject.SetActive(true);
    }

    IEnumerator disableUltimate()
    {
        yield return new WaitForSeconds(ultimateAbilityDuration);
        ultimateAbilityGameobject.SetActive(false);
    }





    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, healRadius);
        Gizmos.color = Color.red;
    }

    #endregion
}
