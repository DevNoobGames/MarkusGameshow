using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class smallMarkus : MonoBehaviour
{
    public GameObject player;
    public Transform target;
    public bool attacking;
    public float speed = 60;
    public GameObject[] targets;

    public float health = 30;
    public smallmarkusSpawnManager spawnM;

    AudioSource selfDestAudio;

    private void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("smallmarkustarget");
        int randval = Random.Range(0, targets.Length);
        target = targets[randval].transform;

        player = GameObject.FindGameObjectWithTag("FPSController");
        selfDestAudio = GetComponent<AudioSource>();

    }

    public void gotHit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(selfDest());
            /*Destroy(gameObject);
            spawnM.markusObjects.Remove(gameObject);
            spawnM.MarkusCounter();*/
        }
        Debug.Log(health);
    }

    IEnumerator selfDest()
    {
        speed = 0;
        selfDestAudio.Play();
        yield return new WaitForSeconds(2);
        Instantiate(Resources.Load("Enemies/ExplosionSystem"), transform.position, transform.rotation);
        spawnM.markusObjects.Remove(gameObject);
        spawnM.MarkusCounter();
        Destroy(gameObject);
    }

    void Update()
    {
        if (!attacking)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            if (Vector3.Distance(transform.position, target.position) < 0.1)
            {
                GetComponent<NavMeshAgent>().Warp(target.position);
                attacking = true;
            }

        }

        if (attacking)
        {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
            if (Vector3.Distance(transform.position, player.transform.position) < 1)
            {
                StartCoroutine(selfDest());
            }
        }
    }
}
