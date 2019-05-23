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

    private Animator playerAnim;
    private FractureObject fractureScript;
    public GameObject GameOverScreen = null;


    
    private bool _dead;
    

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        fractureScript = GetComponent<FractureObject>();
        fallbackHealCooldown = healCooldown;
        amountOfUnitsAttacking = 0;

        if(gameObject.CompareTag("Sphere"))
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

        if(gameObject.CompareTag("possibleTargets") && health <= 0)
        {
            //Debug.Log("yeah im dead");
            gameObject.tag = "destroyedTarget";

            if (destroyable) {
                Debug.Log("Fractured target");
                fractureScript.Fracture(gameObject);
            }
        }
        
        if (gameObject.CompareTag("Enemy"))
        {
            if (!_dead) {
                if (health <= 0) {
                    if (GetComponent<BasicEnemy>().Target != null && GetComponent<BasicEnemy>().Target.GetComponent<LifeAndStats>().amountOfUnitsAttacking > 0)
                    {
                        GetComponent<BasicEnemy>().Target.GetComponent<LifeAndStats>().amountOfUnitsAttacking -= 1;
                    }

                    if(GetComponent<BasicEnemy>().enemyType == 2)
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
                        GetComponent<Ragdoll>().toggleRagdoll(true);
                        _dead = true;
                    }
                    else {
                        Destroy(gameObject);
                    }
                    #endregion

                    #region Dissolve
                    if (dissolveOnDeath) {
                        GetComponent<StatusEffect>().DissolveCoroutine();
                    }
                    #endregion
                }
            }
        }

        if (gameObject.CompareTag("Player") || gameObject.CompareTag("Player2"))
        {
            if (health <= 0) {
                Debug.Log("Player dead");
                playerAnim.SetBool("Dead", true);
                GetComponent<PlayerController>().isDead = true;
            }
        } 

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

             if (gameObject.CompareTag("Player") || gameObject.CompareTag("Player2") || gameObject.CompareTag("Enemy")) 
            { 
                playerAnim.SetTrigger("TakeDamage");
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
        GetComponent<PlayerController>().isDead = false;
        GetComponent<LifeAndStats>().health = 100;
        GetComponent<Animator>().SetBool("Dead", false);
        GetComponent<PlayerController>().isDead = false;
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


    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Player" || gameObject.tag == "Player2") {
            if (other.tag == "EnemyWeapon") {
                TakeDamage(20);
            }
            else if (other.tag == "EnemyProjectile") {
                TakeDamage(10);
            }
        }
    }
}
