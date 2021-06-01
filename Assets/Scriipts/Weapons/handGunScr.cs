using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class handGunScr : MonoBehaviour
{
    public float ammoLoaded; //cuurenlty in clip
    public float ammoClipSize; //max bullets in clip
    public float ammoHolding; //bullets on you as a preson which are not in the gun
    public float damagePerShot;
    public float reloadTime;
    private bool isReloading;
    public GameObject flash;
    public TextMeshProUGUI ammoText;
    public goldenBoxManager gbm;
    public devNoobPlayer dnPlayer;
    public int gunNr;

    [Header("sounds")]
    public AudioSource shotSound;
    public AudioSource reloadSound;
    public AudioSource tenseAnnouncement;

    [Header ("Animations")]
    public Animation Anim;
    public string shootAnimationName;
    public string reloadAnimationName;

    public bool automatic;
    bool automaticShot;
    bool bought;

    private void Start()
    {
        setAmmoText();
        automaticShot = true;
        bought = false;
    }

    IEnumerator automaticReload()
    {
        yield return new WaitForSeconds(0.1f);
        automaticShot = true;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale > 0)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("goldenBox") && gbm.canOpen)
                {
                    if (gunNr == 1 || gunNr == 2)
                    {
                        gbm.canOpen = false;
                        gbm.normalBGMusic.Stop();
                        goldenBoxIndi gbi = hit.transform.GetComponent<goldenBoxIndi>();
                        
                        foreach (movingLights ml in gbm.movingL)
                        {
                            ml.action = "goldenBox";
                            ml.goldenboxRotation = gbi.lightsRotation;
                            ml.GetComponent<Light>().range = 150;
                            ml.GetComponent<Light>().spotAngle = 25;
                        }
                        gbi.lightSource.SetActive(false);
                        hit.transform.tag = "Untagged";
                        StartCoroutine(openGoldenBox(gbi));
                    }
                }
                if (hit.collider.transform.CompareTag("storeFullHealth") && !bought)
                {
                    if (Vector3.Distance(transform.position, hit.collider.transform.position) < 4)
                    {
                        if (gunNr == 1 || gunNr == 2)
                        {
                            if (dnPlayer.money >= 1000 && dnPlayer.health < 100)
                            {
                                bought = true;
                                dnPlayer.addMoney(-1000);
                                dnPlayer.addHealth(999);
                            }
                        }
                    }
                }
                if (hit.collider.transform.CompareTag("storeAmmo100") && !bought)
                {
                    if (Vector3.Distance(transform.position, hit.collider.transform.position) < 4)
                    {
                        if (gunNr == 1 || gunNr == 2)
                        {
                            if (dnPlayer.money >= 200)
                            {
                                bought = true;
                                dnPlayer.addMoney(-200);
                                foreach (handGunScr hgs in dnPlayer.gunScripts)
                                {
                                    hgs.ammoHolding += 100;
                                    setAmmoText();
                                }
                            }
                        }
                    }
                }
                if (hit.collider.transform.CompareTag("storeGunGrade") && !bought && !dnPlayer.bought && dnPlayer.activeGuns < 5)
                {
                    if (Vector3.Distance(transform.position, hit.collider.transform.position) < 4)
                    {
                        if (gunNr == 1 || gunNr == 2)
                        {
                            if (dnPlayer.money >= 2000)
                            {
                                dnPlayer.bought = true;
                                dnPlayer.activeGuns += 1;
                                bought = true;
                                dnPlayer.addMoney(-2000);

                                dnPlayer.gunObjects[0].SetActive(false);
                                dnPlayer.gunObjects[dnPlayer.activeGuns].SetActive(true);

                                foreach (handGunScr hgd in dnPlayer.gunScripts)
                                {
                                    hgd.ammoLoaded = hgd.ammoClipSize;
                                }
                            }
                        }
                    }
                }

            }
        }

        if (Input.GetMouseButton(0) && Time.timeScale > 0)
        {


            if (ammoLoaded > 0 && !isReloading && automaticShot)
            {
                automaticShot = false;
                if (automatic)
                {
                    StartCoroutine(automaticReload());
                }

                ammoLoaded -= 1;
                setAmmoText();
                Anim.Play(shootAnimationName);
                flash.SetActive(true);
                if (!automatic)
                {
                    if (gameObject.activeInHierarchy)
                    {
                        StartCoroutine(TurnOffFlash());
                    }
                }
                shotSound.Play();

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("smallMarkus"))
                    {
                        hit.transform.GetComponent<smallMarkus>().gotHit(damagePerShot);
                    }
                    if (hit.transform.CompareTag("EyeL"))
                    {
                        hit.transform.GetComponentInParent<ASWhaler>().eyeLHit(damagePerShot);
                    }
                    if (hit.transform.CompareTag("EyeR"))
                    {
                        hit.transform.GetComponentInParent<ASWhaler>().eyeRHit(damagePerShot);
                    }
                    if (hit.collider.transform.CompareTag("hitObj"))
                    {
                        hit.collider.transform.GetComponent<FireDudeHitPoint>().fds.gotHit(damagePerShot);
                    }
                    if (hit.collider.transform.CompareTag("creeperDrone"))
                    {
                        hit.collider.transform.GetComponentInParent<Drone1Creeper>().gotHit(damagePerShot);
                    }
                    if (hit.collider.transform.CompareTag("DanisBall"))
                    {
                        hit.collider.transform.parent.transform.parent.GetComponent<DaniMainScr>().gotHit(damagePerShot);
                    }
                    if (hit.collider.transform.CompareTag("droneEye"))
                    {
                        hit.collider.transform.parent.transform.parent.GetComponent<BJScript>().gotHit(damagePerShot);
                    }

                }
            }
            if (ammoLoaded <= 0 && ammoHolding > 0 && !isReloading)
            {
                isReloading = true;
                StartCoroutine(reloading());
            }
        }

        if (ammoLoaded <= 0)
        {
            flash.SetActive(false);
            automaticShot = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            automaticShot = true;
            flash.SetActive(false); 
            bought = false;
            dnPlayer.bought = false;
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && ammoHolding > 0 && ammoLoaded < ammoClipSize && Time.timeScale > 0)
        {
            isReloading = true;
            StartCoroutine(reloading());
        }
    }

    IEnumerator openGoldenBox(goldenBoxIndi gbi)
    {
        tenseAnnouncement.Play();
        yield return new WaitForSeconds(4);
        tenseAnnouncement.Stop();
        yield return new WaitForSeconds(0.5f);

        gbm.rewardText = gbi.rewardText;
        gbi.hingeAnimation.Play();
        gbi.hingeAudio.Play();
        gbm.pickAReward();
    }

    IEnumerator reloading()
    {
        Anim.Play(reloadAnimationName);
        flash.SetActive(false);
        reloadSound.Play();

        yield return new WaitForSeconds(reloadTime);

        float ammoToLoad = ammoClipSize - ammoLoaded;

        if (ammoToLoad <= ammoHolding)
        {
            ammoLoaded = ammoClipSize;
            ammoHolding -= ammoToLoad;
        }
        else if (ammoToLoad > ammoHolding)
        {
            ammoLoaded += ammoHolding;
            ammoHolding = 0;
        }
        setAmmoText();
        isReloading = false;
    }

    IEnumerator TurnOffFlash()
    {
        yield return new WaitForSeconds(0.15f);
        flash.SetActive(false);
    }

    public void setAmmoText()
    {
        if (gunNr == 1)
        {
            ammoText.text = ammoLoaded + "/" + ammoHolding;
        }
        else
        {
            ammoText.text = ammoLoaded + "/" + ammoHolding + "\n" + "Per gun";
        }
    }
}
