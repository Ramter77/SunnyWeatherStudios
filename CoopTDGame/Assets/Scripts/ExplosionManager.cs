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

    // Start is called before the first frame update
    void Start()
    {
        PoolManager.instance.CreatePool(enemyHitExplosion, 3);
        PoolManager.instance.CreatePool(fireballExplosion, 3);
        PoolManager.instance.CreatePool(iceballExplosion, 3);
        PoolManager.instance.CreatePool(blastballExplosion, 3);
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
            PoolManager.instance.ReuseObject(blastballExplosion, explosionPos, rotation);
    }
}
