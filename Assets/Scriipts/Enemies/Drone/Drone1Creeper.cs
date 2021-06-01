using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone1Creeper : MonoBehaviour
{
    public GameObject Player;
    bool loaded;
    public Transform spawnPos;
    public int ammo;

    public float health = 100;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("FPSController");
        loaded = true;
        ammo = 0;
        StartCoroutine(newAmmo());
    }

    void Update()
    {
        Vector3 targetPostition = new Vector3(Player.transform.position.x,
                                this.transform.position.y,
                                Player.transform.position.z);
        this.transform.LookAt(targetPostition);

        if (loaded && ammo > 0)
        {
            loaded = false;
            ammo -= 1;
            GetComponent<AudioSource>().Play();
            GameObject pcb = Instantiate(Resources.Load("Enemies/pancakeBullet"), spawnPos.position, Quaternion.identity) as GameObject;
            pcb.transform.LookAt(Player.transform);
            Rigidbody rb = pcb.GetComponent<Rigidbody>();
            rb.velocity = (Player.transform.position - pcb.transform.position).normalized * 60;
            StartCoroutine(reload());
            if (ammo <= 0)
            {
                StartCoroutine(newAmmo());
            }
        }
    }

    public void gotHit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GetComponentInParent<DroneManager>().droneDestroyed(gameObject);
        }
    }

    IEnumerator newAmmo()
    {
        yield return new WaitForSeconds(4);
        ammo = 30;
    }

    IEnumerator reload()
    {
        yield return new WaitForSeconds(0.3f);
        loaded = true;
    }
}
