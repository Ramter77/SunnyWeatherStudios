using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private float playersEnableDelay = 2f;
    public GameObject _Player1;
    public GameObject _Player2;

    public GameObject MenuCam;
    public GameObject MainMenuHolder;
    public GameObject InGameUiHolder;

    public GameObject fadeOutImage;
    [Tooltip("This Image should be in the MainUI Canvas and should overlay all other UI Elements")]private Animator fadeImageAnim;
    public bool gameStarted = false;

    void Start()
    {
        fadeImageAnim = fadeOutImage.GetComponent<Animator>();

        InputManager.Instance.LockMouse(false);
    }

    private void Update() {
        if (Input.GetButtonDown("Submit")) {
            startGame();
        }
    }
    public void startGame()
    {
        if (gameStarted == false)
        {
            fadeOutImage.SetActive(true);
            fadeImageAnim.SetTrigger("FadeOut");
            gameStarted = true;
            
            //GameManager.Instance.LockMouse(true);
            //LockCursor();

            InputManager.Instance.LockMouse(true);


            _Player1.GetComponent<PlayerController>().MainCameraTransform.parent.transform.parent.transform.parent.gameObject.GetComponent<FreeCameraLook>().StartingCameraAngle();
            
            _Player2.GetComponent<PlayerController>().MainCameraTransform.parent.transform.parent.transform.parent.gameObject.GetComponent<FreeCameraLook>().StartingCameraAngle();
            

            _Player1.GetComponent<PlayerClassOne>().startCooldown();
            _Player1.GetComponent<HealAbility>().startCooldown();
            _Player1.GetComponent<RangedAttack>().startCooldown();
            //_Player1.GetComponent<PlayerController>().MainCameraTransform.parent.transform.parent.transform.parent.gameObject.GetComponent<FreeCameraLook>().StartingCameraAngle();

            _Player2.GetComponent<PlayerClassOne>().startCooldown();
            _Player2.GetComponent<HealAbility>().startCooldown();
            _Player2.GetComponent<RangedAttack>().startCooldown();
            //_Player2.GetComponent<PlayerController>().MainCameraTransform.parent.transform.parent.transform.parent.gameObject.GetComponent<FreeCameraLook>().StartingCameraAngle();

            Time.timeScale = 1f;
            
            
        }
    }

    public void StartingSlowMotion() {
        Time.timeScale = 0.3f;
        StartCoroutine(DisableStartSlowMotion());
    }

    IEnumerator DisableStartSlowMotion() {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 1f;
    }

    public void resumeGame()
    {
        if (gameStarted == false)
        {
            gameStarted = true;
            Time.timeScale = 1f;
            InputManager.Instance.LockMouse(true);
            InGameSpawn();
        }
    }

    public void EnablePlayers() {
        _Player1.GetComponent<PlayerController>().SetAlive();
        _Player1.SetActive(true);

        //_Player1.GetComponent<PlayerController>().MainCameraTransform.parent.transform.parent.transform.parent.gameObject.GetComponent<FreeCameraLook>().StartingCameraAngle();


        _Player2.GetComponent<PlayerController>().SetAlive();
        _Player2.SetActive(true);

        //_Player2.GetComponent<PlayerController>().MainCameraTransform.parent.transform.parent.transform.parent.gameObject.GetComponent<FreeCameraLook>().StartingCameraAngle();
    }

    public void InGameSpawn()
    {
        

        MenuCam.SetActive(false);
        MainMenuHolder.SetActive(false);
        InGameUiHolder.SetActive(true);

        EnablePlayers();
        AudioManager.Instance.PlayMusic(AudioManager.Instance.ingameMusicAudioClip);
        EnemySpawnCycle.Instance.disableAnnoucement();
        EnemySpawnCycle.Instance.startNewWave();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /* public void LockCursor()
    {
        InputManager.Instance._MouseControl.LockMouse = true;
        InputManager.Instance._MouseControl.hideCursor = true;
    } */
}
