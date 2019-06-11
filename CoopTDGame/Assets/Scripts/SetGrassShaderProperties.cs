using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetGrassShaderProperties : MonoBehaviour
{
    [Header ("Set Grass Properties (on start, else on update)")]
    [SerializeField]
    private bool SetGrassShaderPropertiesOnStart;

    [SerializeField]
    //! Not actually billboarding yet, but just disabling wind & player collision interaction
    private float _GrassBillboardingDistance = 50f;


    [Header ("Texture")]
    [SerializeField]
    [Range (0, 1)]
    private float _GrassAlphaClip = 0.5f;
    [SerializeField]
    [Range (0, 1)]
    private float _GrassMetallicness = 0f;
    [SerializeField]
    [Range (0, 1)]
    private float _GrassSmoothness = 0.5f;
    [SerializeField]
    [Range (0, 1)]
    private float _GrassAmbientOcclusion = 1f;

    [Header ("Interactivity")]
    [SerializeField]
    private float _GrassRadius = 5f;
    [SerializeField]
    [Range (0, 15)]
    private float _GrassYOffset = 5f;

    [Header ("Wind")]
    [SerializeField]
    private Vector2 _GrassWindMovement = new Vector2(10, 0);
    [SerializeField]
    private float _GrassWindDensity = 0.03f;
    [SerializeField]
    private float _GrassWindStrength = 2f;
    
    void Start()
    {
        if (SetGrassShaderPropertiesOnStart) {
            _SetGrassShaderProperties();
        }
    }

    void Update()
    {
        if (!SetGrassShaderPropertiesOnStart) {
            _SetGrassShaderProperties();
        }
    }

    void _SetGrassShaderProperties() {
        //Set billboarding distance of all grass for both players at start 
        Shader.SetGlobalFloat("_GrassBillboardingDistance", _GrassBillboardingDistance);

        Shader.SetGlobalFloat("_GrassAlphaClip", _GrassAlphaClip);
        Shader.SetGlobalFloat("_GrassMetallicness", _GrassMetallicness);
        Shader.SetGlobalFloat("_GrassSmoothness", _GrassSmoothness);
        Shader.SetGlobalFloat("_GrassAmbientOcclusion", _GrassAmbientOcclusion);

        Shader.SetGlobalFloat("_GrassRadius", _GrassRadius);
        Shader.SetGlobalFloat("_GrassYOffset", _GrassYOffset);

        Shader.SetGlobalVector("_GrassWindMovement", _GrassWindMovement);
        Shader.SetGlobalFloat("_GrassWindDensity", _GrassWindDensity);
        Shader.SetGlobalFloat("_GrassWindStrength", _GrassWindStrength);
    }
}
