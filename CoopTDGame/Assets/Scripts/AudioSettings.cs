using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mainMixer;

    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("musicVol", volume);
    }

    public void setEffectVolume(float volume)
    {
        mainMixer.SetFloat("effectVol", volume);
    }
}
