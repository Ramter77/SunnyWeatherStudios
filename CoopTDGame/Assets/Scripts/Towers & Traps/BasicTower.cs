﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MultiAudioListener;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class BasicTower : MonoBehaviour
{
    [Header("Object References")]
    public GameObject closestEnemy = null;
    [SerializeField] private GameObject[] gos; // game object array for enemies
    public GameObject shooter; // the tower - used for intercept calculation
    private GameObject target; // the target he picked from the enemy array - used for intercept calculation
    public GameObject[] bulletPrefabs;
    public GameObject bulletPrefab; // prefab he shoots
    private Projectile bulletPrefabProjectileScript;
    public Transform shootingPoint;
    public Transform centerAttackRadius; // center for attack range calculation

    [Header("Attack Settings")]
    [Tooltip("speed of the projectile")] public float shotSpeed = 20f;
    [Tooltip("Attack Speed of the tower")] public float attackSpeed; // the lower the value the faster the turret can shoot
    [Tooltip("Attack Range of th tower")]  public float attackRange = 20f; // the range of the tower
    [Tooltip("Minimum Attack Range of Tower")] public float minAttackRange = 5f;
    private int turretLayerIgnore = ~11; // ignore this layer (the layer of tower)

    //locations
    [Header("Positions")]
    [Tooltip("the calculated point the tower will shoot at")] public Vector3 interceptPoint; 
    private Vector3 shooterPosition; // tower position
    private Vector3 targetPosition; // target position
    //velocities
    private Vector3 shooterVelocity; // tower velocity
    [SerializeField] private Vector3 targetVelocity; // target velocity, since we are working with nav mesh, you need to access the agent velocity, not rigidbody

    public bool activated = false;


    private Component[] ownColliders;
    private MeshCollider crystalCollider;

    private MultiAudioSource audioSource;
    private AudioClip _audioClip;

    // Start is called before the first frame update
    void Start()
    {
        changeProjectile(0);
        /* bulletPrefab = bulletPrefabs[0];
        if (AudioManager.Instance.towerProjectiles[0] != null) {
            _audioClip = AudioManager.Instance.towerProjectiles[0];
        } */
        audioSource = GetComponent<MultiAudioSource>();

        //Get own colliders
        ownColliders = GetComponentsInChildren<Collider>();
        if (shooter.transform.childCount > 0) {
            crystalCollider = shooter.transform.GetChild(0).GetComponent<MeshCollider>();
        }


        FindClosestTarget();
        target = closestEnemy;
        attackSpeed = 2;
        shooterPosition = shooter.transform.position;
        targetPosition = target.transform.position;
        shooterVelocity = shooter.GetComponent<Rigidbody>() ? shooter.GetComponent<Rigidbody>().velocity : Vector3.zero;
        targetVelocity = target.GetComponent<Rigidbody>() ? target.GetComponent<Rigidbody>().velocity : Vector3.zero;
        StartCoroutine(shootCd());
    }

    #region Aiming and Shooting
    /// <summary>
    /// Calls for the intercept point, then checks if that point is in range, if in range checks for any objects blocking its path
    /// If there is no object blocking the path of the bullet, the tower shoots.
    /// </summary>

    private void aimAtTarget()
    {
        if(activated)
        {
            FindClosestTarget();
            target = closestEnemy;
            if (target != null)
            {
                shooterPosition = shooter.transform.position;
                targetPosition = target.transform.position;
                shooterVelocity = shooter.GetComponent<Rigidbody>() ? shooter.GetComponent<Rigidbody>().velocity : Vector3.zero;
                targetVelocity = target.GetComponent<BasicEnemy>().agent.velocity;
                interceptPoint = FirstOrderIntercept
                (
                    shooterPosition,
                    shooterVelocity,
                    shotSpeed,
                    targetPosition,
                    targetVelocity
                );
                Vector3 spawnPoint = shootingPoint.position;
                Vector3 centerOfAttackRadius = centerAttackRadius.position;
                Vector3 targetPoint = interceptPoint;
                Vector3 toTarget = targetPoint - spawnPoint;
                if (Vector3.Distance(centerOfAttackRadius, targetPoint) <= attackRange)
                {
                    RaycastHit hit;
                    float distance = Vector3.Distance(gameObject.transform.position, interceptPoint);
                    Vector3 fwd = interceptPoint - gameObject.transform.position;
                    if (Physics.Raycast(transform.position, fwd, out hit, distance, turretLayerIgnore))
                    {
                        if (hit.collider.tag == "Environment") // list all the tags for objects that should block line of projectile
                        {
                            Debug.DrawLine(transform.position, hit.point);
                            //Debug.Log("Terrain in the way");
                            StartCoroutine(shootCd());
                        }
                        else // when it collides with an object of different tag 
                        {
                            if (bulletPrefab.GetComponent<projectileVelocity>() != null) {
                                bulletPrefab.GetComponent<projectileVelocity>().speed = shotSpeed;
                            }

                            GameObject bullet = Instantiate(bulletPrefab, spawnPoint, Quaternion.LookRotation(toTarget));
                            if (bullet.GetComponent<Rigidbody>() != null) {
                                bullet.GetComponent<Rigidbody>().useGravity = false;
                            }

                            bullet.layer = 16;

                            AudioManager.Instance.PlaySound(audioSource, _audioClip);

                            StartCoroutine(shootCd());
                        }
                    }
                    else // when no collision occurs
                    {
                        if (bulletPrefab.GetComponent<projectileVelocity>() != null) {
                            bulletPrefab.GetComponent<projectileVelocity>().speed = shotSpeed;
                        }

                        GameObject bullet = Instantiate(bulletPrefab, spawnPoint, Quaternion.LookRotation(toTarget));
                        if (bullet.GetComponent<Rigidbody>() != null) {
                            bullet.GetComponent<Rigidbody>().useGravity = false;
                        }
                        
                        bullet.layer = 16;
                        StartCoroutine(shootCd());
                    }
                }
                else
                {
                    //Debug.Log("Turret: Target not in range");
                    StartCoroutine(shootCd());
                }


                //Debug.Log(shooterVelocity);
                //Debug.Log(targetVelocity);
                //Debug.Log(interceptPoint);
            }
            else
            {
                StartCoroutine(shootCd());
            }

        }

    }


    #endregion

    public void startAiming()
    {
        StartCoroutine(shootCd());
    }

    public IEnumerator shootCd()
    {
        yield return new WaitForSeconds(attackSpeed);
        aimAtTarget();
    }


    void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR
        Vector3 centerOfAttackRadius = centerAttackRadius.position;
        Vector3 targetPoint = interceptPoint;
        if (Vector3.Distance(centerOfAttackRadius, targetPoint) <= attackRange)
        {
            Handles.color = new Color(0f, 1f, 0f, 0.2f);
            Handles.DrawSolidDisc(centerAttackRadius.position, Vector3.down, attackRange);
        }
        else
        {
            Handles.color = new Color(1f, 0f, 0f, 0.2f);
            Handles.DrawSolidDisc(centerAttackRadius.position, Vector3.down, attackRange);
        }
        #endif
    }

    public void changeProjectile(int i) {
        bulletPrefab = bulletPrefabs[i];
        if (AudioManager.Instance.towerProjectiles.Length > 0) {
            _audioClip = AudioManager.Instance.towerProjectiles[i];
        }

        /* //Get all sphere collider on the prefab & ignore all collisions
        SphereCollider[] bulletPrefabSphereColliders = bulletPrefab.GetComponents<SphereCollider>();
        IgnoreCollisions(bulletPrefabSphereColliders); */
    }

    /* private void IgnoreCollisions(SphereCollider[] colliders) {
        //loop over own colliders
        foreach (Collider _col in ownColliders) {
            //loop over bulletPrefabSphereColliders
            foreach (Collider _col2 in colliders)
            {
                Physics.IgnoreCollision(_col, _col2);
                Physics.IgnoreCollision(crystalCollider, _col2);
            }
        }
    } */

    /// <summary>
    /// find closest enemy
    /// </summary>
    public void FindClosestTarget()
    {   
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            float diff = Vector3.Distance(centerAttackRadius.position, go.transform.position);
            float curDistance = diff;
            if(curDistance > minAttackRange && curDistance < distance)
            {
                closestEnemy = go;
                distance = curDistance;
            }
        }
    }

    #region Intercept Point calculation

    /// <summary>
    /// This Vector 3 calculates a point in the "future" on which a shot projectile of the tower is able to hit 
    /// the targeted enemy. This point is calculated based on the current enemy and shooter velocity and takes the travel 
    /// time of the bullet into account.
    /// Any changes in velocity on the enemy site can lead into the projectile missing its target
    /// </summary>
    /// <param name="shooterPosition"></param>
    /// <param name="shooterVelocity"></param>
    /// <param name="shotSpeed"></param>
    /// <param name="targetPosition"></param>
    /// <param name="targetVelocity"></param>
    /// <returns></returns>
    public static Vector3 FirstOrderIntercept
        (
            Vector3 shooterPosition,
            Vector3 shooterVelocity,
            float shotSpeed,
            Vector3 targetPosition,
            Vector3 targetVelocity
        )

    {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
        float t = FirstOrderInterceptTime
        (
            shotSpeed,
            targetRelativePosition,
            targetRelativeVelocity
        );
        return targetPosition + t * (targetRelativeVelocity);
    }
    //first-order intercept using relative target position
    public static float FirstOrderInterceptTime
    (
        float shotSpeed,
        Vector3 targetRelativePosition,
        Vector3 targetRelativeVelocity
    )
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f)
            return 0f;

        float a = velocitySquared - shotSpeed * shotSpeed;

        //handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude /
            (
                2f * Vector3.Dot
                (
                    targetRelativeVelocity,
                    targetRelativePosition
                )
            );
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                    t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
    }

#endregion


}