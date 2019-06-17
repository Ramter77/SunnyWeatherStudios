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
            Time.timeScale = 1f;
            //GameManager.Instance.LockMouse(true);
            //LockCursor();

            InputManager.Instance.LockMouse(true);
        }
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
        _Player2.GetComponent<PlayerController>().SetAlive();
        _Player2.SetActive(true);
    }

    public void InGameSpawn()
    {
        MenuCam.SetActive(false);
        MainMenuHolder.SetActive(false);
        InGameUiHolder.SetActive(true);

        EnablePlayers();
        AudioManager.Instance.PlayMusic(AudioManager.Instance.ingameMusicAudioClip);
        EnemySpawnCycle.Instance.annoucementText.text = ("The game starts now! You need to defend the Sphere!");
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
