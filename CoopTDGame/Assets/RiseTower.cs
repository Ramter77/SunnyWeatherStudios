using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseTower : MonoBehaviour
{
    private SphereCollider ownCollider;
    private Animation anim;
    private Transform towerChild;
    private bool risen;

    private PlayerController playC;
    private bool _input;

    void Start()
    {
        ownCollider = GetComponent<SphereCollider>();
        anim = GetComponent<Animation>();
        towerChild = transform.GetChild(0).GetChild(0);
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Colliding with " + other.tag);
        if (other.CompareTag("Player") || other.CompareTag("Player2")) {
            playC = other.GetComponent<PlayerController>();

            //* Player 0 input */
            if (playC.Player_ == 0)
            {
                _input = InputManager.Instance.Interact0;
            }

            //* Player 1 input */
            else if (playC.Player_ == 1)
            {
                _input = InputManager.Instance.Interact1;
            }

            //*Player 2 input */
            else if (playC.Player_ == 2) {
                _input = InputManager.Instance.Interact2;
            }

            if (_input) {
                _RiseTower();
            }
        }
    }

    void _RiseTower() {
        if (!risen) {
            risen = true;

            ownCollider.enabled = false;
            anim.Play();
            towerChild.tag = "possibleTargets";
        }
    }
}
