using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealAbility : MonoBehaviour
{

    [Header("General UI settings")]
    public Image healthbar;
    private float currentHealth = 100f;
    public float maxHealth = 100f;

    [Header("Heall Ability Settings")]

    public Image healAbilityUiImageOn;
    public Image healAbilityUiImageOff;
    public Image healAbilityCooldownImage;
    [SerializeField] private float healAbilityCooldown = 0.1f;

    [SerializeField] private int healAbilityCost = 10;

    private float healAbilityRechardgeSpeed; // ability cooldown time

    public float healRadius = 7f;

    public bool selfHeal = false;

    [SerializeField] private float healAmount = 25f;

    private float fallbackHealAmount = 25f;

    public GameObject healParticle;


    #region Input
    private PlayerController playC;
    private bool _healInput;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        playC = GetComponent<PlayerController>();
        fallbackHealAmount = healAmount;
        healAbilityRechardgeSpeed = healAbilityCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        // ui displays
        currentHealth = GetComponent<LifeAndStats>().health;
        healthbar.fillAmount = currentHealth / maxHealth;

        healAbilityCooldownImage.fillAmount -= 1 / healAbilityCooldown * Time.deltaTime;


        //* Player 0 input */
        if (playC.Player_ == 0)
        {
            _healInput = InputManager.Instance.Heal0;
        }

        //* Player 1 input */
        else if (playC.Player_ == 1)
        {
            _healInput = InputManager.Instance.Heal1;
        }

        //*Player 2 input */
        else if (playC.Player_ == 2)
        {
            _healInput = InputManager.Instance.Heal2;
        }
    }

    #region Heal Ability 

    /// <summary>
    /// heals the player
    /// you can choose to enable self heal (so it will heal himself and others)
    /// or just heal other players -> more coop
    /// </summary>

    void healAbility()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        Instantiate(healParticle, spawnPos, Quaternion.identity);


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
                        if (hit.gameObject.GetComponent<LifeAndStats>().health <= 0)
                        {

                            //hit.gameObject.GetComponent<RevivePlayer>().Revive(hit.gameObject);
                            hit.gameObject.GetComponent<LifeAndStats>().Revive();
                        }
                        else
                        {
                            //! Why not reset to max health? :
                            /* if (hit.gameObject.GetComponent<LifeAndStats>().health + healAmount > hit.gameObject.GetComponent<LifeAndStats>().maxhealth) {
                                hit.gameObject.GetComponent<LifeAndStats>().health = hit.gameObject.GetComponent<LifeAndStats>().maxhealth;
                            }
                            else {
                                hit.gameObject.GetComponent<LifeAndStats>().healHealth(healAmount);
                            } */

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
    }
    #endregion




}
