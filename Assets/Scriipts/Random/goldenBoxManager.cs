using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class goldenBoxManager : MonoBehaviour
{
    public AudioSource rewardAudio;
    public AudioSource failAudio;
    public AudioSource normalBGMusic;
    public TextMeshPro rewardText;

    public movingLights[] movingL;
    public devNoobPlayer dnPlayer;

    public GameObject instructionTextOBJ;
    public TextMeshProUGUI instructionText;
    public GameObject soundCreditsTextOBJ;
    public TextMeshProUGUI soundCreditsText;

    public GameObject winPanel;

    [Header ("Traps")]
    public GameObject[] Saws;
    public GameObject[] FirePoleParticles;
    public GameObject[] FirePoleMovers;

    [Header ("attack managers")]
    public smallmarkusSpawnManager smallMarkusManager;
    public ASWhalerManager asWhalerMan;
    public FireDudeManage fireDudeMan;
    public DroneManager droneMan;
    public DaniManager danMan;
    public BJManager bjMan;
    public bool canOpen;

    [System.Serializable] public delegate void spawnRewardMethod();
    [SerializeField] public List<spawnRewardMethod> spawnReward = new List<spawnRewardMethod>();

    private void Start()
    {
        Saws = GameObject.FindGameObjectsWithTag("trapSaw");
        FirePoleParticles = GameObject.FindGameObjectsWithTag("firePoleParticles");
        FirePoleMovers = GameObject.FindGameObjectsWithTag("firePoleMovers");

        canOpen = true;
        CreateList();
        //soundCredits("Now Playing: One Man Symphony - Mohnfeld (Part 1)");
    }

    void CreateList()
    {
        spawnReward.Add(moreMoney1);
        spawnReward.Add(moreMoney1);
        //spawnReward.Add(moreMoney1);
        spawnReward.Add(moreMoney2);
        spawnReward.Add(moreMoney2);
        //spawnReward.Add(moreMoney2);
        spawnReward.Add(moreMoney5);
        spawnReward.Add(moreMoney100);
        spawnReward.Add(ResetLife);
        //spawnReward.Add(ResetLife);

        spawnReward.Add(smallMarkusAttack);
        spawnReward.Add(smallMarkusAttack);
        spawnReward.Add(ASWhalerAttack);
        spawnReward.Add(ASWhalerAttack);
        spawnReward.Add(FireDudeAttack);
        spawnReward.Add(droneAttack);
        spawnReward.Add(DaniAttack);
        spawnReward.Add(DaniAttack);
        spawnReward.Add(BJAttack);
    }

    public void pickAReward()
    {
        if (spawnReward.Count > 0)
        {
            int randVal = Random.Range(0, spawnReward.Count);
            spawnReward[randVal]();
            spawnReward.RemoveAt(randVal);
        }
    }

    public void GoldenBoxCounter()
    {
        Debug.Log(spawnReward.Count);
        if (spawnReward.Count == 0)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            winPanel.SetActive(true);
            winPanel.GetComponent<winMenuScript>().setScoreText(dnPlayer.totalHealthLost);
        }
    }

    public void ResetLife()
    {
        rewardText.text = "FULL HEALTH!";
        dnPlayer.addHealth(999);
        StartCoroutine(playSound(rewardAudio));
        StartCoroutine(rotateTheLightsBackToStandard(4, "Rotating"));
        StartCoroutine(waitToOpen(4));
        Invoke("GoldenBoxCounter", 2);
    }

    public void BJAttack()
    {
        rewardText.text = "ELECTRIC DRONES!";
        StartCoroutine(playSound(failAudio));
        StartCoroutine(BJStarted());
    }
    IEnumerator BJStarted()
    {
        yield return new WaitForSeconds(4);
        bjMan.SpawnStart();
        StartCoroutine(rotateTheLightsBackToStandard(0, "Rotating"));
    }

    public void DaniAttack()
    {
        rewardText.text = "DANI!";
        StartCoroutine(playSound(failAudio));
        StartCoroutine(danStarted());
    }
    IEnumerator danStarted()
    {
        yield return new WaitForSeconds(4);
        danMan.startSeq();
        StartCoroutine(rotateTheLightsBackToStandard(0, "Center"));
    }

    public void droneAttack()
    {
        rewardText.text = "PANCAKE DRONES!";
        StartCoroutine(playSound(failAudio));
        StartCoroutine(droneStarted());
    }
    IEnumerator droneStarted()
    {
        yield return new WaitForSeconds(4);
        droneMan.startAttack();
        StartCoroutine(rotateTheLightsBackToStandard(0, "Rotating"));
    }

    public void FireDudeAttack()
    {
        rewardText.text = "FIRE JUMPER!";
        StartCoroutine(playSound(failAudio));
        StartCoroutine(startAFireDude());
    }
    IEnumerator startAFireDude()
    {
        yield return new WaitForSeconds(4);
        fireDudeMan.startAttack();
        StartCoroutine(rotateTheLightsBackToStandard(0, "Rotating"));
    }

    public void ASWhalerAttack()
    {
        rewardText.text = "ASWHALER!";
        StartCoroutine(playSound(failAudio));
        StartCoroutine(startASWhaler());
    }
    IEnumerator startASWhaler()
    {
        yield return new WaitForSeconds(4);
        asWhalerMan.startAttack();
        StartCoroutine(rotateTheLightsBackToStandard(0, "Center"));
    }

    public void smallMarkusAttack()
    {
        rewardText.text = "SMALL MARKUS!";
        StartCoroutine(playSound(failAudio));
        StartCoroutine(startsmallMark());
    }
    IEnumerator startsmallMark()
    {
        yield return new WaitForSeconds(4);
        smallMarkusManager.spawnAMarkus();
        StartCoroutine(rotateTheLightsBackToStandard(0, "Rotating"));
    }

    IEnumerator rotateTheLightsBackToStandard(float waittime, string action)
    {
        yield return new WaitForSeconds(waittime);
        foreach (movingLights ml in movingL)
        {
            ml.action = action;
            ml.GetComponent<Light>().range = 66;
            ml.GetComponent<Light>().spotAngle = 54;
        }
    }

    public void moreMoney1()
    {
        rewardText.text =  "1000$!!";
        dnPlayer.addMoney(1000);
        StartCoroutine(playSound(rewardAudio));
        StartCoroutine(rotateTheLightsBackToStandard(4, "Rotating"));
        StartCoroutine(waitToOpen(4));
        Invoke("GoldenBoxCounter", 2);
    }
    public void moreMoney2()
    {
        rewardText.text = "2000$!!";
        dnPlayer.addMoney(2000);
        StartCoroutine(playSound(rewardAudio));
        StartCoroutine(rotateTheLightsBackToStandard(4, "Rotating"));
        StartCoroutine(waitToOpen(4));
        Invoke("GoldenBoxCounter", 2);
    }
    public void moreMoney5()
    {
        rewardText.text = "5000$!!";
        dnPlayer.addMoney(5000);
        StartCoroutine(playSound(rewardAudio));
        StartCoroutine(rotateTheLightsBackToStandard(4, "Rotating"));
        StartCoroutine(waitToOpen(4));
        Invoke("GoldenBoxCounter", 2);
    }
    public void moreMoney100()
    {
        rewardText.text = "100000$!!";
        dnPlayer.addMoney(100000);
        StartCoroutine(playSound(rewardAudio));
        StartCoroutine(rotateTheLightsBackToStandard(4, "Rotating"));
        StartCoroutine(waitToOpen(4));
        Invoke("GoldenBoxCounter", 2);
    }

    IEnumerator waitToOpen(float time)
    {
        yield return new WaitForSeconds(time);
        canOpen = true;
    }

    public void OpenTzeSaws(bool open)
    {
        foreach (GameObject saw in Saws)
        {
            Animation animation = saw.GetComponent<Animation>();
            if (open)
            {
                animation["circlesawanim"].time = 0;
                animation["circlesawanim"].speed = 1;
            }
            else
            {
                animation["circlesawanim"].time = animation["circlesawanim"].length;
                animation["circlesawanim"].speed = -1;
            }
            animation.Play("circlesawanim");
        }
    }
    public void OpenTzeFireParticles(bool open)
    {
        foreach (GameObject fp in FirePoleParticles)
        {
            if (open)
            {
                fp.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                fp.GetComponent<ParticleSystem>().Stop();
            }
        }
    }
    public void OpenTzeMovingFire(bool open)
    {
        foreach (GameObject fpm in FirePoleMovers)
        {
            if (open)
            {
                fpm.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                fpm.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    public void openInstructionText(string instructions)
    {
        Animation animationT = instructionTextOBJ.GetComponent<Animation>();
        instructionText.text = instructions;
        animationT["instructiontextAnim"].time = 0;
        animationT["instructiontextAnim"].speed = 1;
        animationT.Play("instructiontextAnim");
    }
    public void closeInstructionText()
    {
        Animation animationT = instructionTextOBJ.GetComponent<Animation>();
        animationT["instructiontextAnim"].time = animationT["instructiontextAnim"].length;
        animationT["instructiontextAnim"].speed = -1;
        animationT.Play("instructiontextAnim");
    }
    public void soundCredits(string credits)
    {
        StartCoroutine(soundCreditsCour(credits));
    }
    IEnumerator soundCreditsCour(string credits)
    {
        Animation animationA = soundCreditsTextOBJ.GetComponent<Animation>();
        soundCreditsText.text = credits;
        animationA["instructiontextAnim"].time = 0;
        animationA["instructiontextAnim"].speed = 1;
        animationA.Play("instructiontextAnim");
        yield return new WaitForSeconds(7);
        animationA["instructiontextAnim"].time = animationA["instructiontextAnim"].length;
        animationA["instructiontextAnim"].speed = -1;
        animationA.Play("instructiontextAnim");
    }



    IEnumerator playSound(AudioSource audio)
    {
        yield return new WaitForSeconds(1);
        audio.Play();
    }
}
