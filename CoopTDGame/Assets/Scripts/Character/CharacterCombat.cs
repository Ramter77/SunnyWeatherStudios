using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Utility;

[RequireComponent(typeof(CharacterBehaviour))]
public class CharacterCombat : MonoBehaviour{

    public string locomotionLayerName = "Locomotion";
    public string weaponLayerName = "Weapon";

    
    public float _TargetRange = 50f;

    public GameObject WeaponHolder;

    public Transform head;

    public bool _InFight;
    public bool _Armed;
    public bool _Block;
    public bool _InCombo;
    public bool _ChainAttack;
    public bool _AttackWhileWalking = true;
    


    private Animator _Anim;
    private CharacterStateBehaviour _StateBehaviour;
    private Cinemachine.CinemachineTargetGroup _Target;
    [HideInInspector]
    public AnimatorStateInfo currentState;

    CharacterBehaviour _Char;

    Transform currentTarget;

    int locomotionLayer; 
    int weaponLayer;


    float chainTimeLeft;

    private void Start() {
        _Anim = GetComponent<Animator>();
        _StateBehaviour = _Anim.GetBehaviour<CharacterStateBehaviour>();
        _Target = FindObjectOfType<Cinemachine.CinemachineTargetGroup>();

        //Debug.Log(_Target.tag);

        _Anim.SetBool("Armed", _Armed);

        

        locomotionLayer = _Anim.GetLayerIndex(locomotionLayerName);
        weaponLayer = _Anim.GetLayerIndex(weaponLayerName);
        
    }

    private void Update() {
       

        if(_InFight && currentTarget == null){
            if(FindClosestEnemy(_TargetRange) != null){
                ResetTarget(FindClosestEnemy(_TargetRange).transform);
            } else {
                SwitchStance(false);
            }
        }

        if(currentTarget){
            //Debug.Log(currentTarget.position);

            Vector3 targetPos = new Vector3(currentTarget.position.x, currentTarget.position.y + currentTarget.localScale.y, currentTarget.position.z);

            Debug.DrawLine(head.position, targetPos, Color.red);
        } 
    }
    
    public void Attack(int comboInput){

        if(!_Armed){
            SwitchStance(true);
        } else {
            if(!_InFight)_InFight = true;
            _Anim.SetTrigger("Attack");
        }

        

        //CheckArmed
        
        //_Anim.SetBool("InFight", true);
        //
        //_Anim.SetBool("InCombo", true);
        //_Anim.SetBool("Chain", true);
        //_Anim.SetInteger("ComboInput", comboInput);

        //Debug.Log(_Anim.GetNextAnimatorStateInfo(locomotionLayer).length);

    }

    public void Block(bool blocking){
        _Block = blocking;
        _Anim.SetBool("Block", blocking);
    }

    public void SwitchStance(bool sheath){

        if(_InFight || _Armed){

            if(sheath){
                _Anim.SetTrigger("ToggleWeapon");
                _Armed = false;
            }
            _InFight = false;
            
        } else {

            if(sheath){
                _Anim.SetTrigger("ToggleWeapon");
                _Armed = true;
            }   
            _InFight = true;
            

            if(FindClosestEnemy(_TargetRange) != null){
                ResetTarget(FindClosestEnemy(_TargetRange).transform);
            }
            

        }

        
        

        _Anim.SetBool("Armed", _Armed);
        _Anim.SetBool("InFight", _InFight);

    }

    public void ToggleWeapon(){

        WeaponHolder.SetActive(_Armed);

    }

    void ResetTarget(Transform target){

        currentTarget = target;
        int targetSelf = 0;

        _Target.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[2];

        if(currentTarget != null){
            targetSelf = 1;

            _Target.m_Targets[0].target = target;
            _Target.m_Targets[0].weight = 1;
            _Target.m_Targets[0].radius = 2;
        }

        _Target.m_Targets[targetSelf].target = transform;
        _Target.m_Targets[targetSelf].weight = 1;
        _Target.m_Targets[targetSelf].radius = 2;


    }

    private GameObject FindClosestEnemy(float maxDistance)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        
        GameObject closest = null;

        float distance = Mathf.Infinity;

        Vector3 position = transform.position;

        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        if(closest != null) distance = Vector3.Distance(transform.position, closest.transform.position);
        if((closest != null) && (distance <= maxDistance)) {
            return closest;
        }else return null;
            
        //Debug.Log(Vector3.Distance(transform.position, closest.transform.position) + " to  " + closest.name);
        //Debug.Log(distance);
    }
    
}