using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : Singleton<ExplosionManager>
{
    [Header("Explosion/Effects Settings")]
    [Tooltip("Effect that will be displayed when Enemies hit something")]
    public GameObject enemyHitExplosion;

    [Tooltip("Effect that will be displayed when a fireball collides with something")]
    public GameObject fireballExplosion;

    [Tooltip("Effect that will be displayed when a iceball collides with something")]
    public GameObject iceballExplosion;

    [Tooltip("Effect for blast projectiles")]
    public GameObject blastballExplosion;

    public GameObject defaultTowerExplosion;

    // Start is called before the first frame update
    void Start()
    {
        PoolManager.instance.CreatePool(enemyHitExplosion, 10);
        PoolManager.instance.CreatePool(fireballExplosion, 10);
        PoolManager.instance.CreatePool(iceballExplosion, 10);
        PoolManager.instance.CreatePool(blastballExplosion, 10);
        PoolManager.instance.CreatePool(defaultTowerExplosion, 10);
    }

    public void displayEffect(int explosionIndex, Vector3 explosionPos, Quaternion rotation)
    {
        if (explosionIndex == 0)
            PoolManager.instance.ReuseObject(enemyHitExplosion, explosionPos, rotation);
        else if(explosionIndex == 1)
            PoolManager.instance.ReuseObject(fireballExplosion, explosionPos, rotation);
        else if (explosionIndex == 2)
            PoolManager.instance.ReuseObject(iceballExplosion, explosionPos, rotation);
        else if (explosionIndex == 3)
            PoolManager.instance.ReuseObject(blastballExplosion, explosionPos, Quaternion.Euler(90,0,0));
        else if (explosionIndex == 4)
            PoolManager.instance.ReuseObject(defaultTowerExplosion, explosionPos, Quaternion.Euler(-90,0,0));
    }
}
