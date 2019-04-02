﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header ("Footsteps")]
    [SerializeField]
    private AudioClip[] normalFootsteps;
    [SerializeField]
    private AudioClip[] pathFootsteps;

    private AudioSource audioSource;
    private TerrainDetector terrainDetector;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        terrainDetector = new TerrainDetector();
    }

    public void FootstepSound() {
        //Execute this function from the animations when the foot hits the ground (& the character is grounded?)
        //AudioClip clip = GetRandomClip();
        //audioSource.PlayOneShot(clip);


        //if (!audioSource.isPlaying) {
            

        if (InputManager.Instance.Vertical == 1) {
            audioSource.PlayOneShot(normalFootsteps[0]);
            Debug.Log("Play footstep sound from: " + transform.name);
        }
        //}
    }

    private AudioClip GetRandomClip()
    {
        //Check which layer the player is on
        int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

        //Return a clip from the corresponding clips array
        switch(terrainTextureIndex)
        {
            /* case 0:
                return pathFootsteps[UnityEngine.Random.Range(0, pathFootsteps.Length)]; */
            default:
                return normalFootsteps[UnityEngine.Random.Range(0, normalFootsteps.Length)];
        }
        
    }
}
