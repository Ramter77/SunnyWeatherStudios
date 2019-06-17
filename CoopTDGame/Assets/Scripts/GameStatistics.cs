using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStatistics : MonoBehaviour
{
    public Text WaveCountText;
    public Text KillCountText;
    public Text RevivesCountText;
    public Text SoulsUsedCountText;
    public Text SoulsPickedUpCountText;




    private void Awake()
    {
        WaveCountText.text = ("" + GameAnalytics.Instance.waveReached);
        KillCountText.text = ("" + GameAnalytics.Instance.enemyDeath);
        int combinedRevives = GameAnalytics.Instance.player2Revive + GameAnalytics.Instance.player2Revive;
        RevivesCountText.text = ("" + combinedRevives);
        SoulsUsedCountText.text = ("" + GameAnalytics.Instance.soulsUsed);
        SoulsPickedUpCountText.text = ("" + GameAnalytics.Instance.soulsPickedUp);
        InputManager.Instance.LockMouse(false);
    }


    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
