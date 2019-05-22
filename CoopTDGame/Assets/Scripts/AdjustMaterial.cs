using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMaterial : MonoBehaviour
{
    [Header ("Material property strings")]
    [SerializeField]
    private string dissolveString = "_Dissolve";
    [SerializeField]
    private string burnString = "_Flames";
    [SerializeField]
    private string freezeString = "_Ice";

    [Header ("VFX")]
    [SerializeField]
    private GameObject burnVFX;
    [SerializeField]
    private GameObject freezeVFX;

    [Header ("Options")]
    [SerializeField]    
    private bool useMeshRenderer;
    private MeshRenderer meshRenderer;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Material mat;
    private MeshRenderer weaponRenderer;
    private Material weaponMat;

    
    private bool dissolved, burned, frozen;
    private bool startDissolving, startBurning, startFreezing;
    private float dissolveLerp, burnLerp, freezeLerp;
    private float _dissolveDuration, _burnDuration, _freezeDuration;

    private Vector2 lerpValues = new Vector2(0, 1);

    void Start()
    {
        if (useMeshRenderer) {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            mat = meshRenderer.material;
        }
        else {
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            mat = skinnedMeshRenderer.material;
        }

        weaponRenderer = GetComponent<StatusEffect>().weaponRenderer;
        if (weaponRenderer != null) {
            weaponMat = weaponRenderer.material;
        }
    }

    void Update()
    {
        #region DISSOLVE
        if (startDissolving) {
            Dissolving();
        }
        #endregion

        #region BLAST
        if (startFreezing & startBurning) {
            Burning();
            Freezing();
        }
        #endregion

        #region BURN
        else if (startBurning) {
            Burning();
        }
        #endregion

        #region FREEZE
        else if (startFreezing) {
            Freezing();
        }
        #endregion
    }

    private void _AdjustMaterial(string property, bool includeWeapon, float lerpTime, bool reverse) {
        if (reverse) {
            lerpValues.x = 1;
            lerpValues.y = 0;
        }
        else
        {
            lerpValues.x = 0;
            lerpValues.y = 1;
        }


        mat.SetFloat(property, Mathf.Lerp(lerpValues.x, lerpValues.y, lerpTime));

        if (includeWeapon) {
            weaponMat.SetFloat(property, Mathf.Lerp(lerpValues.x, lerpValues.y, lerpTime));
        }
    }

    public void Dissolve(float duration) {
        _dissolveDuration = duration;
        startDissolving = true;
    }

    public void Burn(float duration) {
        _burnDuration = duration;
        startBurning = true;
    }

    public void Freeze(float duration) {
        _freezeDuration = duration;
        startFreezing = true;
    }

    private void Dissolving() {
        _AdjustMaterial(dissolveString, true, dissolveLerp, dissolved);
            
        if (dissolveLerp < 1) { //While lerp is below the end limit
            //Increment it at the desired rate every frame
            dissolveLerp += Time.deltaTime / _dissolveDuration;
        }
        else
        {
            dissolved = !dissolved;
        }
    }

    private void Burning() {
        _AdjustMaterial(burnString, false, burnLerp, burned);
        burnVFX.SetActive(!burned);
        
        if (burnLerp < 1) { //While lerp is below the end limit
            //Increment it at the desired rate every frame
            burnLerp += Time.deltaTime / _burnDuration;
        }
        else
        {
            burnLerp = 0;
            startBurning = false;
            burned = !burned;
        }
    }

    private void Freezing() {
        _AdjustMaterial(freezeString, false, freezeLerp, frozen);
        freezeVFX.SetActive(!frozen);
        
        if (freezeLerp < 1) { //While lerp is below the end limit
            //Increment it at the desired rate every frame
            freezeLerp += Time.deltaTime / _freezeDuration;
        }
        else
        {
            freezeLerp = 0;
            startFreezing = false;
            frozen = !frozen;
        }
    }
}
