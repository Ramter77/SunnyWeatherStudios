using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClassOne : MonoBehaviour
{
    [Header("References")]
    private Animator playerAnim;
    private GameObject Player;
    public Transform Camera;

    [Header("General UI settings")]
    public Image healthbar;
    private float currentHealth = 100f;
    public float maxHealth = 100f;

    [Header("Heall Ability Settings")]

    [SerializeField] private KeyCode healAbilityHotkey = KeyCode.Q;

    public Image healAbilityUiImageOn;
    public Image healAbilityUiImageOff;

    [SerializeField] private float healAbilityCooldown = 0.1f;

    [SerializeField] private int healAbilityCost = 10;

    private float healAbilityRechardgeSpeed; // ability cooldown time

    public float healRadius = 7f;

    public bool selfHeal = false;

    [SerializeField] private float healAmount = 25f;

    private float fallbackHealAmount = 25f;

    [Header("Slash Ability Settings")]

    [SerializeField] private KeyCode slashAbilityHotkey = KeyCode.E;

    public Image slashAbilityUiImageOn;
    public Image slashAbilityUiImageOff;

    [SerializeField] private float slashAbilityCooldown = 0.1f;

    [SerializeField] private int slashAbilityCost = 20;

    private float slashRechargeSpeed;

    public GameObject slashSlashPrefab;

    public Transform FirePoint;

    [Header("Ultimate Ability Settings")]

    [SerializeField] private KeyCode ultimateAbilityHotkey = KeyCode.Tab;

    public Image ultimateAbilityUiImageOn;
    public Image ultimateAbilityUiImageOff;

    [SerializeField] private float ultimateAbilityCooldown = 0.1f;

    [SerializeField] private int ultimateAbilityCost = 20;

    private float ultimateRechargeSpeed;

    public GameObject ultimateAbilityGameobject;

    [Tooltip("Duration has to be smaller than cooldown")] public float ultimateAbilityDuration = 10f;


    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        Player = gameObject;
        fallbackHealAmount = healAmount;
        
        //ability Cooldowns
        ultimateAbilityGameobject.SetActive(false);
        slashRechargeSpeed = slashAbilityCooldown;
        healAbilityRechardgeSpeed = healAbilityCooldown;
        ultimateRechargeSpeed = ultimateAbilityCooldown;
       
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = GetComponent<LifeAndStats>().health;
        healthbar.fillAmount = currentHealth / maxHealth;


        #region Input / Abilities
        
        //If cooldown is low enough: shoot
        if (Time.time > healAbilityRechardgeSpeed && SoulBackpack.Instance.sharedSoulAmount >= healAbilityCost)
        {
            healAbilityUiImageOn.enabled = true;
            healAbilityUiImageOff.enabled = false;
            if (Input.GetKeyDown(healAbilityHotkey))
            {
                healAbilityRechardgeSpeed = Time.time + healAbilityCooldown;
                healAbility();
                //Start animation which displays the healing effect and player anim
                SoulBackpack.Instance.reduceSoulsByCost(healAbilityCost);
            }
        }
        else
        {
            healAbilityUiImageOn.enabled = false;
            healAbilityUiImageOff.enabled = true;
        }
        //If cooldown is low enough: shoot
        if (Time.time > slashRechargeSpeed && SoulBackpack.Instance.sharedSoulAmount >= slashAbilityCost)
        {
            slashAbilityUiImageOn.enabled = true;
            slashAbilityUiImageOff.enabled = false;
            if (Input.GetKeyDown(slashAbilityHotkey))
            {
                slashRechargeSpeed = Time.time + slashAbilityCooldown;
                slashAbility();
                //Start animation which displays the slash
                SoulBackpack.Instance.reduceSoulsByCost(slashAbilityCost);
            }
        }
        else
        {
            slashAbilityUiImageOn.enabled = false;
            slashAbilityUiImageOff.enabled = true;
        }
        //If cooldown is low enough: shoot
        if (Time.time > ultimateRechargeSpeed && SoulBackpack.Instance.sharedSoulAmount >= ultimateAbilityCost)
        {
            ultimateAbilityUiImageOn.enabled = true;
            ultimateAbilityUiImageOff.enabled = false;
            if (Input.GetKeyDown(ultimateAbilityHotkey))
            {
                ultimateRechargeSpeed = Time.time + ultimateAbilityCooldown;
                ultimateAbility();
                StartCoroutine(disableUltimate());
                //Start animation which displays the ultimate
                SoulBackpack.Instance.reduceSoulsByCost(ultimateAbilityCost);
            }
        }
        else
        {
            ultimateAbilityUiImageOn.enabled = false;
            ultimateAbilityUiImageOff.enabled = true;
        }
        #endregion

    }

    void healAbility()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, healRadius); // draw a sphere at desire point based on player pos + offset and desired radius of effect
        if (col.Length > 0)
        {

            foreach (Collider hit in col) // checks each object hit
            {
                if (selfHeal)
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
                if (!selfHeal)
                {
                    if ((hit.tag == "Player" || hit.tag == "Player2") && hit.gameObject != this.gameObject)
                    {
                        if (hit.gameObject.GetComponent<LifeAndStats>().health <= 75)
                        {
                            hit.gameObject.GetComponent<LifeAndStats>().healHealth(healAmount);
                        }
                        if (hit.gameObject.GetComponent<LifeAndStats>().health > 75)
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

    void slashAbility()
    {
        Vector3 slashSpawnPoint = FirePoint.position;
        Instantiate(slashSlashPrefab, slashSpawnPoint, Camera.rotation);
    }
}
