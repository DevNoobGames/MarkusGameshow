using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    public Transform spawnPoint;
    public int spawned;
    public goldenBoxManager gbm;
    public GameObject[] markusSpawnTarget;
    public List<GameObject> markusObjects;

    void Start()
    {
        markusSpawnTarget = GameObject.FindGameObjectsWithTag("smallmarkustarget");
        spawnAMarkus();
    }

    IEnumerator spawnAMark(int AmountToSpawn)
    {
        GameObject m = Instantiate(Resources.Load("Enemies/smallMarkus"), spawnPoint.position, Quaternion.identity) as GameObject;
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

    void spawnAMarkus()
    {
        foreach (GameObject mark in markusSpawnTarget)
        {
            mark.transform.GetChild(0).gameObject.SetActive(true);
        }
        foreach (movingLights ml in gbm.movingL)
        {
            ml.theLight.color = Color.red;
        }

        spawned = 0;
        StartCoroutine(spawnAMark(10));
    }
}
