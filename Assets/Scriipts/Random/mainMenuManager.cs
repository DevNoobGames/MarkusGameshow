using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuManager : MonoBehaviour
{
    public goldenBoxManager gbm;
    public GameObject[] objectsToActive;
    public GameObject[] objectsToDisable;
    public AudioSource gameBGSong;

    public void startTheGame()
    {
        foreach (GameObject act in objectsToActive)
        {
            act.SetActive(true);
        }
        foreach (GameObject dis in objectsToDisable)
        {
            dis.SetActive(false);
        }
        gameBGSong.Play();
        gbm.soundCredits("Now Playing: One Man Symphony - Mohnfeld (Part 1)");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
