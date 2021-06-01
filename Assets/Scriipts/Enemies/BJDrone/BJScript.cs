using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BJScript : MonoBehaviour
{
    GameObject Player;
    public float healthPrimary;
    bool destroyed;
    Rigidbody rb;
    public ParticleSystem particles;

    public AudioSource selfDestAudio;
    public AudioSource beepAudio;

    public float minX = -5;
    public float maxX = 5;
    public float minZ = -5;
    public float maxZ = 5;
    Vector3 target;

    public float speed = 10;

    public BJManager manager;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("FPSController");
        rb = GetComponent<Rigidbody>();

        selfDestAudio = GameObject.FindGameObjectWithTag("selfDest").GetComponent<AudioSource>();
        beepAudio = GameObject.FindGameObjectWithTag("beep").GetComponent<AudioSource>();

        destroyed = false;
        InvokeRepeating("newTarget", 0, 4);
    }

    public void gotHit(float damage)
    {
        healthPrimary -= 50;
        if (healthPrimary <= 0)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            particles.Play();
            StartCoroutine(secondaryHealth());
        }
    }

    public void newTarget()
    {
        if (healthPrimary > 0)
        {
            float x = Random.Range(Player.transform.position.x + minX, Player.transform.position.x + maxX);
            float z = Random.Range(Player.transform.position.z + minZ, Player.transform.position.z + maxZ);
            target = new Vector3(x, 15, z);
        }
    }

    public void dest()
    {
        if (!destroyed)
        {
            StartCoroutine(destroyedSeq());
        }
    }

    IEnumerator destroyedSeq()
    {
        destroyed = true;
        particles.Stop();
        beepAudio.Play();
        yield return new WaitForSeconds(1);
        selfDestAudio.Play();
        yield return new WaitForSeconds(2);
        Instantiate(Resources.Load("Enemies/ExplosionSystem"), transform.position, transform.rotation);
        manager.droneObjects.Remove(gameObject);
        manager.BJCounter();
        Destroy(gameObject);
    }

    IEnumerator secondaryHealth()
    {
        yield return new WaitForSeconds(5);
        if (!destroyed)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = false;
            particles.Stop();
            healthPrimary = 100;   
        }
    }

    void Update()
    {
        if (healthPrimary > 0)
        {
            Vector3 targetPostition = new Vector3(Player.transform.position.x,
                        this.transform.position.y,
                        Player.transform.position.z);
            this.transform.LookAt(targetPostition);

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
    }
}
