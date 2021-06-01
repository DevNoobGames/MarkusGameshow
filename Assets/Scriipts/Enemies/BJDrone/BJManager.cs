using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BJManager : MonoBehaviour
{
    public int spawned;
    public goldenBoxManager gbm;
    public List<GameObject> droneObjects;

    public AudioSource backgroundAudio;
    public AudioSource alarmAudio;

    public void SpawnStart()
    {
        StartCoroutine(spawnBJSeq());
    }

    IEnumerator spawnBJSeq()
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

        gbm.soundCredits("Now Playing: ISAo - Toys Masters | airyluvs.com");
        gbm.openInstructionText("Hit a drone in the eye twice and it will fall down. Then jump on it quickly before it rises back up!");

        yield return new WaitForSeconds(3);
        backgroundAudio.Play();

        spawned = 0;
        StartCoroutine(spawnABJ(5));
    }

    IEnumerator spawnABJ(int AmountToSpawn)
    {
        GameObject BJD = Instantiate(Resources.Load("Enemies/Droneparent"), new Vector3(0,50,0), Quaternion.identity) as GameObject;
        BJD.GetComponent<BJScript>().manager = this;
        droneObjects.Add(BJD);
        yield return new WaitForSeconds(0.3f);
        spawned += 1;
        if (spawned < AmountToSpawn)
        {
            StartCoroutine(spawnABJ(AmountToSpawn));
        }
        else
        {
            //nada
        }
    }

    public void BJCounter()
    {
        if (droneObjects.Count == 0)
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
    }
    IEnumerator waitASec()
    {
        yield return new WaitForSeconds(1);
        gbm.canOpen = true;
    }
}
