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

    /* [Tooltip ("Cost to place currently selected tower")]
    public int soulCost; */

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
    /* [Tooltip("HotKey to place assigned Prefabs")]
    [SerializeField]
    private KeyCode hotkey = KeyCode.Mouse0; */

    [Tooltip("Maximum distance to place Prefabs")]
    [SerializeField]
    private float maxRayDistance = 100;

    [Tooltip("Check to Instantiate Prefabs straight")]
    [SerializeField]
    private Boolean fixedAngle = false;

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
    private bool setColorToRed;
    #endregion

    #region Internal
    private float halfScale;
    private MeshRenderer[] meshRenderers;
    
    private PlayerController playC;
    private Animator playerAnim;
    private bool _input;
    private bool _transferInput;
    private bool _enterBuildMode, _spawnTowerMode;
    private bool _build1, _build2, _build3;
    private bool enteredBuildMode;
    private Camera MainCamera;
    #endregion
    #endregion

    private void Start()
    {
        #region References
        playC = GetComponent<PlayerController>();
        playerAnim = GetComponent<Animator>();

        

        /* _meleeAttack = GetComponent<MeleeAttack>();
        _rangedAttack = GetComponent<RangedAttack>(); */
        #endregion
    }

    private void Update()
    {
        if (playC.Player_ == 0) {
        checkHotKeys(); //Instantiates Prefab & sets it to currentPrefab to use for following functions
        if (currentPrefab != null)
        {
            //Get halfScale in update to check transform.y against it & place appropriately because the anchor is centered
            halfScale = currentPrefab.transform.localScale.y / 2f;

            MovePrefabToRayHit();
            ChangeMaterialColor();
            RotatePrefabByScrolling();

            #region Input check for placement
            if (playC.Player_ == 0) {
                //_enterBuildMode = Input.GetKeyDown(KeyCode.Alpha0 + 1 + i);
                _input = InputManager.Instance.Fire1;
                _transferInput = Input.GetKey(KeyCode.C);
            } 

            if (_input) {
                PlacePrefabOnRelease();
            }
            #endregion
        }
        }























        if (playC.Player_ == 1) {
            _enterBuildMode = InputManager.Instance.BuildMode1;
            _build1 = InputManager.Instance.Heal1;
            _build2 = InputManager.Instance.Ultimate1;
            _build3 = InputManager.Instance.Slash1;

            _transferInput = InputManager.Instance.isRunning1;
        }
        else if (playC.Player_ == 2) {
            _enterBuildMode = InputManager.Instance.BuildMode2;
            _build1 = InputManager.Instance.Heal2;
            _build2 = InputManager.Instance.Ultimate2;
            _build3 = InputManager.Instance.Slash2;

            _transferInput = InputManager.Instance.isRunning2;
        }

        //Enter build mode
        if (_enterBuildMode && !playC.isRangedAttacking && !playC.isMeleeAttacking && !playC.isJumping && !playC.isDead) {
            if (_transferInput) {
                
                GameObject.FindObjectOfType<soulTransfer>().InputHandler(GetComponent<Animator>());
            }
            else if (!_transferInput) {
                //playerAnim.SetBool("Channeling", false);
            



            playC.isInBuildMode = true;
            /* Debug.Log("buidö1: "+_build1); */
            //if pressed x, y, b set current prefab (enter spawnturrentmode)
            if (_build1) {
                /* Destroy(currentPrefab);
                    currentPrefabIndex = -1; */

                if (currentPrefabIndex != -1) {
                    //If 0 is already selected when clicking x then destroy it
                    Destroy(currentPrefab);
                    currentPrefabIndex = -1;

                    _spawnTowerMode = false;
                }
                else {
                    //Else instanstaite it
                    _spawnTowerMode = true;
                    currentPrefab = Instantiate(Prefabs[0]);
                    currentPrefabIndex = 0;
                    currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<PlacePrefabCollisionColor>().placeByPlayer_ = playC.Player_; 

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
                }                
            }
            if (_build2) {
                if (currentPrefabIndex != -1) {
                    //If 0 is already selected when clicking x then destroy it
                    Destroy(currentPrefab);
                    currentPrefabIndex = -1;

                    _spawnTowerMode = false;
                }
                else {
                    //Else instanstaite it
                    _spawnTowerMode = true;
                    currentPrefab = Instantiate(Prefabs[1]);
                    currentPrefabIndex = 1;
                    currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<PlacePrefabCollisionColor>().placeByPlayer_ = playC.Player_; 

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
                }                
            }
            if (_build3) {
                if (currentPrefabIndex != -1) {
                    //If 0 is already selected when clicking x then destroy it
                    Destroy(currentPrefab);
                    currentPrefabIndex = -1;

                    _spawnTowerMode = false;
                }
                else {
                    //Else instanstaite it
                    _spawnTowerMode = true;
                    currentPrefab = Instantiate(Prefabs[2]);
                    currentPrefabIndex = 2;
                    currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<PlacePrefabCollisionColor>().placeByPlayer_ = playC.Player_; 

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
                }                
            }

            /* else if (_build2) {
                _spawnTowerMode = true;
                currentPrefab = Instantiate(Prefabs[1]);
                currentPrefabIndex = 1;
            }
            else if (_build3) {
                _spawnTowerMode = true;
                currentPrefab = Instantiate(Prefabs[2]);
                currentPrefabIndex = 2;
            } */

             //If in spawnturrentmode move the instantiate prefab
            if (_spawnTowerMode)
            {
                //If in spawnturrentmode then when clicking the same option again, kill that prefab and exit the mode
                /* if (_build1) {
                    Destroy(currentPrefab);
                    currentPrefabIndex = -1;

                    _spawnTowerMode = false;
                }
                else if (_build2) {
                    Destroy(currentPrefab);
                    currentPrefabIndex = -1;

                    _spawnTowerMode = false;
                }
                else if (_build3) {
                    Destroy(currentPrefab);
                    currentPrefabIndex = -1;

                    _spawnTowerMode = false;
                } */

                //else {
                //Get halfScale in update to check transform.y against it & place appropriately because the anchor is centered
                //halfScale = currentPrefab.transform.localScale.y / 2f;

                if (currentPrefab != null) {
                    /* Debug.Log("currentPrefab present"); */
                MovePrefabToRayHit();
                ChangeMaterialColor();
                RotatePrefabByScrolling();
                }


            } 
        
        //On exiting building mode, if current prefab is not null the place it
        else {
            playC.isInBuildMode = false;
            //if not in spawnturrentmode set an invalid index n exit build mode
            if (currentPrefab == null) {
                //Debug.Log("DEFSEGJKESNEJFLNSPOOOOOOOOOOOOOOY");
                currentPrefabIndex = -1;
                //Destroy(currentPrefab);
            }
            //Place it!
            else {
                if (!setColorToRed)
            {
                PlacePrefabOnRelease();
                
            }
            else {
                Debug.Log("sydggsdrbtxzsrtzhrtsjdrgsdtrgberz");
                currentPrefabIndex = -1;
                Destroy(currentPrefab);
            }
            }
                /* Destroy(currentPrefab);
                currentPrefabIndex = -1;

                playC.isInBuildMode = false; */

                /* _meleeAttack.enabled = true;     //reenable melee combat & ranged combat
                _rangedAttack.enabled = true; */
            //}
        }
        }
        }
    }


        
    //}

    #region checkHotKeys
    private void checkHotKeys()
    {
        for (int i = 0; i < Prefabs.Length; i++)
        {
            if (playC.Player_ == 0) {
                //If a number key between 1-9 is pressed, instantiate corresponding Prefab
                if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
                {
                    //If pressed again: reset
                    if (PressedKeyOfCurrentPrefab(i))
                    {
                        Destroy(currentPrefab);
                        currentPrefabIndex = -1;

                        playC.isInBuildMode = false;

                        /* _meleeAttack.enabled = true;     //reenable melee combat & ranged combat
                        _rangedAttack.enabled = true; */
                    }
                    else
                    {
                        playC.isInBuildMode = true;

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

                        /* _meleeAttack.enabled = false;    //disable melee combat & ranged combat   
                        _rangedAttack.enabled = false;   */               
                    }

                    break;
                }
            }
            else if (playC.Player_ == 1) {
                //Enter build mode
                /* Debug.Log(_enterBuildMode); */
                if (_enterBuildMode) {
                    if (!playC.isInBuildMode) {
                        //enteredBuildMode = true;

                        playC.isInBuildMode = true;

                        if (currentPrefab != null)
                        {
                            Destroy(currentPrefab);
                        }

                        if (InputManager.Instance.Heal1) {
                            currentPrefab = Instantiate(Prefabs[0]);
                            currentPrefabIndex = 0;
                            currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<PlacePrefabCollisionColor>().placeByPlayer_ = playC.Player_; 
                        }
                        else if (InputManager.Instance.Ultimate) {
                            currentPrefab = Instantiate(Prefabs[1]);
                            currentPrefabIndex = 1;
                            currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<PlacePrefabCollisionColor>().placeByPlayer_ = playC.Player_; 
                        }
                        else if (InputManager.Instance.Slash1) {
                            currentPrefab = Instantiate(Prefabs[2]);
                            currentPrefabIndex = 2;
                            currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<PlacePrefabCollisionColor>().placeByPlayer_ = playC.Player_; 
                        }

                        /* currentPrefab = Instantiate(Prefabs[i]);
                        currentPrefabIndex = i; */

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
                    }
                    //Pressed again
                    else {
                        Destroy(currentPrefab);
                        currentPrefabIndex = -1;

                        playC.isInBuildMode = false;
                    }
                }
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
        /* Debug.Log("ChangeMaterialColor: "+setColorToRed); */
        /* if (playC.isInBuildMode)
        { */
            if (setColorToRed)
            {
                if (currentPrefab.transform.childCount > 0) {
                    currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
                }
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
        /* } */
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
            string tag = "MainCamera";
        if (playC.Player_ == 2)
        {
            tag = "MainCamera2";
        }
        Debug.Log("Finding " + tag + " tag");
        MainCamera = GameObject.FindGameObjectWithTag(tag).GetComponent<Camera>();
            ray = MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        }
        else
        {
            //Cast a ray to the mouse position
            ray = MainCamera.ScreenPointToRay(Input.mousePosition);
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
        /* if (_input)
        { */
            if (!setColorToRed)
            {
                #region TEST
                if (currentPrefab.name == "TestRangeIndicator(Clone)")
                {

                    Vector3 pos = new Vector3(currentPrefab.transform.position.x, currentPrefab.transform.position.y + 5, currentPrefab.transform.position.z);
                    Destroy(currentPrefab);
                    Instantiate(TestPrefabs[0], pos, transform.rotation);
                    //if target area object prefab
                    //then instantiate fireball above

                    GetComponent<Animator>().SetTrigger("RangedAttack");

                    //Reset
                    //currentPrefab = null;
                    playC.isInBuildMode = false;
                }
                #endregion

                #region Normal behaviour
                else
                {
                    if (SoulStorage.Instance.soulCount > SoulStorage.Instance.costToBuild)   //If enough souls
                    {
                        if (!setColorToRed)
            {
                        if (currentPrefab.transform.childCount > 0) {
                            //Switch back to original Material
                            currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = OriginalMaterial;
                            //Turn on collision   
                            currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<MeshCollider>().isTrigger = false;  
                            //Disable OnTrigger script           
                            currentPrefab.transform.GetChild(0).GetChild(0).GetComponent<PlacePrefabCollisionColor>().enabled = false; 

                            currentPrefab.gameObject.layer = 11;            //Put on "Turrets" layer to prevent casting ray on itself
                        }

                        
                        SoulStorage.Instance.substractCostsToBuild();   //Subtract souls

                        //Reset
                        currentPrefab = null;
                        //placing = false;
                        playC.isInBuildMode = false;
                        /* _meleeAttack.enabled = true;
                        _rangedAttack.enabled = true; */
            }
                    }
                }
                #endregion
            }
        //}
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
