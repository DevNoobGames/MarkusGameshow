using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class winMenuScript : MonoBehaviour
{
    public devNoobPlayer dnPlayer;
    public TextMeshProUGUI scoreText;

    public void setScoreText(float damage)
    {
        scoreText.text = "You lost a total of " + damage + " health";
    }

    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void quitthegame()
    {
        Application.Quit();
    }
}
