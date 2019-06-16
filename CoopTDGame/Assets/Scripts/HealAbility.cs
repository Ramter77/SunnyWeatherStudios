using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealAbility : MonoBehaviour
{
    [Tooltip("0 for Player 1; and 1 for Player 2")]public int playerIndex;
    public GameObject playerToHeal;

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

    public GameObject isHealedParticle;

    private Animator playerAnim;

    #region Input
    private PlayerController playC;
    private LifeAndStats lifeAndStatsScript;
    private bool _healInput;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        isHealedParticle.SetActive(false);
        if (playerIndex == 0)
        {
            playerToHeal = GameObject.FindGameObjectWithTag("Player2");
        }
        else
        {
            playerToHeal = GameObject.FindGameObjectWithTag("Player");
        }
        playC = GetComponent<PlayerController>();
        playerAnim = GetComponent<Animator>();
        lifeAndStatsScript = GetComponent<LifeAndStats>();
        fallbackHealAmount = healAmount;
        healAbilityRechardgeSpeed = healAbilityCooldown;


        if (healAbilityCooldownImage)
            healAbilityCooldownImage.fillAmount = 0;
        else
            Debug.Log("No HealAbilityCooldownImage");
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = lifeAndStatsScript.health;
        if(healthbar != null)
        {
            float v = currentHealth/ maxHealth;
            healthbar.fillAmount = v;
        }
        if (healAbilityCooldownImage != null)
            healAbilityCooldownImage.fillAmount += 1 / healAbilityCooldown * Time.deltaTime;
        else
            Debug.Log("No HealAbilityCooldownImage");

        if (!playC.isDead) {
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

        #region HealInput/Call
        if (Time.time > healAbilityRechardgeSpeed && SoulBackpack.Instance.sharedSoulAmount >= healAbilityCost)
        {
            if(healAbilityUiImageOff != null && healAbilityUiImageOn != null)
            {
                healAbilityUiImageOn.enabled = true;
                healAbilityUiImageOff.enabled = false;
            }
            else
                Debug.Log("No HealAbilityImage & No HealAbilityOffImage");


            if (_healInput) //{
            //if (Input.GetKeyDown(healAbilityHotkey))
            {
                if (!playC.isMeleeAttacking && !playC.isRangedAttacking && !playC.isCasting && playC.isGrounded/*  && !playC.isJumping */ && !playC.isDead)
                {
                    playC.isCasting = true;
                    //playC.isRangedAttacking = true;

                    healAbilityRechardgeSpeed = Time.time + healAbilityCooldown;
                    healAbility();
                    playerAnim.SetTrigger("Heal");
                    if(healAbilityCooldownImage != null)
                    {
                        healAbilityCooldownImage.fillAmount = 0;
                    }
                    else
                        Debug.Log("No HealAbilityCooldownImage");
                    //Start animation which displays the healing effect and player anim
                    SoulBackpack.Instance.reduceSoulsByCost(healAbilityCost);
                }
            }
        }
        else
        {
            if (healAbilityUiImageOff != null && healAbilityUiImageOn != null)
            {
                healAbilityUiImageOn.enabled = false;
                healAbilityUiImageOff.enabled = true;
            }
            else
                Debug.Log("No HealAbilityImage & No HealAbilityOffImage");
        }
        #endregion
    }

    #region Heal Ability 

    /// <summary>
    /// heals the player
    /// you can choose to enable self heal (so it will heal himself and others)
    /// or just heal other players -> more coop
    /// </summary>
    IEnumerator disableIsHealed()
    {
        yield return new WaitForSeconds(3.5f);
        playerToHeal.GetComponent<HealAbility>().isHealedParticle.SetActive(false);
    }

    void healAbility()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 spawnRot = new Vector3(0,90,0);
        Instantiate(healParticle, spawnPos, Quaternion.LookRotation(spawnRot));
        if(!selfHeal)
        {
            if(playerToHeal != null)
            {
                playerToHeal.GetComponent<HealAbility>().isHealedParticle.SetActive(true);
                StartCoroutine(disableIsHealed());
                if (isHealedParticle.transform.childCount == 3)
                {
                    playerToHeal.GetComponent<HealAbility>().isHealedParticle.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Simulate(0.0f, true, true);
                    playerToHeal.GetComponent<HealAbility>().isHealedParticle.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
                    playerToHeal.GetComponent<HealAbility>().isHealedParticle.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Simulate(0.0f, true, true);
                    playerToHeal.GetComponent<HealAbility>().isHealedParticle.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
                    playerToHeal.GetComponent<HealAbility>().isHealedParticle.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Simulate(0.0f, true, true);
                    playerToHeal.GetComponent<HealAbility>().isHealedParticle.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
                }

                if (playerToHeal.GetComponent<LifeAndStats>().health <= 0)
                {
                    //playerToHeal.GetComponent<LifeAndStats>().Revive();

                    GameAnalytics.Instance.PlayerRevive(playC.Player);

                    playerToHeal.GetComponent<PlayerController>().isDead = false;
                    playerToHeal.GetComponent<LifeAndStats>().health = 100;
                    playerToHeal.GetComponent<Animator>().SetBool("Dead", false);
                }
                else if (playerToHeal.GetComponent<LifeAndStats>().health > maxHealth - healAmount)
                {
                    GameAnalytics.Instance.PlayerHeal(playC.Player);

                    healAmount = maxHealth - playerToHeal.GetComponent<LifeAndStats>().health;
                    playerToHeal.GetComponent<LifeAndStats>().healHealth(healAmount);
                    healAmount = fallbackHealAmount;
                }
                else if (playerToHeal.GetComponent<LifeAndStats>().health <= 75)
                {
                    GameAnalytics.Instance.PlayerHeal(playC.Player);

                    playerToHeal.GetComponent<LifeAndStats>().healHealth(healAmount);
                    healAmount = fallbackHealAmount;
                }
            }
        }
        if(selfHeal)
        {
            if(gameObject.GetComponent<LifeAndStats>().health <= 75)
            {
                gameObject.GetComponent<LifeAndStats>().healHealth(healAmount);
                healAmount = fallbackHealAmount;
            }
            else if (gameObject.GetComponent<LifeAndStats>().health > maxHealth - healAmount)
            {
                healAmount = maxHealth - gameObject.GetComponent<LifeAndStats>().health;
                gameObject.GetComponent<LifeAndStats>().healHealth(healAmount);
                healAmount = fallbackHealAmount;
            }
            /*
            if (playerToHeal.GetComponent<LifeAndStats>().health <= 75)
            {
                playerToHeal.GetComponent<LifeAndStats>().healHealth(healAmount);
                healAmount = fallbackHealAmount;
            }
            else if (playerToHeal.GetComponent<LifeAndStats>().health > maxHealth - healAmount)
            {
                healAmount = maxHealth - playerToHeal.GetComponent<LifeAndStats>().health;
                playerToHeal.GetComponent<LifeAndStats>().healHealth(healAmount);
                healAmount = fallbackHealAmount;
            }
            else if (playerToHeal.GetComponent<LifeAndStats>().health <= 0)
            {
                playerToHeal.GetComponent<LifeAndStats>().Revive();
            }*/
        }


        //AudioManager.Instance.PlaySound(playC.playerAudioSource, Sound.playerHeal);
        /*

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
                            } 

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
        }*/
    }
    #endregion




}
