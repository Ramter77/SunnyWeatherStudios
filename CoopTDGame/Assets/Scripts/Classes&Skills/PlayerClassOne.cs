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


    [Header("Ultimate Ability Settings")]

    /* [SerializeField] private KeyCode ultimateAbilityHotkey = KeyCode.Tab; */

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
    private bool _ultimateInput;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        playC = GetComponent<PlayerController>();
        playerAnim = GetComponent<Animator>();
        Player = gameObject;
        //ability Cooldowns
        ultimateAbilityGameobject.SetActive(false);
        //weaponGameobject.SetActive(true);
        ultimateRechargeSpeed = ultimateAbilityCooldown;
        if (ultimateAbilityCooldownImage != null)
        {
            ultimateAbilityCooldownImage.fillAmount = 0;
        }
        else
            Debug.Log("No Ult CooldownImage");
    }

    // Update is called once per frame
    void Update()
    {
        if (!playC.isDead) {
            if (ultimateAbilityCooldownImage != null)
            {
                ultimateAbilityCooldownImage.fillAmount += 1 / ultimateAbilityCooldown * Time.deltaTime;
            }
            else
                Debug.Log("No Ult CooldownImage");

            //* Player 0 input */
            if (playC.Player_ == 0)
            {
                _ultimateInput = InputManager.Instance.Ultimate0;
            }

            //* Player 1 input */
            else if (playC.Player_ == 1)
            {
                _ultimateInput = InputManager.Instance.Ultimate1;
            }

            //*Player 2 input */
            else if (playC.Player_ == 2) {
                _ultimateInput = InputManager.Instance.Ultimate2;
            }
        }

        #region Input / Abilities
        //If cooldown is low enough: shoot
        if (Time.time > ultimateRechargeSpeed && SoulBackpack.Instance.sharedSoulAmount >= ultimateAbilityCost)
        {
            if(ultimateAbilityUiImageOn != null && ultimateAbilityUiImageOff != null)
            {
                ultimateAbilityUiImageOn.enabled = true;
                ultimateAbilityUiImageOff.enabled = false;
            }
            else
                Debug.Log("No ult icons selected");

            if (_ultimateInput)
            //if (Input.GetKeyDown(ultimateAbilityHotkey))
            {
                if (!playC.isMeleeAttacking && !playC.isRangedAttacking && !playC.isInBuildMode/*  && !playC.isJumping */ && !playC.isDead) {
                    ultimateRechargeSpeed = Time.time + ultimateAbilityCooldown;
                    ultimateAbility();
                    ultimateAbilityCooldownImage.fillAmount = 0;
                    StartCoroutine(disableUltimate());
                    //Start animation which displays the ultimate
                    SoulBackpack.Instance.reduceSoulsByCost(ultimateAbilityCost);
                }
            }
        }
        else
        {
            if (ultimateAbilityUiImageOn != null && ultimateAbilityUiImageOff != null)
            {
                ultimateAbilityUiImageOn.enabled = false;
                ultimateAbilityUiImageOff.enabled = true;
            }
            else
                Debug.Log("No ult icons selected");
        }
        #endregion
    }
       

    public void risePrefab() {
        if (!playC.isMeleeAttacking && !playC.isRangedAttacking && !playC.isInBuildMode && playC.isGrounded/*  && !playC.isJumping */ && !playC.isDead) {
            playerAnim.SetTrigger("Slash");
            //SoulBackpack.Instance.reduceSoulsByCost(slashAbilityCost);
        }
    }

    void ultimateAbility()
    {
        ultimateAbilityGameobject.SetActive(true);
        weaponGameobject.SetActive(false);

        AudioManager.Instance.PlaySound(playC.playerAudioSource, Sound.playerUltimate);
    }

    IEnumerator disableUltimate()
    {
        yield return new WaitForSeconds(ultimateAbilityDuration);
        ultimateAbilityGameobject.SetActive(false);
        weaponGameobject.SetActive(true);
    }

}
