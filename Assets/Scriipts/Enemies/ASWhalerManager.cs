using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASWhalerManager : MonoBehaviour
{
    public GameObject bigMarkus;
    public GameObject ASWhaler;

    public CameraShake camShake;
    public goldenBoxManager gbm;

    public AudioSource backgroundAudio;
    public AudioSource alarmAudio;
    public AudioSource earthQuakeAudio;


    public void startAttack()
    {
        StartCoroutine(ASWhaleStarter());
    }

    public void startStowing()
    {
        StartCoroutine(ASWhalerStower());
    }
    IEnumerator ASWhalerStower()
    {
        backgroundAudio.Stop();
        earthQuakeAudio.Play();
        gbm.closeInstructionText();

        gbm.OpenTzeSaws(false);
        gbm.OpenTzeFireParticles(false);
        gbm.OpenTzeMovingFire(false);

        Animation animation = ASWhaler.GetComponent<Animation>();
        animation["asWhalerMovingUp"].time = animation["asWhalerMovingUp"].length;
        animation["asWhalerMovingUp"].speed = -1;
        animation.Play("asWhalerMovingUp");
        yield return new WaitForSeconds(5);

        ASWhaler.SetActive(false);

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

    IEnumerator ASWhaleStarter()
    {
        ASWhaler.SetActive(true);

        ASWhaler.GetComponent<ASWhaler>().eyeLhealth = 100;
        ASWhaler.GetComponent<ASWhaler>().eyeRhealth = 100;
        ASWhaler.GetComponent<ASWhaler>().eyeL.GetComponent<MeshRenderer>().material = ASWhaler.GetComponent<ASWhaler>().blackMat;
        ASWhaler.GetComponent<ASWhaler>().eyeR.GetComponent<MeshRenderer>().material = ASWhaler.GetComponent<ASWhaler>().blackMat;


        foreach (movingLights ml in gbm.movingL)
        {
            ml.theLight.color = Color.red;
            ml.action = "Center";
        }
        alarmAudio.Play();
        earthQuakeAudio.Play();
        camShake.shakeDuration = 9;

        Animation markAnim = bigMarkus.GetComponent<Animation>();
        markAnim["markusMovingDown"].time = 0;
        markAnim["markusMovingDown"].speed = 1;
        markAnim.Play("markusMovingDown");

        yield return new WaitForSeconds(3);

        Animation animation = ASWhaler.GetComponent<Animation>();
        animation["asWhalerMovingUp"].time = 0;
        animation["asWhalerMovingUp"].speed = 1;
        animation.Play("asWhalerMovingUp");

        yield return new WaitForSeconds(2);
        gbm.OpenTzeSaws(true);
        gbm.OpenTzeFireParticles(true);
        gbm.OpenTzeMovingFire(true);

        yield return new WaitForSeconds(3);
        earthQuakeAudio.Stop();
        alarmAudio.Stop();
        backgroundAudio.Play();
        gbm.soundCredits("Now Playing: ISAo - Toys Masters | airyluvs.com");
        gbm.openInstructionText("Hit him in the eyes when he stops shooting!");
        yield return new WaitForSeconds(1);
        ASWhaler.GetComponent<ASWhaler>().loaded = true;
    }
}
