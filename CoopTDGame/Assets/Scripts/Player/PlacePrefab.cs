using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePrefab : MonoBehaviour
{
    #region Variables
    //[Header("Prefabs")]
    [Tooltip("Prefabs to place with number keys 1-9 on colliders with assigned HotKey")]
    [SerializeField]
    private GameObject[] Prefabs;

    public GameObject[] TestPrefabs;

    [Tooltip ("Cost to place currently selected tower")]
    public int soulCost;

    [Tooltip("Layer of colliders to place Prefabs on ('Ground')")]
    [SerializeField]
    private LayerMask mask = 0;

    [Tooltip("Material used to color transparent Prefabs. Green if it's allowed to be placed at that location and Red if it's not")]
    //[SerializeField]
    private Material RedGreenMaterial;
    private Material OriginalMaterial;
    private Material _Material; //Own material

    //Current selected prefab with assigned HotKey
    private GameObject currentPrefab;
    private Transform currentPrefabChild;
    private int currentPrefabIndex = -1;


    [Header("Controls")]
    [Tooltip("HotKey to place assigned Prefabs")]
    [SerializeField]
    private KeyCode hotkey = KeyCode.Mouse0;

    [Tooltip("Maximum distance to place Prefabs")]
    [SerializeField]
    private float maxRayDistance = 100;

    [Tooltip("Check to Instantiate Prefabs straight")]
    [SerializeField]
    private Boolean fixedAngle;

    [Tooltip("Check to place Prefabs in the middle of the screen")]
    [SerializeField]
    private Boolean fixedCameraPlacement;

    [Tooltip("Check to rotate Prefabs by a fixed value")]
    [SerializeField]
    private Boolean fixedRotation = true;

    [Tooltip ("Multiplier to multiply mouse wheel rotation speed by")]
    [SerializeField]
    private float mouseWheelRotationMultiplier = 0.5f;
    private float mouseWheelRotation;


    #region Booleans
    private bool placing, setColorToRed;
    #endregion

    #region Internal
    private float halfScale;
    private MeshRenderer[] meshRenderers;
    private MeleeAttack _meleeAttack;

    private Camera CameraM;
    #endregion
    #endregion

    private void Start()
    {
        //soulStorage = GameObject.FindGameObjectWithTag("GameManager").GetComponentInChildren<SoulStorage>();
        //! --> this instead:
        SoulStorage.Instance.costToBuild = soulCost;

        #region References
        CameraM = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _meleeAttack = GetComponent<MeleeAttack>();
        #endregion
    }

    private void Update()
    {
        checkHotKeys(); //Instantiates Prefab & sets it to currentPrefab to use for following functions
        if (currentPrefab != null)
        {
            //Get halfScale in update to check transform.y against it & place appropriately because the anchor is centered
            halfScale = currentPrefab.transform.localScale.y / 2f;

            MovePrefabToRayHit();
            ChangeMaterialColor();
            RotatePrefabByScrolling();
            PlacePrefabOnRelease();
        }
    }

    #region checkHotKeys
    private void checkHotKeys()
    {
        for (int i = 0; i < Prefabs.Length; i++)
        {
            //If a number key between 1-9 is pressed, instantiate corresponding Prefab
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                //If pressed again: reset
                if (PressedKeyOfCurrentPrefab(i))
                {
                    Destroy(currentPrefab);
                    currentPrefabIndex = -1;

                    _meleeAttack.enabled = true;     //reenable melee combat & ranged combat
                }
                else
                {
                    placing = true;

                    if (currentPrefab != null)
                    {
                        Destroy(currentPrefab);
                    }

                    currentPrefab = Instantiate(Prefabs[i]);
                    currentPrefabIndex = i;

                    //Save original Material & On Instantiation give the Prefab the RedGreenMaterial
                    //currentPrefabChild = currentPrefab.transform.GetChild(0).GetChild(0);
                    if (currentPrefab.transform != null) {
                        if (currentPrefab.transform.childCount > 0) {
                    if (currentPrefab.transform.GetChild(0) != null) {
                    if (currentPrefab.transform.GetChild(0).GetChild(0) != null) {
                        OriginalMaterial = currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material;
                        currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("RedGreenMaterial");
                        //currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshCollider>().isTrigger = false;    //Turn off collision
                    }
                    }
                    }
                    }

                    _meleeAttack.enabled = false;    //disable melee combat & ranged combat                    
                }

                break;
            }
        }
    }

    private bool PressedKeyOfCurrentPrefab(int i)
    {
        return currentPrefab != null && currentPrefabIndex == i;
    }
    #endregion

    #region ChangeMaterialColor
    private void ChangeMaterialColor()
    {
        if (placing)
        {
            if (setColorToRed)
            {
                currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
            }
            else
            {
                if (currentPrefab.name != "TestRangeIndicator(Clone)")
                {
                    //currentPrefabChild = 
                    if (currentPrefab != null) {
                        if (currentPrefab.transform.childCount > 0) {
                    if (currentPrefab.transform.GetChild(0).GetChild(0) != null) {
                        currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
                    }
                    }
                    }
                }
            }
        }
    }
    #endregion

    #region MovePrefabToRayHit
    private void MovePrefabToRayHit()
    {
        Ray ray;
        RaycastHit hit;

        if (fixedCameraPlacement)
        {
            //Cast a ray to the middle of the screen
            ray = CameraM.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        }
        else
        {
            //Cast a ray to the mouse position
            ray = CameraM.ScreenPointToRay(Input.mousePosition);
        }

        if (Physics.Raycast(ray, out hit, maxRayDistance, mask))
        {
            //Move currentPrefab to rayCastHit position
            currentPrefab.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            if (!fixedAngle)
            {
                //Change rotation
                currentPrefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
        }
    }
    #endregion

    #region RotatePrefabByScrolling
    private void RotatePrefabByScrolling()
    {
        //Rotate the currentPrefab by scrolling the mouseWheel
        mouseWheelRotation = Input.mouseScrollDelta.y;
        currentPrefab.transform.Rotate(Vector3.up, mouseWheelRotation * mouseWheelRotationMultiplier * 10);

        if (fixedRotation) {
            //Reset rotation
            mouseWheelRotation = 0;
        }
    }
    #endregion

    #region PlaceOnRelease
    private void PlacePrefabOnRelease()
    {
        //If hotKey is pressed, place prefab if allowed
        if (Input.GetKeyDown(hotkey))
        {
            if (!setColorToRed)
            {
                #region TEST
                //Debug.Log(currentPrefab.name);
                if (currentPrefab.name == "TestRangeIndicator(Clone)")
                {

                    Vector3 pos = new Vector3(currentPrefab.transform.position.x, currentPrefab.transform.position.y + 5, currentPrefab.transform.position.z);
                    Destroy(currentPrefab);
                    Instantiate(TestPrefabs[0], pos, transform.rotation);
                    //if target area object prefab
                    //then instantiate fireball above

                    GetComponent<Animator>().SetTrigger("MagicAttack");

                    //Reset
                    //currentPrefab = null;
                    placing = false;
                }
                #endregion

                #region Normal behaviour
                else
                {
                    if (SoulStorage.Instance.soulCount > soulCost)           //If enough souls
                    {
                        if (currentPrefab.transform.childCount > 0) {
                        currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = OriginalMaterial;   //Switch back to original Material
                        currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshCollider>().isTrigger = false;             //Turn on collision
                        currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<PlacePrefabCollisionColor>().enabled = false;  //Disable OnTrigger script
                        }

                        currentPrefab.gameObject.layer = 11;    //Put on "Turrets" layer
                        SoulStorage.Instance.substractCostsToBuild();    //Subtract souls

                        //Reset
                        currentPrefab = null;
                        placing = false;
                        _meleeAttack.enabled = true;
                    }
                }
                #endregion
            }
        }
    }
    #endregion

    #region Public SetColorToRed
    public void SetColorToRed(bool red)
    {
        if (red)
        {
            setColorToRed = true;
        }
        else
        {
            setColorToRed = false;
        }
    }
    #endregion
}
