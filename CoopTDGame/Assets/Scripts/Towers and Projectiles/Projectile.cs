using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{   
    [SerializeField]
    private float speed = 10;

    [SerializeField]
    private float destroyTime = 5;
    [SerializeField]
    private bool destroyOnContact = false;
    [SerializeField]
    private bool unparentChild = false;
    [SerializeField]
    private bool destroyChild = false;
    [SerializeField]
    private float childDestroyTime = 1;
    

    private Light lightSource;

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }

    void Start()
    {
        //!NOT NEEDED AFTER ALL????? First disable light just before destroying object to stop TLA _DEBUG_STACK_LEAK
        //lightSource = GetComponent<Light>();
        //StartCoroutine(DisableLight(destroyTime));

        //Then destroy in destroyTime amout of seconds
        Destroy(gameObject, destroyTime);
    }

    IEnumerator DisableLight(float delay) {
        yield return new WaitForSeconds(delay);
        //lightSource.enabled = false;
    }

    void Update()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (destroyOnContact) {
            if (unparentChild) {
                if (transform.GetChild(0) != null) {
                    if (destroyChild) {
                        Destroy(transform.GetChild(0).gameObject, childDestroyTime);
                    }
                    transform.GetChild(0).transform.parent = null;
                }
            }
            Destroy(gameObject);
        }
    }
}
