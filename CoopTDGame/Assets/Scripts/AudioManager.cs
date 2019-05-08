using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    #region TEST
    //Define Enum
    public enum TestEnum{Test1, Test2, Test3};

    //This is what you need to show in the inspector.
    public TestEnum Tests;
    #endregion



    [SerializeField]
    [Tooltip ("Stop any sound before playing the next one")]
    private bool stopSoundBeforeNext;

    //private Component[] audioSources;
    [SerializeField]
    [Tooltip ("The first child of the object playing the sounds")]
    private AudioSource soundAudioSource;
    [SerializeField]
    [Tooltip ("The second child of the object playing the music (looping)")]
    private AudioSource musicAudioSource;

    void Start()
    {
        //Get audio sources
        //audioSources = GetComponents(typeof(AudioSource));
        //If not assigned in editor
        if (soundAudioSource == null) {
            //If there is no audio source: add one
            //if (audioSources.Length == 0) {
                soundAudioSource = gameObject.AddComponent<AudioSource>();
                soundAudioSource.playOnAwake = false;
            //}
            /* else
            {
                //Assign the first one to SOUND
                soundAudioSource = audioSources[0].GetComponent<AudioSource>(); 
            } */
        }

        if (musicAudioSource == null) {
            //If there is no audio source: add one
            //if (audioSources.Length == 0 || audioSources.Length == 1) {
                musicAudioSource = gameObject.AddComponent<AudioSource>();
                musicAudioSource.loop = true;
            //}
        }
    }

    /// <summary>
    /// Switch the clip of the audio source to the provided one and play it on the passed audio source
    /// or on it's own when it's null
    /// </summary>
    /// <param name="_source">Use provided audio source or own audio source when passed null</param>
    /// <param name="_clip">Play provided _clip once</param>
    public void PlaySound(AudioSource _source, AudioClip _clip) {
        //Set audio source
        AudioSource _audioSource;
        if (_source != null) {
            _audioSource = _source;
        }
        else
        {
            _audioSource = soundAudioSource;
        }

        //Set clip
        _audioSource.clip = _clip;

        //Play parameters
        if (stopSoundBeforeNext) {
            //If already playing sound then stop it first
            if (_audioSource.isPlaying) {
                _audioSource.Stop();
            }
        }

        //Play once
        _audioSource.PlayOneShot(_audioSource.clip);
    }


    /// <summary>
    /// Switch the music to the provided clip
    /// </summary>
    /// <param name="_clip">Play provided _clip</param>
    public void PlayMusic(AudioClip _clip) {
        //Stop music if already playing
        if (musicAudioSource.isPlaying) {
            musicAudioSource.Stop();
        }

        //Start provided music clip
        musicAudioSource.clip = _clip;
        musicAudioSource.Play();
    }
}
