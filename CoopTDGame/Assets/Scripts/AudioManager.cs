using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MultiAudioListener;


public enum Sound { empty, 
                    playerJump, playerMeleeAttack, playerRangedAttack, playerHeal, playerUltimate, playerActivateBuilding, playerCombineBuildingFire, 
                    playerCombineBuildingIce, playerCombineBuildingBlast, playerTakeDamage,playerDeath, playerRevive,
                    playerPickupSoul, meleeImpact,
                    enemyMeleeAttack, enemyRangedAttack, enemyBossAttack, enemyTakeDamage, enemyRagdoll,
                    sphereTakeDamage, towerTakeDamage, towerDefault, towerFire, towerIce, towerBlast, towerDefaultImpact, towerFireImpact, towerIceImpact, towerBlastImpact, trapDefault, trapFire, trapIce, trapBlast, 
                    };
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    [Tooltip ("Stop any sound before playing the next one")]
    private bool stopSoundBeforeNext;

    //[SerializeField]
    [Tooltip ("The range used to randomize pitch")]
    public bool randomizeVolume;
    [SerializeField]
    [Tooltip ("The range used to randomize volume")]
    private Vector2 volumeRandomization = new Vector2(-0.1f, 0.1f);
    [SerializeField]
    [Tooltip ("The range used to randomize pitch")]
    private Vector2 pitchRandomization = new Vector2(-0.1f, 0.1f);

    [SerializeField] //hidden because it adds the component on start
    [Tooltip ("The first child of the object playing the sounds")]
    private MultiAudioSource soundAudioSource;
    [SerializeField] //hidden because it adds the component on start
    [Tooltip ("The second child of the object playing the music (looping)")]
    private MultiAudioSource musicAudioSource;
    [Header ("Music")]
    [SerializeField]
    private bool playMenuMusicOnStart;
    [SerializeField]
    [Tooltip ("Menu music clip (looping)")]
    private AudioClip menuMusicAudioClip;
    [SerializeField]
    [Tooltip ("Ingame music clip (looping)")]
    public AudioClip ingameMusicAudioClip;
    

    [Header("Movement Sounds")]
    [Header("Player Sounds")]
    public AudioClip[] playerJump;
    //public AudioClip playerLand;

    [Header("Attacks")]
    public AudioClip[] playerMeleeAttack;
    public AudioClip[] playerMeleeImpact;
    public AudioClip[] playerRangedAttack;

    [Header("Spells")]
    public AudioClip[] playerHeal;
    public AudioClip[] playerUltimate;
    public AudioClip[] playerActivateBuilding;
    public AudioClip[] playerActivateTrap;              //Trap activation for traps are shorter, so a shorter sound file is played
    [Tooltip ("Order: Fire, Ice, Blast")]
    public AudioClip[] playerCombineBuilding;       //when you combine an ability with a tower

    [Header("Life & Death")]
    public AudioClip[] playerTakeDamage;
    //public AudioClip playerTakingHighDamage;
    public AudioClip[] playerDeath;
    public AudioClip[] playerRevive;

    [Header("Interactions")]
    public AudioClip[] playerPickupSoul;
    public AudioClip[] meleeImpacts;

    
    [Header ("Enemies")]
    [Space (15)]
    public AudioClip[] enemyMeleeAttack;
    public AudioClip[] enemyRangedAttack;
    public AudioClip[] enemyBossAttack;
    public AudioClip[] enemyTakeDamage;
    public AudioClip[] enemyRagdoll;

    [Header ("Towers & Traps")]
    [Space (15)]
    public AudioClip[] sphereTakeDamage;
    public AudioClip[] towerTakeDamage;
    [Tooltip ("Order: Default, Fire, Ice, Blast")]
    public AudioClip[] towerProjectiles;
    [Tooltip ("Order: Default, Fire, Ice, Blast")]
    public AudioClip[] towerProjectileImpacts;
    [Tooltip ("Order: Default, Fire, Ice, Blast")]
    public AudioClip[] trapVFX;


    void Start()
    {
        if (playMenuMusicOnStart) {
            PlayMusic(menuMusicAudioClip);
        }
    }

    public void StopSound(MultiAudioSource _source) {
        if (_source != null) {
            _source.Stop();
        }
    }

    /// <summary>
    /// Switch the clip of the audio source to the provided one and play it on the passed audio source
    /// or on it's own when it's null
    /// </summary>
    /// <param name="_source">Use provided audio source or own audio source when passed null</param>
    /// <param name="_clip">Play provided _clip once</param>
    public void PlaySound(MultiAudioSource _source, Sound sound, bool randomizePitch) {
        //if (sound != null) {
            //Set audio source
            MultiAudioSource  _audioSource;
            if (_source != null) {
                _audioSource = _source;
            }
            else
            {
                _audioSource = soundAudioSource;
            }

            //Play parameters
            if (stopSoundBeforeNext) {
                //If already playing sound then stop it first
                if (_audioSource.IsPlaying) {
                    _audioSource.Stop();
                }
            }

            AudioClip _clip;
            //Pick audioClip
            switch (sound) {
                #region Player
                case Sound.playerJump:
                    _clip = playerJump[Random.Range(0, playerJump.Length - 1)];
                    break;
                case Sound.playerMeleeAttack:
                    _clip = playerMeleeAttack[Random.Range(0, playerMeleeAttack.Length - 1)];
                    break;
                case Sound.playerRangedAttack:
                    _clip = playerRangedAttack[Random.Range(0, playerRangedAttack.Length - 1)];
                    break;
                case Sound.playerHeal:
                    _clip = playerHeal[Random.Range(0, playerHeal.Length - 1)];
                    break;
                case Sound.playerUltimate:
                    _clip = playerUltimate[Random.Range(0, playerUltimate.Length - 1)];
                    break;
                case Sound.playerActivateBuilding:
                    _clip = playerActivateBuilding[Random.Range(0, playerActivateBuilding.Length - 1)];
                    break;
                case Sound.playerCombineBuildingFire:
                    _clip = playerCombineBuilding[0];
                    break;
                case Sound.playerCombineBuildingIce:
                    _clip = playerCombineBuilding[1];
                    break;
                case Sound.playerCombineBuildingBlast:
                    _clip = playerCombineBuilding[2];
                    break;

                #region Life & Death
                case Sound.playerTakeDamage:
                    _clip = playerTakeDamage[Random.Range(0, playerTakeDamage.Length - 1)];
                    break;
                case Sound.playerDeath:
                    _clip = playerDeath[Random.Range(0, playerDeath.Length - 1)];
                    break;
                case Sound.playerRevive:
                    _clip = playerRevive[Random.Range(0, playerRevive.Length - 1)];
                    break;
                case Sound.playerPickupSoul:
                    _clip = playerPickupSoul[Random.Range(0, playerPickupSoul.Length - 1)];
                    break;
                    #endregion
                #endregion

                #region Interactions
                case Sound.meleeImpact:
                    _clip = meleeImpacts[Random.Range(0, meleeImpacts.Length - 1)];
                    break;
                #endregion

                #region Enemy
                case Sound.enemyMeleeAttack:
                    _clip = enemyMeleeAttack[Random.Range(0, enemyMeleeAttack.Length - 1)];
                    break;
                case Sound.enemyRangedAttack:
                    _clip = enemyRangedAttack[Random.Range(0, enemyRangedAttack.Length - 1)];
                    break;
                case Sound.enemyBossAttack:
                    _clip = enemyBossAttack[Random.Range(0, enemyBossAttack.Length - 1)];
                    break;
                case Sound.enemyTakeDamage:
                    _clip = enemyTakeDamage[Random.Range(0, enemyTakeDamage.Length - 1)];
                    break;
                case Sound.enemyRagdoll:
                    _clip = enemyRagdoll[Random.Range(0, enemyRagdoll.Length - 1)];
                    break;
                #endregion

                #region Towers & Traps
                case Sound.sphereTakeDamage:
                    _clip = sphereTakeDamage[Random.Range(0, sphereTakeDamage.Length - 1)];
                    break;
                case Sound.towerTakeDamage:
                    _clip = towerTakeDamage[Random.Range(0, towerTakeDamage.Length - 1)];
                    break;

                //Player & Tower Projectiles
                case Sound.towerDefault:
                    _clip = towerProjectiles[0];
                    break;
                case Sound.towerFire:
                    _clip = towerProjectiles[1];
                    break;
                case Sound.towerIce:
                    _clip = towerProjectiles[2];
                    break;
                case Sound.towerBlast:
                    _clip = towerProjectiles[3];
                    break;

                //Player & Tower Projectiles
                case Sound.towerDefaultImpact:
                    _clip = towerProjectileImpacts[0];
                    break;
                case Sound.towerFireImpact:
                    _clip = towerProjectileImpacts[1];
                    break;
                case Sound.towerIceImpact:
                    _clip = towerProjectileImpacts[2];
                    break;
                case Sound.towerBlastImpact:
                    _clip = towerProjectileImpacts[3];
                    break;

                //Trap VFX
                case Sound.trapDefault:
                    _clip = trapVFX[0];
                    break;
                case Sound.trapFire:
                    _clip = trapVFX[1];
                    break;
                case Sound.trapIce:
                    _clip = trapVFX[2];
                    break;
                case Sound.trapBlast:
                    _clip = trapVFX[3];
                    break;
                #endregion

                default:
                    _clip = null;
                    break;
            }

            if (randomizeVolume) {
                _audioSource.Volume = Random.Range(_audioSource.Volume+volumeRandomization.x, _audioSource.Volume+volumeRandomization.y);
            }

            if (randomizePitch) {
                _audioSource.Pitch = Random.Range(_audioSource.Pitch+pitchRandomization.x, _audioSource.Pitch+pitchRandomization.y);
            }

            if (_clip != null) { 
                //Set clip
                _audioSource.AudioClip = _clip;

                //Play once
                _audioSource.Play();
            }
        //}
    }


    /// <summary>
    /// Switch the music to the provided clip
    /// </summary>
    /// <param name="_clip">Play provided _clip</param>
    public void PlayMusic(AudioClip _clip) {
        //Stop music if already playing
        if (musicAudioSource.IsPlaying) {
            musicAudioSource.Stop();
        }

        //Start provided music clip
        musicAudioSource.AudioClip = _clip;
        musicAudioSource.Play();
    }
}
