using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeImage : MonoBehaviour
{
    public GameObject MenuCanvas;

    private Animator anim;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void prepareGameToPlay()
    {
        if(MenuCanvas != null)
        {
            MenuCanvas.GetComponent<MainMenu>().InGameSpawn();
            //gameObject.SetActive(false);
            
            anim.SetTrigger("FastFadeIn");

            StartCoroutine(DisableFadeImage());
        }
    }

    IEnumerator DisableFadeImage() {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    public void TurnPlayersAtStart() {
        MenuCanvas.GetComponent<MainMenu>()._Player1.transform.eulerAngles = new Vector3(0, -240, 0);
        MenuCanvas.GetComponent<MainMenu>()._Player2.transform.eulerAngles = new Vector3(0, -71, 0);

        MenuCanvas.GetComponent<MainMenu>().StartingSlowMotion();

        MenuCanvas.GetComponent<MainMenu>()._Player1.GetComponent<PlayerController>().MainCameraTransform.parent.transform.parent.transform.parent.gameObject.GetComponent<FreeCameraLook>().StartingCameraAngle();
            
        MenuCanvas.GetComponent<MainMenu>()._Player2.GetComponent<PlayerController>().MainCameraTransform.parent.transform.parent.transform.parent.gameObject.GetComponent<FreeCameraLook>().StartingCameraAngle();
    }
}
