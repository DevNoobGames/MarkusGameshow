using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallmarkusSpawnManager : MonoBehaviour
{
    public Transform spawnPoint;
    public int spawned;
    public goldenBoxManager gbm;
    public GameObject[] markusSpawnTarget;
    public List<GameObject> markusObjects;

    public AudioSource backgroundAudio;
    public AudioSource alarmAudio;

    void Start()
    {
        markusSpawnTarget = GameObject.FindGameObjectsWithTag("smallmarkustarget");
        //spawnAMarkus();
    }

    IEnumerator spawnAMark(int AmountToSpawn)
    {
        GameObject m = Instantiate(Resources.Load("Enemies/smallMarkus"), spawnPoint.position, Quaternion.identity) as GameObject;
        m.GetComponent<smallMarkus>().spawnM = this;
        markusObjects.Add(m);
        yield return new WaitForSeconds(0.5f);
        spawned += 1;
        if (spawned < AmountToSpawn)
        {
            StartCoroutine(spawnAMark(AmountToSpawn));
        }
        else
        {
            yield return new WaitForSeconds(2);
            foreach (GameObject mark in markusSpawnTarget)
            {
                mark.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

    }

    public void MarkusCounter()
    {
        if (markusObjects.Count == 0)
        {
            foreach (movingLights ml in gbm.movingL)
            {
                ml.theLight.color = Color.white;
            }
            StartCoroutine(waitASec());
            gbm.OpenTzeSaws(false);
            gbm.OpenTzeFireParticles(false);
            gbm.OpenTzeMovingFire(false);
            backgroundAudio.Stop();
            gbm.normalBGMusic.Play();
            gbm.soundCredits("Now Playing: One Man Symphony - Mohnfeld (Part 1)");
            gbm.closeInstructionText();
            gbm.GoldenBoxCounter();
        }
    }
    IEnumerator waitASec()
    {
        yield return new WaitForSeconds(1);
        gbm.canOpen = true;
    }


    IEnumerator spawnMarkSeq()
    {
        alarmAudio.Play();
        foreach (GameObject mark in markusSpawnTarget)
        {
            mark.transform.GetChild(0).gameObject.SetActive(true);
        }
        foreach (movingLights ml in gbm.movingL)
        {
            ml.theLight.color = Color.red;
        }

        yield return new WaitForSeconds(5);
        gbm.OpenTzeSaws(true);
        gbm.OpenTzeFireParticles(true);
        gbm.OpenTzeMovingFire(true);

        yield return new WaitForSeconds(3);
        backgroundAudio.Play();

        gbm.soundCredits("Now Playing: One Man Symphony - Devil Trigger");
        gbm.openInstructionText("Just kill every small Markus! Be careful, they explode!");

        spawned = 0;
        StartCoroutine(spawnAMark(20));
    }

    public void spawnAMarkus()
    {
        StartCoroutine(spawnMarkSeq());
    }
}
