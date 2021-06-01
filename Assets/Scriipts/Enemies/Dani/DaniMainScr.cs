using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaniMainScr : MonoBehaviour
{
    public GameObject Player;
    Animation anim;
    public ParticleSystem particles;
    public float health = 100;
    AudioSource audios;
    public DaniManager danmag;

    private void Start()
    {
        //Player = GameObject.FindGameObjectWithTag("FPSController");
        anim = GetComponent<Animation>();
        audios = GetComponent<AudioSource>();
    }

    public void gotHit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            anim.Stop();
            danmag.stowTheD();
        }
        Debug.Log(health);
    }

    void Update()
    {
        Vector3 targetPostition = new Vector3(Player.transform.position.x,
                                this.transform.position.y,
                                Player.transform.position.z);
        this.transform.LookAt(targetPostition);
    }

    public void startAttacking()
    {
        StartCoroutine(DaniAttack());
    }
    IEnumerator DaniAttack()
    {
        if (health > 0)
        {
            anim.Play();
            yield return new WaitForSeconds(2.75f);
            smash();
            yield return new WaitForSeconds(1);
            smash();
            yield return new WaitForSeconds(1);
            smash();
            yield return new WaitForSeconds(1);
            smash();
            yield return new WaitForSeconds(1);
            smash();
        }
        yield return new WaitForSeconds(4);
        StartCoroutine(DaniAttack());
    }

    public void smash()
    {
        Camera.main.GetComponent<CameraShake>().shakeDuration = 0.3f;
        particles.Play();
        audios.Play();
    }
}
