using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MultiAudioListener;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    [Tooltip ("Stop any sound before playing the next one")]
    private bool stopSoundBeforeNext;

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
    private AudioClip ingameMusicAudioClip;
    

    [Header("Movement Sounds")]
    [Header("Player Sounds")]
    public AudioClip playerJump;
    //public AudioClip playerLand;

    [Header("Attacks")]
    public AudioClip[] playerMeleeAttack;
    public AudioClip playerUltimateAttack;
    //public AudioClip playerRangedAttack;

    [Header("Spells")]
    public AudioClip playerHeal;
    public AudioClip playerUltimateActivation;

    [Header("Damaged")]
    public AudioClip playerTakingDamage;
    public AudioClip playerTakingHighDamage;

    [Header("Interactions")]
    public AudioClip playerActivateBuilding;
    public AudioClip playerPickupSoul;

    
    [Header ("Enemies")]
    [Space (15)]
    public AudioClip[] enemyTakingDamage;

    
    [Header ("Towers & Traps")]
    [Space (15)]
    public AudioClip towerTakingDamage;
    [Tooltip ("Order: Default, Fire, Ice, Blast")]
    public AudioClip[] towerProjectiles;

    public AudioClip trapFire, trapIce, trapBlast;



    void Start()
    {
        if (playMenuMusicOnStart) {
            PlayMusic(menuMusicAudioClip);
        }
    }

    /// <summary>
    /// Switch the clip of the audio source to the provided one and play it on the passed audio source
    /// or on it's own when it's null
    /// </summary>
    /// <param name="_source">Use provided audio source or own audio source when passed null</param>
    /// <param name="_clip">Play provided _clip once</param>
    public void PlaySound(MultiAudioSource _source, AudioClip _clip) {
        if (_clip != null) {
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

            //Set clip
            _audioSource.AudioClip = _clip;

            //Play once
            _audioSource.Play();
        }
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
