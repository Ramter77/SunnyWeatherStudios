using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{   
    [SerializeField]
    private float speed = 10;

    [SerializeField]
    private float destroyTime = 5;

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
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
