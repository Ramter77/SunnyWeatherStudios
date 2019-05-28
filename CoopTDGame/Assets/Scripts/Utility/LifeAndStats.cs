using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeAndStats : MonoBehaviour
{
    public float health = 100f;
    public float maxhealth = 100f;
    public float defense = 20f;
    private float healCooldown = 5f;
    private float fallbackHealCooldown;
    public int amountOfUnitsAttacking = 0;

    #region Soul
    private bool dropSoul = true;   //(Controls if an object drops souls) //Todo: Randomize?
    private GameObject Soul;
    #endregion

    public GameObject particleEffect;
    public float ParticleOnHitEffectYoffset = 1;
    public Transform particleSpawnLocation;
    private Vector3 spawnPos;

    public bool ragdollOnDeath;
    public bool dissolveOnDeath;
    public bool destroyable;
    public bool invincible = false;
    private float invinciblityDuration = 0.3f;

    private PlayerController playC;
    private Animator anim;
    private BasicEnemy basicEnemyScript;
    private Ragdoll ragdollScript;
    private StatusEffect statusEffectScript;
    private FractureObject fractureScript;
    
    public GameObject GameOverScreen = null;


    
    private bool _dead;
    

    void Start()
    {
        if (GetComponent<PlayerController>() != null) {
            playC = GetComponent<PlayerController>();
        }
        anim = GetComponent<Animator>();

        if (gameObject.CompareTag("Enemy")) {
            basicEnemyScript = GetComponent<BasicEnemy>();
            ragdollScript = GetComponent<Ragdoll>();
            statusEffectScript = GetComponent<StatusEffect>();
        }

        fractureScript = GetComponent<FractureObject>();
        fallbackHealCooldown = healCooldown;
        amountOfUnitsAttacking = 0;

        if (gameObject.CompareTag("Sphere"))
        {
            if (GameOverScreen != null) {
                GameOverScreen.SetActive(false);
            }
            else
            {
                Debug.Log("GameOverScreen not assigned to sphere");
            }
        }
        
    }

    void Update()
    {
        reduceHealthCooldown();
        
        if (gameObject.CompareTag("Sphere"))
        {
            GameManager.Instance.GetComponent<SoulStorage>().soulCount = Mathf.RoundToInt(health);

            if (health < 0)
            {
                Debug.Log("Fractured SPHERE");
                fractureScript.Fracture(gameObject.transform.parent.gameObject);

                GameOverScreen.SetActive(true);
                StartCoroutine(restartgame());
            }
        }
    }

    IEnumerator restartgame()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(1);
    }

    public void TakeDamage(float dmg) {
        if (!invincible) {
            Invincible();

            health -= dmg;
            ParticleOnHitEffect(ParticleOnHitEffectYoffset);

            if (gameObject.CompareTag("Player") || gameObject.CompareTag("Player2"))
            { 
                AudioManager.Instance.PlaySound(playC.playerAudioSource, AudioManager.Instance.playerTakingDamage);

                if (!playC.isDead) {
                    if (health <= 0) {
                        Debug.Log(playC.gameObject.name + " is dead");
                        playC.isDead = true;

                        anim.SetBool("Dead", true);
                    }
                    else
                    {
                        anim.SetTrigger("TakeDamage");
                    }
                }
            }

            else if (gameObject.CompareTag("Enemy")) 
            {
                if (AudioManager.Instance.towerProjectiles.Length > 0) {
                    AudioManager.Instance.PlaySound(gameObject.GetComponent<AudioSource>(), AudioManager.Instance.enemyTakingDamage[basicEnemyScript.enemyType]);
                }
                
                if (!_dead) {
                    if (health <= 0) {
                        if (basicEnemyScript.Target != null && basicEnemyScript.Target.GetComponent<LifeAndStats>().amountOfUnitsAttacking > 0)
                        {
                            basicEnemyScript.Target.GetComponent<LifeAndStats>().amountOfUnitsAttacking -= 1;
                        }

                        if (basicEnemyScript.enemyType == 2)
                        {
                            EnemySpawnCycle.Instance.callnewWave();
                        }

                        #region Instantiate Soul
                        if (dropSoul) {
                            Vector3 dropPos = new Vector3(transform.position.x, transform.position.y+2, transform.position.z);
                            GameObject _Soul = Instantiate(Resources.Load("Soul", typeof(GameObject)), dropPos, Quaternion.identity) as GameObject;
                        }
                        #endregion
                        
                        #region Ragdoll
                        if (ragdollOnDeath) {
                            ragdollScript.toggleRagdoll(true);
                            _dead = true;
                        }
                        else {
                            Destroy(gameObject);
                        }
                        #endregion

                        #region Dissolve
                        if (dissolveOnDeath) {
                            statusEffectScript.DissolveCoroutine();
                        }
                        #endregion
                    }
                }
                else
                {
                    anim.SetTrigger("TakeDamage");
                }
            }

            else if (gameObject.CompareTag("possibleTargets")) {
                AudioManager.Instance.PlaySound(gameObject.GetComponent<AudioSource>(), AudioManager.Instance.towerTakingDamage);

                if (health <= 0) {
                    gameObject.tag = "destroyedTarget";

                    if (destroyable) {
                        Debug.Log("Fractured target");
                        fractureScript.Fracture(gameObject);
                    }
                }
            }
        }
    }

    public void ParticleOnHitEffect(float yOffset) {
        if (particleEffect != null) {
            //If there us no spawn location then use the Y offset
            if (particleSpawnLocation == null) {
                spawnPos = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
            }

            //Else use the provided spawn location
            else {
                spawnPos = particleSpawnLocation.position;
            }

            Instantiate(particleEffect, spawnPos, Quaternion.identity);
        }
    }

    public void healHealth(float healAmount)
    {
        if(healCooldown <= 0)
        {
            health += healAmount;
            healCooldown = fallbackHealCooldown;
        }
    }

    public void Revive() {
        playC.isDead = false;
        health = 100;
        anim.SetBool("Dead", false);
    }

    void reduceHealthCooldown()
    {
        healCooldown -= Time.deltaTime;
    }

    void Invincible() {
        invincible = true;

        StartCoroutine(stopInvincible(invinciblityDuration));
    }

    IEnumerator stopInvincible(float duration) {
        yield return new WaitForSeconds(duration);

        invincible = false;
    }
}
