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

    public int soulCost;

    [Tooltip("Layer of colliders to place Prefabs on ('Ground')")]
    [SerializeField]
    private LayerMask mask = 0;

    [Tooltip("Material used to color transparent Prefabs. Green if it's allowed to be placed at that location and Red if it's not")]
    //[SerializeField]
    private Material RedGreenMaterial;
    private Material OriginalMaterial1;
    private Material _Material; //Own material

    //Current selected prefab with assigned HotKey
    private GameObject currentPrefab;
    private int currentPrefabIndex = -1;


    [Header("Controls")]
    [Tooltip("HotKey to place assigned Prefabs")]
    [SerializeField]
    private KeyCode hotkey = KeyCode.Mouse0;

    [Tooltip("Maximum distance to place Prefabs")]
    [SerializeField]
    private float maxRayDistance = 1000;

    [Tooltip("Check to Instantiate Prefabs straight")]
    [SerializeField]
    private Boolean fixedAngle;

    [Tooltip("Check to place Prefabs in the middle of the screen")]
    [SerializeField]
    private Boolean fixedCameraPlacement;

    [Tooltip("Check to rotate Prefabs by a fixed value")]
    [SerializeField]
    private Boolean fixedRotation = true;

    private float mouseWheelRotation;
    [SerializeField]
    private float mouseWheelRotationMultiplier = 0.1f;



    //Todo: Clean these
    private bool placing, setColorToRed;
    private float halfScale;


    SoulStorage soulStorage;



    private MeshRenderer[] meshRenderers;
    #endregion

    private void Start()
    {
        soulStorage = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoulStorage>();
        soulStorage.costToBuild = soulCost;

        //Todo: save original materials to replace after placing individual Prefabs
        //OriginalMaterial + i = Prefabs[i].GetComponent<MeshRenderer>().material;
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
            //If pressed a number key between 1-9 instantiate corresponding Prefab
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                //If pressed again: reset
                if (PressedKeyOfCurrentPrefab(i))
                {
                    Destroy(currentPrefab);
                    currentPrefabIndex = -1;

                    GetComponent<MeleeAttack>().enabled = false;
                }
                else
                {
                    if (currentPrefab != null)
                    {
                        Destroy(currentPrefab);
                    }

                    currentPrefab = Instantiate(Prefabs[i]);
                    currentPrefabIndex = i;

                    placing = true;

                    //Save original Material & On Instantiation give the Prefab the RedGreenMaterial
                    OriginalMaterial1 = currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material;    //! 'OriginalMaterial' + i
                    currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load<Material>("RedGreenMaterial");


                    //currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshCollider>().isTrigger = false;    //Turn off collision

                    //GetComponent<MeleeAttack>().enabled = false;
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
            //Color Prefab green when on base terrain level (0), else color it red
            if (setColorToRed)
            {
                currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
            }
            else
            {
                if (currentPrefab.name != "TestRangeIndicator(Clone)")
                {
                    //if (currentPrefab.transform.position.y == halfScale) {
                    currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
                    //}
                    //else {
                    //  currentPrefab.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
                    //}
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
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        }
        else
        {
            //Cast a ray to the mouse position
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        if (Physics.Raycast(ray, out hit, maxRayDistance, mask))
        {
            //Move currentPrefab to rayCastHit position + half of its scale
            currentPrefab.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            //currentPrefab.transform.position = new Vector3(hit.point.x, hit.point.y + halfScale, hit.point.z);

            //Make it stand on hit Terrain with 90 degree angle
            if (!fixedAngle)
            {
                currentPrefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
        }
    }
    #endregion

    #region RotatePrefabByScrolling
    private void RotatePrefabByScrolling()
    {
        if (fixedRotation)
        {
            //Rotate the currentPrefab by scrolling the mouseWheel
            mouseWheelRotation = Input.mouseScrollDelta.y;
            currentPrefab.transform.Rotate(Vector3.up, mouseWheelRotation * mouseWheelRotationMultiplier * 10);

            //Reset rotation
            mouseWheelRotation = 0;
        }
        else
        {
            mouseWheelRotation += Input.mouseScrollDelta.y;
            currentPrefab.transform.Rotate(Vector3.up, mouseWheelRotation * mouseWheelRotationMultiplier);
        }

    }
    #endregion

    #region PlaceOnRelease
    private void PlacePrefabOnRelease()
    {
        //If hotKey is pressed again then reset currentObject
        if (Input.GetKeyDown(hotkey))
        {
            if (!setColorToRed)
            {
                //!Test
                //Debug.Log(currentPrefab.name);
                if (currentPrefab.name == "TestRangeIndicator(Clone)")
                {

                    Vector3 pos = new Vector3(currentPrefab.transform.position.x, currentPrefab.transform.position.y + 5, currentPrefab.transform.position.z);
                    Destroy(currentPrefab);
                    Instantiate(TestPrefabs[0], pos, transform.rotation);
                    //if target area object prefab
                    //then instantiate fireball above

                    //Reset
                    //currentPrefab = null;
                    placing = false;
                }
                //Normal behaviour
                else
                {
                    if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoulStorage>().soulCount > soulCost)
                    {
                        currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = OriginalMaterial1;    //Switch back to original Material
                        //currentPrefab.transform.GetChild(0).GetComponent<BoxCollider>().isTrigger = false;    //Turn on collision
                        currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshCollider>().isTrigger = false;    //Turn on collision
                        currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<PlacePrefabCollisionColor>().enabled = false;     //Disable OnTrigger script

                        currentPrefab.gameObject.layer = 11;    //Put on "Turrets" layer
                        soulStorage.substractCostsToBuild();

                        //Reset
                        currentPrefab = null;
                        placing = false;

                        GetComponent<MeleeAttack>().enabled = true;
                    }
                }
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
