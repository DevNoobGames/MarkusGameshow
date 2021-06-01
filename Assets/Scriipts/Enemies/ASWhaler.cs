using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASWhaler : MonoBehaviour
{
    public GameObject Player;
    public bool loaded;
    public Transform shootPos;
    public int ammo;

    public ASWhalerManager asMan;

    public GameObject eyeR;
    public GameObject eyeL;

    public float eyeRhealth = 100;
    public float eyeLhealth = 100;

    public Material redMat;
    public Material blackMat;
    public Material whiteMat;

    void Start()
    {
        //Player = GameObject.FindGameObjectWithTag("FPSController");
        loaded = false; //set loaded only after his animation is finished!!!
    }

    // Update is called once per frame
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
            GameObject bullets = Instantiate(Resources.Load("Enemies/ASWhaler/ASWhalerSpike"), shootPos.position, Quaternion.identity) as GameObject;
            bullets.transform.LookAt(Player.transform);
            Rigidbody rb = bullets.GetComponent<Rigidbody>();
            rb.velocity = (Player.transform.position - bullets.transform.position).normalized * 40;
            StartCoroutine(Reload());
            if (ammo <= 0)
            {
                StartCoroutine(newAmmo());
            }
        }
    }
    
    public void eyeLHit(float damage)
    {
        if (eyeLhealth > 0 && ammo <= 0)
        {
            eyeLhealth -= damage;
        }
        if (eyeLhealth <= 0)
        {
            eyeL.GetComponent<MeshRenderer>().material = whiteMat;
            eyeL.transform.GetChild(0).gameObject.SetActive(false);
        }
        checkBothEyes();
    }

    public void eyeRHit(float damage)
    {
        if (eyeRhealth > 0 && ammo <= 0)
        {
            eyeRhealth -= damage;
        }
        if (eyeRhealth <= 0)
        {
            eyeR.GetComponent<MeshRenderer>().material = whiteMat;
            eyeR.transform.GetChild(0).gameObject.SetActive(false);
        }
        checkBothEyes();
    }

    public void checkBothEyes()
    {
        if (eyeLhealth <= 0 && eyeRhealth <= 0)
        {
            loaded = false;
            ammo = 0;
            asMan.startStowing();
        }
    }

    IEnumerator newAmmo()
    {
        if (eyeRhealth > 0)
        {
            eyeR.GetComponent<MeshRenderer>().material = redMat;
            eyeR.transform.GetChild(0).gameObject.SetActive(true);
        }
        if (eyeLhealth > 0)
        {
            eyeL.GetComponent<MeshRenderer>().material = redMat;
            eyeL.transform.GetChild(0).gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(5);
        ammo = 10;
        if (eyeRhealth > 0)
        {
            eyeR.GetComponent<MeshRenderer>().material = blackMat;
        }
        if (eyeLhealth > 0)
        {
            eyeL.GetComponent<MeshRenderer>().material = blackMat;
        }
        eyeR.transform.GetChild(0).gameObject.SetActive(false);
        eyeL.transform.GetChild(0).gameObject.SetActive(false);


    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.5f);
        loaded = true;
    }
}
