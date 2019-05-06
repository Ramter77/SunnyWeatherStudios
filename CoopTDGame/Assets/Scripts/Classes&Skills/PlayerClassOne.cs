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
    public Image healAbilityCooldownImage;
    [SerializeField] private float healAbilityCooldown = 0.1f;

    [SerializeField] private int healAbilityCost = 10;

    private float healAbilityRechardgeSpeed; // ability cooldown time

    public float healRadius = 7f;

    public bool selfHeal = false;

    [SerializeField] private float healAmount = 25f;

    private float fallbackHealAmount = 25f;

    public GameObject healParticle;

    [Header("Slash Ability Settings")]

    [SerializeField] private KeyCode slashAbilityHotkey = KeyCode.E;

    public Image slashAbilityUiImageOn;
    public Image slashAbilityUiImageOff;
    public Image slashAbilityCooldownImage;
    [SerializeField] private float slashAbilityCooldown = 0.1f;

    [SerializeField] private int slashAbilityCost = 20;

    private float slashRechargeSpeed;

    public GameObject slashSlashPrefab;

    public Transform FirePoint;

    [Header("Ultimate Ability Settings")]

    [SerializeField] private KeyCode ultimateAbilityHotkey = KeyCode.Tab;

    public Image ultimateAbilityUiImageOn;
    public Image ultimateAbilityUiImageOff;
    public Image ultimateAbilityCooldownImage;
    [SerializeField] private float ultimateAbilityCooldown = 0.1f;

    [SerializeField] private int ultimateAbilityCost = 20;

    private float ultimateRechargeSpeed;

    public GameObject weaponGameobject;
    public GameObject ultimateAbilityGameobject;

    [Tooltip("Duration has to be smaller than cooldown")] public float ultimateAbilityDuration = 10f;



    #region INPUT
    private PlayerController playC;
    private bool _healInput;
    private bool _slashInput;
    private bool _ultimateInput;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        playC = GetComponent<PlayerController>();
        playerAnim = GetComponent<Animator>();
        Player = gameObject;
        fallbackHealAmount = healAmount;
        
        //ability Cooldowns
        ultimateAbilityGameobject.SetActive(false);
        //weaponGameobject.SetActive(true);
        slashRechargeSpeed = slashAbilityCooldown;
        healAbilityRechardgeSpeed = healAbilityCooldown;
        ultimateRechargeSpeed = ultimateAbilityCooldown;
       
    }

    // Update is called once per frame
    void Update()
    {
        // ui displays
        currentHealth = GetComponent<LifeAndStats>().health;
        healthbar.fillAmount = currentHealth / maxHealth;

        ultimateAbilityCooldownImage.fillAmount -=  1 / ultimateAbilityCooldown * Time.deltaTime;
        slashAbilityCooldownImage.fillAmount -= 1 / slashAbilityCooldown * Time.deltaTime;
        healAbilityCooldownImage.fillAmount -= 1 / healAbilityCooldown * Time.deltaTime;


        //* Player 0 input */
        if (playC.Player_ == 0)
        {
            _healInput = InputManager.Instance.Heal;
            _slashInput = InputManager.Instance.Slash;
            _ultimateInput = InputManager.Instance.Ultimate;
        }

        //* Player 1 input */
        else if (playC.Player_ == 1)
        {
            _healInput = InputManager.Instance.Heal1;
            _slashInput = InputManager.Instance.Slash1;
            _ultimateInput = InputManager.Instance.Ultimate1;
        }

        //*Player 2 input */
        else if (playC.Player_ == 2) {
            _healInput = InputManager.Instance.Heal2;
            _slashInput = InputManager.Instance.Slash2;
            _ultimateInput = InputManager.Instance.Ultimate2;
        }

        /* #region Input
        if (_input) {
            //If not already attacking or in build mode
            if (!playC.isMeleeAttacking && !playC.isRangedAttacking && !playC.isInBuildMode && !playC.isJumping) {
                playC.isMeleeAttacking = true;
                _MeleeAttack();
            }
        }
        #endregion */

        #region Input / Abilities
        
        //If cooldown is low enough: shoot
        if (Time.time > healAbilityRechardgeSpeed && SoulBackpack.Instance.sharedSoulAmount >= healAbilityCost)
        {
            healAbilityUiImageOn.enabled = true;
            healAbilityUiImageOff.enabled = false;


            if (_healInput) //{
            //if (Input.GetKeyDown(healAbilityHotkey))
            {
                if (!playC.isMeleeAttacking && !playC.isRangedAttacking && !playC.isInBuildMode && !playC.isJumping && !playC.isDead) {
                    playC.isRangedAttacking = true;

                    healAbilityRechardgeSpeed = Time.time + healAbilityCooldown;
                    healAbility();
                    playerAnim.SetTrigger("Heal");
                    healAbilityCooldownImage.fillAmount = 1;
                    //Start animation which displays the healing effect and player anim
                    SoulBackpack.Instance.reduceSoulsByCost(healAbilityCost);
                }
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

            if (_slashInput)
            //if (Input.GetKeyDown(slashAbilityHotkey))
            {
                if (!playC.isMeleeAttacking && !playC.isRangedAttacking && !playC.isInBuildMode && !playC.isJumping) {
                    playC.isRangedAttacking = true;

                    slashRechargeSpeed = Time.time + slashAbilityCooldown;
                    slashAbility();
                    playerAnim.SetTrigger("Slash");
                    slashAbilityCooldownImage.fillAmount = 1;
                    //Start animation which displays the slash
                    SoulBackpack.Instance.reduceSoulsByCost(slashAbilityCost);
                }
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

            if (_ultimateInput)
            //if (Input.GetKeyDown(ultimateAbilityHotkey))
            {
                if (!playC.isMeleeAttacking && !playC.isRangedAttacking && !playC.isInBuildMode && !playC.isJumping) {
                    ultimateRechargeSpeed = Time.time + ultimateAbilityCooldown;
                    ultimateAbility();
                    ultimateAbilityCooldownImage.fillAmount = 1;
                    StartCoroutine(disableUltimate());
                    //Start animation which displays the ultimate
                    SoulBackpack.Instance.reduceSoulsByCost(ultimateAbilityCost);
                }
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
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y+5, transform.position.z);
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
                        if (hit.gameObject.GetComponent<LifeAndStats>().health <= 0) {

                            //hit.gameObject.GetComponent<RevivePlayer>().Revive(hit.gameObject);
                            hit.gameObject.GetComponent<LifeAndStats>().Revive();
                        }
                        else {

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

    void ultimateAbility()
    {
        ultimateAbilityGameobject.SetActive(true);
        weaponGameobject.SetActive(false);
    }

    IEnumerator disableUltimate()
    {
        yield return new WaitForSeconds(ultimateAbilityDuration);
        ultimateAbilityGameobject.SetActive(false);
        weaponGameobject.SetActive(true);
    }

    void slashAbility()
    {
        //* First set the projectile
        GetComponent<RangedAttack>().ChangeProjectileTo(slashSlashPrefab);
        //* Then execute animation which calls public function "ShootActiveProjectile" on "RangedAttack" component
        //playerAnim.SetTrigger("RangedAttack");



        /* Vector3 slashSpawnPoint = FirePoint.position;
        Instantiate(slashSlashPrefab, slashSpawnPoint, Camera.rotation); */
    }
}
