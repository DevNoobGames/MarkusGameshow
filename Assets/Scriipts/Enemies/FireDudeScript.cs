using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDudeScript : MonoBehaviour
{
    Animation anim;
    Rigidbody rb;
    public GameObject player;
    Vector3 targetPos;
    public bool JumpingProcess;
    FireDudeManage fdm;

    CameraShake camShake;

    public ParticleSystem particles;

    GameObject exitTarget;
    GameObject deathTarget;

    public float GroundDistance;

    public float health = 100;

    void Start()
    {
        anim = GetComponent<Animation>();
        rb = GetComponent<Rigidbody>();
        exitTarget = GameObject.FindGameObjectWithTag("exitTarget");
        deathTarget = GameObject.FindGameObjectWithTag("deathTarget");
        camShake = Camera.main.GetComponent<CameraShake>();
        player = GameObject.FindGameObjectWithTag("FPSController");
        fdm = GameObject.FindGameObjectWithTag("firedudeman").GetComponent<FireDudeManage>();
        particles.Play();
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GroundDistance + 0.1f);
    }

    public void gotHit(float damage)
    {
        GetComponent<AudioSource>().Play();
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            StartCoroutine(death());
        }
    }

    IEnumerator death()
    {
        particles.Stop();
        fdm.backgroundAudio.Stop();
        camShake.shakeDuration = 8;
        yield return new WaitForSeconds(4);
        particles.Play();
        yield return new WaitForSeconds(1);
        anim.Play();
        yield return new WaitForSeconds(1.33f);
        targetPos = exitTarget.transform.position;
        rb.AddForce(Vector3.up * 500);
    }

    private void Update()
    {
        bool check1 = IsGrounded();
        if (!check1)
        {   
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 30 * Time.deltaTime);
        }

        if (check1 && !JumpingProcess && health > 0)
        {
            camShake.shakeDuration = 0.3f;
            JumpingProcess = true;
            StartCoroutine(Jump());
        }

        if (Vector3.Distance(transform.position, exitTarget.transform.position) < 1 && health <= 0)
        {
            targetPos = new Vector3(exitTarget.transform.position.x, exitTarget.transform.position.y + 40, exitTarget.transform.position.z);
            rb.AddForce(Vector3.up * 5);
            fdm.backToNormal();
            Destroy(gameObject, 4);
        }
        if (Vector3.Distance(transform.position, deathTarget.transform.position) < 1)
        {
            fdm.backToNormal();
            Destroy(gameObject);
        }
    }

    IEnumerator Jump()
    {
        JumpingProcess = true;
        yield return new WaitForSeconds(2);
        particles.Stop();
        yield return new WaitForSeconds(6);
        particles.Play();
        camShake.shakeDuration = 2;
        yield return new WaitForSeconds(1);
        anim.Play();
        yield return new WaitForSeconds(1.33f);
        if (health > 0)
        {
            targetPos = player.transform.position;
            rb.AddForce(Vector3.up * 1000);
        }
        yield return new WaitForSeconds(1);
        JumpingProcess = false;
    }
}
