using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private float playersEnableDelay = 2f;
    public GameObject _Player1;
    public GameObject _Player2;

    public GameObject MenuCam;
    public GameObject MainMenuHolder;
    public GameObject InGameUiHolder;

    public Image fadeOutImage;
    [Tooltip("This Image should be in the MainUI Canvas and should overlay all other UI Elements")]private Animator fadeImageAnim;
    private bool gameStarted = false;

    void Start()
    {
        fadeImageAnim = fadeOutImage.GetComponent<Animator>();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
            fadeImageAnim.SetTrigger("FadeOut");
            gameStarted = true;
            Time.timeScale = 1f;
            LockCursor();
        }
    }

    public void EnablePlayers() {
        _Player1.GetComponent<PlayerController>().SetAlive();
        _Player2.GetComponent<PlayerController>().SetAlive();
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

    public void LockCursor()
    {
        InputManager.Instance._MouseControl.LockMouse = true;
        InputManager.Instance._MouseControl.hideCursor = true;
    }
}
