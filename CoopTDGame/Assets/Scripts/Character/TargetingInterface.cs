using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Utility;

public class TargetingInterface : MonoBehaviour{

    #region  Public editor Settings 
    [Header("Settings")]
    [Tooltip("Prints debug information to the console and draws certain gizmos")]
    public bool debug = false;
    [Tooltip("Exact Tag which should be targeted \nConsiders capitalization!")]
    public List<string> _TargetTags = new List<string>();

    [Tooltip("Character point of vision \nOptional!")]
    Transform _CharacterPointOfView;

    [Range(1, 500)]
    public float _TargetRange = 50f;

    [Range(1, 500)]
    public float _CombatRange = 10f;

    [Range(0, 360)]
    public float rotationSpeed = 10f;

    #endregion

    #region  Accessible variables & lists

    [HideInInspector]
    public bool rotateToTarget = false;

    [HideInInspector]
    public GameObject _CurrentTarget;
    public TargetInfo _CurrentTargetInfo;

    [HideInInspector]
    public List<GameObject> _TargetsInRange = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> _AllTargets = new List<GameObject>();

    #endregion 

    #region Functional variables
    Camera _CurrentCam;
    GameObject _TargetContainer;
    float _CurrentTargetDistance;
    LayerMask _IgnoreSelfLayer;
    #endregion

    #region CM Controlls for Player
    Cinemachine.CinemachineTargetGroup CM_TargetGroup;
    #endregion

    #region Setup

    private void Start() {

        if(GameObject.FindGameObjectWithTag("MainCamera")){
            _CurrentCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        } else _CurrentCam = Camera.main;

        CM_TargetGroup = FindObjectOfType<Cinemachine.CinemachineTargetGroup>();

        _IgnoreSelfLayer = transform.gameObject.layer;

        if(_CharacterPointOfView == null)_CharacterPointOfView = transform;

        RefershAllTargets();
    }
    #endregion

    #region GameUpdates

    private void Update() {
        if(debug){
            IndicateClosestTargetInfo(true, false, true, false);
            
            //! Only for dev testing
            /* if(_CurrentTargetInfo.t_Object){
                //Debug.Log("hasTarget");
                if(_CurrentTargetInfo.GetDistanceToObject(transform) < _CombatRange){
                    //Debug.Log("inRange");
                    rotateToTarget = true;
                } else rotateToTarget = false;
            } else  rotateToTarget = false; */
        }
    }

   
    private void LateUpdate() {

        //Debug.Log("rtt: " + rotateToTarget + " | ctnn: " + (_CurrentTargetInfo.t_Object != null));

        if(rotateToTarget && _CurrentTargetInfo.t_Object != null){
            //transform.LookAt(_CurrentTarget.transform.position);
            RotateToCurrentTarget();
        }
    }
    #endregion

    #region Character & Camera controll Functions 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logTargetDirection"></param>
    public void RotateToCurrentTarget(bool logTargetDirection = false){

        //Debug.Log("called rtt");

        if(null == _CurrentTargetInfo.t_Object) {
            rotateToTarget = false;
            return;
        } else {
            Vector3 _CurrentTargetDirection = _CurrentTargetInfo.t_transform.position - transform.position;
            Quaternion _CurrentTargetRotation = Quaternion.LookRotation(_CurrentTargetDirection);

            if(logTargetDirection) Debug.Log("Direction to taget is: " + _CurrentTargetDirection);

            transform.rotation = Quaternion.Lerp(transform.rotation, _CurrentTargetRotation, rotationSpeed * Time.deltaTime);
        }

    }

    /// <summary> DOES NOTHING IF NOT CALLED BY PLAYER
    /// </summary>
    /// <param name="target"></param>
    public void ResetCMFocusTarget(GameObject target){
        
        if(transform.gameObject.tag == "Player"){
            Transform _FocusTarget = target.transform;

            int targetSelf = 0;

            CM_TargetGroup.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[2];

            if(_FocusTarget != null){
                targetSelf = 1;

                CM_TargetGroup.m_Targets[0].target = _FocusTarget;
                CM_TargetGroup.m_Targets[0].weight = 1;
                CM_TargetGroup.m_Targets[0].radius = 2;
            }

            CM_TargetGroup.m_Targets[targetSelf].target = transform;
            CM_TargetGroup.m_Targets[targetSelf].weight = 1;
            CM_TargetGroup.m_Targets[targetSelf].radius = 2;}
    }
    
    /// <summary> Function to refresh </summary>
    public void RefershAllTargets(){
        _AllTargets = FindAllTargets();
    }

    #endregion

    #region Bools for status checks 

    /// <summary> xxx
    /// 
    /// </summary>
    /// <returns></returns>
    public bool AnyTargetInRange(){
        if(FindClosestTarget() != null) return true;
        else return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="castFromCharacterPovInstad"></param>
    /// <param name="debug"></param>
    /// <returns></returns>
    public bool AnyTargetsInVision(bool castFromCharacterPovInstad = false, bool debug = false){

        if(FindAllTargetInVision(castFromCharacterPovInstad, debug).Count > 0)return true;
        else return false;

    }

    /// <summary> xxx
    /// 
    /// </summary>
    /// <param name="castFromCharacterPovInstad"></param>
    /// <param name="debug"></param>
    /// <returns></returns>
    public bool CurrentTargetVisible(bool castFromCharacterPovInstad = false, bool debug = false){
        
        Vector3 pointOfVision;

        if(castFromCharacterPovInstad) pointOfVision = _CharacterPointOfView.position; 
        else pointOfVision = Camera.main.transform.position;

        if(_CurrentTargetInfo == null) return false;
        else if(_CurrentTargetInfo.IsVisibleFrom(pointOfVision, _TargetRange)) return true;
        else return false;        
    }

    #endregion

    #region Find Functions - Public lists of accessible targets

    /// <summary>
    /// 
    /// </summary>
    /// <param name="castFromCharacterPovInstad"></param>
    /// <param name="debug"></param>
    /// <returns></returns>
    public List<GameObject> FindAllTargetInVision(bool castFromCharacterPovInstad = false, bool debug = false){

        List<GameObject> targetsInRange = FindAllTargetsInRange(_TargetRange);

        List<GameObject> _TargetsInVision = new List<GameObject>();

        Vector3 pointOfVision;

        if(castFromCharacterPovInstad) pointOfVision = _CharacterPointOfView.position; 
        else pointOfVision = Camera.main.transform.position;

        foreach (GameObject target in targetsInRange){
            
            if(null == target) continue;
            else _CurrentTargetInfo = new TargetInfo(target);

            if(_CurrentTargetInfo.IsVisibleFrom(pointOfVision, _TargetRange, false)){
                if(target != null)_TargetsInVision.Add(target);
            }  
            
        } if(debug) Debug.Log(_TargetsInVision.Count + " targets in vision"); 
        
        return _TargetsInVision;
    }

    /// <summary> Finds all Gameobjects tagged with any of the target tags </summary>
    /// <returns> a list of all gameobjects found</returns>
    public List<GameObject> FindAllTargets() {

        List<GameObject> allTargets = new List<GameObject>();
        
        foreach (string tag in _TargetTags){
            allTargets.AddRange(GameObject.FindGameObjectsWithTag(tag));    
        }

        return allTargets;
    }

    /// <summary>xxx
    /// 
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public List<GameObject> FindAllTargetsInRange(float range) {

        List<GameObject> targetsInRange = new List<GameObject>();

        foreach (GameObject target in _AllTargets){
            if(target != null){
                if(Vector3.Distance(transform.position, target.transform.position) < range)targetsInRange.Add(target);
            }
        }

        return targetsInRange;
    }

    /// <summary> xxx </summary>
    /// <returns> xxx</returns>
    public GameObject FindClosestTarget(){

        List<GameObject> targetsInRange = FindAllTargetsInRange(_TargetRange);

        GameObject closestTarget = null;

        float minTargetDist = float.MaxValue;
        float currentTargetDist;

        foreach (GameObject target in targetsInRange){
            
            if(null != target){
                currentTargetDist = Vector3.Distance(transform.position, target.transform.position);

                if (currentTargetDist < minTargetDist){
                    closestTarget = target;
                    minTargetDist = currentTargetDist;
                }
            }

        }

        if(null != closestTarget) return closestTarget;
        else return null;
    }
    
    #endregion

    #region Debug Functions
    //* Debug functions */

    /// <summary> Showing information on the closest enemy
    ///     <para/>Debug Ray:       Blue = No Target, draw to player | Red = has target, draw to target center
    ///     <para/>TargetSwitches:  Logs old and new targets name, their collider height and the current distance to them
    ///     <para/>TargetDist:      Logs the current distance to the closest target whenever it changes
    /// </summary>
    /// <param name="drawLineFromCamera"> Draw a debug Line/Ray from the main camera to the target? </param>
    /// <param name="useRayInstead"> If true, Draws an infinite ray in target direction insead </param>
    /// <param name="logTargetSwitches"> Log target switches to console? </param>
    /// <param name="logTargetDist"> Continiously log distance to target to console? </param>
    public void IndicateClosestTargetInfo(bool drawLineFromCamera = false, bool useRayInstead = false, bool logTargetSwitches = false, bool logTargetDist = false){

        Vector3 _CamPos = Camera.main.transform.position;

        if(FindClosestTarget()){

            
            _CurrentTargetInfo = new TargetInfo(FindClosestTarget());

            if(null == _CurrentTargetInfo) return;

            //Log switch to new closest target
            if(_TargetContainer != _CurrentTargetInfo.t_Object && logTargetSwitches){
                if(_TargetContainer != null){
                    Debug.Log(  "Switching from: " + _TargetContainer.name + " to : " + _CurrentTargetInfo.t_Name + "\n" + 
                                "With Target height: " + _CurrentTargetInfo.t_CollMaxHeight + "\n" +
                                "At basic distance of: " + _CurrentTargetInfo.GetDistanceToObject(transform));
                }
                _TargetContainer = _CurrentTargetInfo.t_Object;
            }

            //Log closest Target Distance if changed
            if(_CurrentTargetInfo.GetDistanceToObject(transform) != _CurrentTargetDistance && logTargetDist){
                Debug.Log("Distance to " + _CurrentTargetInfo.t_Name + ": " + _CurrentTargetInfo.GetDistanceToObject(transform));
                _CurrentTargetDistance = _CurrentTargetInfo.GetDistanceToObject(transform);
            }

            //Draw Debug Ray to target
            if(drawLineFromCamera){
                
                if(useRayInstead)   Debug.DrawRay(_CamPos, ( _CurrentTargetInfo.GetTargetCheckPos() - _CamPos).normalized * _TargetRange, Color.red);
                else                Debug.DrawLine(_CamPos,  _CurrentTargetInfo.GetTargetCheckPos(), Color.red);
            }

        } else if(drawLineFromCamera){
            if(useRayInstead)   Debug.DrawRay(_CamPos, (transform.position - _CamPos).normalized * _TargetRange, Color.blue);
            else                Debug.DrawLine(_CamPos, transform.position, Color.blue);
        }
    }

    /// <summary> xx</summary>
    private void OnDrawGizmos() {
        if(rotateToTarget && debug) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.GetComponent<Collider>().bounds.center, transform.GetComponent<Collider>().bounds.size);
        }
    }
    #endregion

}

#region Helper classes
/// <summary>
/// 
/// </summary>
public class TargetInfo{
    #region public attributes
    public GameObject t_Object;
    public Transform t_transform;
    public LayerMask t_Layer;
    public Bounds t_Bounds;
    public Vector3 t_CollCenter;
    public bool t_HasCollider = false;
    public string t_Name;
    public float t_CollMidHeight;
    public float t_CollMaxHeight;
    #endregion
    
    /// <summary> Constructor
    /// 
    /// </summary>
    /// <param name="_Target"></param>
    public TargetInfo(GameObject _Target){

        if(null == _Target) return; 
        else {
            t_Object = _Target;
            t_Name = _Target.name;
            t_transform = _Target.transform;
            t_Layer = _Target.layer;

            if(_Target.GetComponent<Collider>()){
                t_HasCollider = true;
                t_Bounds = _Target.GetComponent<Collider>().bounds;
                t_CollCenter = t_Bounds.center;
                t_CollMidHeight = t_Bounds.center.y;
                t_CollMaxHeight = t_Bounds.max.y;
            }
        }
    }
    
    #region  public Class functionality
    /// <summary> Calculates distance between object and target</summary>
    /// <param name="from">Transfrom to check the distance from</param>
    /// <returns>Distance between from transforom and target</returns>
    public float GetDistanceToObject(Transform from){

        float DistToTarget = Vector3.Distance(from.position, t_transform.position);

        return DistToTarget;
    }

    /// <summary> Takes the current position of the target 
    /// and adds a desired percentual amout of it's colliders height to the y axis
    /// </summary>
    /// <param name="heightPercentagePoint"> 0 - 1 | percentage of modelheight | .5f by default</param>
    /// <returns>target position with modified y axis</returns>
    public Vector3 GetTargetCheckPos(float heightPercentagePoint = .5f){
        
        float heightAdjustment = 0;

        if(t_Bounds != null) heightAdjustment += t_CollMaxHeight * heightPercentagePoint;
        else heightAdjustment += t_transform.lossyScale.y * heightPercentagePoint;
        
        Vector3 _CurrentCheckPos =  t_transform.position + (Vector3.up * heightAdjustment);

        return _CurrentCheckPos;
    }

    /// <summary> Checks whether the target is visible
    /// 
    /// </summary>
    /// <param name="visibleFrom"></param>
    /// <param name="inRangeOf"></param>
    /// <param name="debug"></param>
    /// <returns></returns>
    public bool IsVisibleFrom(Vector3 visibleFromPos, float inRangeOf, bool debug = false){

        RaycastHit hit;
        Vector3 _CurrentTargetDirection = (t_transform.position - visibleFromPos).normalized;
        Ray _OriginToCurrentTargetRay   = new Ray(visibleFromPos, _CurrentTargetDirection);

        if(debug) Debug.DrawRay(visibleFromPos, _CurrentTargetDirection * inRangeOf, Color.red);

        if(Physics.Raycast(_OriginToCurrentTargetRay, out hit, inRangeOf)){

            if(debug && hit.transform != null)Debug.Log("Vision blocked by: " + hit.transform.gameObject.name);

            if (hit.transform == t_transform) return true;
            else return false;
        } else return false;
    }

    #endregion
}

public enum TargetSetting{
    Closest, next, previous
}
#endregion