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
        PoolManager.instance.CreatePool(enemyHitExplosion, 2);
        PoolManager.instance.CreatePool(fireballExplosion, 2);
        PoolManager.instance.CreatePool(iceballExplosion, 2);
    }

    public void displayEffect(int explosionIndex, Transform explosionPos, Quaternion rotation)
    {
        if (explosionIndex == 0)
            PoolManager.instance.ReuseObject(enemyHitExplosion, explosionPos.position, rotation);
        else if(explosionIndex == 1)
            PoolManager.instance.ReuseObject(fireballExplosion, explosionPos.position, rotation);
        else if (explosionIndex == 2)
            PoolManager.instance.ReuseObject(iceballExplosion, explosionPos.position, rotation);
        else if (explosionIndex == 3)
            PoolManager.instance.ReuseObject(blastballExplosion, explosionPos.position, rotation);
    }
}
