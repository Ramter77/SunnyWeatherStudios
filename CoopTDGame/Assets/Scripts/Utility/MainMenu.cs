using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject _Player1;
    public GameObject _Player2;
    public GameObject _Player1Duplica;
    public GameObject _Player2Duplica;

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
        if(Input.GetButtonDown("Submit")) {
            startGame();
        }
    }
    public void startGame()
    {
        if(gameStarted == false)
        {
            fadeImageAnim.SetTrigger("FadeOut");
            gameStarted = true;
            LockCursor();
        }
    }

    public void InGameSpawn()
    {
        MenuCam.SetActive(false);
        MainMenuHolder.SetActive(false);
        _Player1Duplica.SetActive(false);
        _Player2Duplica.SetActive(false);
        _Player1.SetActive(true);
        _Player2.SetActive(true);
        InGameUiHolder.SetActive(true);
        EnemySpawnCycle.Instance.startNewWave();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
