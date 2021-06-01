using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDudeManage : MonoBehaviour
{
    public AudioSource alarmAudio;
    public goldenBoxManager gbm;
    public AudioSource backgroundAudio;
    public Transform spawnPoint;

    public void startAttack()
    {
        StartCoroutine(FireAttack());
    }

    IEnumerator FireAttack()
    {
        alarmAudio.Play();

        foreach (movingLights ml in gbm.movingL)
        {
            ml.theLight.color = Color.red;
        }

        yield return new WaitForSeconds(5);
        gbm.OpenTzeSaws(true);
        gbm.OpenTzeFireParticles(true);
        gbm.OpenTzeMovingFire(true);
        gbm.soundCredits("Now Playing: Alexander Ehlers - Doomed");
        gbm.openInstructionText("Get under him and shoot in him. Be careful for the fire!");

        yield return new WaitForSeconds(3);
        backgroundAudio.Play();

        Instantiate(Resources.Load("Enemies/FireDude"), spawnPoint.position, Quaternion.identity);   
    }

    public void backToNormal()
    {
        foreach (movingLights ml in gbm.movingL)
        {
            ml.theLight.color = Color.white;
        }
        //gbm.canOpen = true;
        StartCoroutine(waitASec());
        gbm.OpenTzeSaws(false);
        gbm.OpenTzeFireParticles(false);
        gbm.OpenTzeMovingFire(false);
        gbm.closeInstructionText();
        backgroundAudio.Stop();
        gbm.normalBGMusic.Play();
        gbm.soundCredits("Now Playing: One Man Symphony - Mohnfeld (Part 1)");
        gbm.GoldenBoxCounter();
    }

    IEnumerator waitASec()
    {
        yield return new WaitForSeconds(1);
        gbm.canOpen = true;
    }
}
