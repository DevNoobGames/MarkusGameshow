using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaniManager : MonoBehaviour
{
    public GameObject bigMarkus;
    public GameObject Dani;

    public CameraShake camShake;
    public goldenBoxManager gbm;

    public AudioSource backgroundAudio;
    public AudioSource alarmAudio;
    public AudioSource earthQuakeAudio;



    public void startSeq()
    {
        StartCoroutine(DaniStarter());
    }
    IEnumerator DaniStarter()
    {
        Dani.SetActive(true);

        Dani.GetComponent<DaniMainScr>().health = 100;



        foreach (movingLights ml in gbm.movingL)
        {
            ml.theLight.color = Color.red;
            ml.action = "Center";
        }
        alarmAudio.Play();
        earthQuakeAudio.Play();
        camShake.shakeDuration = 6;

        Animation markAnim = bigMarkus.GetComponent<Animation>();
        markAnim["markusMovingDown"].time = 0;
        markAnim["markusMovingDown"].speed = 1;
        markAnim.Play("markusMovingDown");

        yield return new WaitForSeconds(3);

        Animation animation = Dani.GetComponent<Animation>();
        animation["daniComesUp"].time = 0;
        animation["daniComesUp"].speed = 1;
        animation.Play("daniComesUp");

        yield return new WaitForSeconds(2);
        gbm.OpenTzeSaws(true);
        gbm.OpenTzeFireParticles(true);
        gbm.OpenTzeMovingFire(true);
        gbm.soundCredits("Now Playing: Alexandr Zhelanov - Corp On Road");
        gbm.openInstructionText("Shoot his golden ball!");

        yield return new WaitForSeconds(1);
        earthQuakeAudio.Stop();
        alarmAudio.Stop();
        backgroundAudio.Play();
        yield return new WaitForSeconds(1);

        Dani.GetComponent<DaniMainScr>().startAttacking();
    }

    public void stowTheD()
    {
        StartCoroutine(daniStower());
    }
    IEnumerator daniStower()
    {
        backgroundAudio.Stop();
        earthQuakeAudio.Play();
        camShake.shakeDuration = 5;

        gbm.OpenTzeSaws(false);
        gbm.OpenTzeFireParticles(false);
        gbm.OpenTzeMovingFire(false);
        gbm.closeInstructionText();

        Animation animation = Dani.GetComponent<Animation>();
        animation["daniComesUp"].time = animation["daniComesUp"].length;
        animation["daniComesUp"].speed = -1;
        animation.Play("daniComesUp");
        yield return new WaitForSeconds(3);

        Dani.SetActive(false);

        Animation markAnim = bigMarkus.GetComponent<Animation>();
        markAnim["markusMovingDown"].time = markAnim["markusMovingDown"].length;
        markAnim["markusMovingDown"].speed = -1;
        markAnim.Play("markusMovingDown");

        yield return new WaitForSeconds(2);

        foreach (movingLights ml in gbm.movingL)
        {
            ml.theLight.color = Color.white;
            ml.action = "Rotating";
        }
        earthQuakeAudio.Stop();
        gbm.normalBGMusic.Play();
        gbm.soundCredits("Now Playing: One Man Symphony - Mohnfeld (Part 1)");
        gbm.canOpen = true;
        gbm.GoldenBoxCounter();
    }
}
