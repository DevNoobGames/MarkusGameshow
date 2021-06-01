using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    public float speed = 40;
    public AudioSource alarmAudio;
    public goldenBoxManager gbm;
    public AudioSource backgroundAudio;

    public List<GameObject> drones = new List<GameObject>();

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }

    public void startAttack()
    {
        StartCoroutine(Attack());
    }

    public void droneDestroyed(GameObject droneDest)
    {
        drones.Remove(droneDest);
        Destroy(droneDest);
        if (drones.Count <= 0)
        {
            backToNormal();
        }
    }

    public void backToNormal()
    {
        foreach (movingLights ml in gbm.movingL)
        {
            ml.theLight.color = Color.white;
        }
        //gbm.canOpen = true;
        StartCoroutine(waitASec());
        gbm.OpenTzeSaws(false);
        gbm.OpenTzeFireParticles(false);
        gbm.OpenTzeMovingFire(false);
        gbm.closeInstructionText();
        backgroundAudio.Stop();
        gbm.normalBGMusic.Play();
        gbm.GoldenBoxCounter();
        gbm.soundCredits("Now Playing: One Man Symphony - Mohnfeld (Part 1)");
    }
    IEnumerator waitASec()
    {
        yield return new WaitForSeconds(1);
        gbm.canOpen = true;
    }

    IEnumerator Attack()
    {
        alarmAudio.Play();

        foreach (movingLights ml in gbm.movingL)
        {
            ml.theLight.color = Color.red;
        }

        yield return new WaitForSeconds(5);
        gbm.OpenTzeSaws(true);
        gbm.OpenTzeFireParticles(true);
        gbm.OpenTzeMovingFire(true);
        gbm.soundCredits("Now Playing: Zander Noriega - Fight Them Until We Cant");
        gbm.openInstructionText("Shoot them! Be careful, they shoot pancakes.");

        yield return new WaitForSeconds(3);
        backgroundAudio.Play();

        GameObject drone1 = Instantiate(Resources.Load("Enemies/1CreeperDrone"), transform.position, Quaternion.identity) as GameObject;
        drone1.transform.parent = gameObject.transform;
        drone1.transform.localPosition = new Vector3(0, 1, 55);
        GameObject drone2 = Instantiate(Resources.Load("Enemies/1CreeperDrone"), transform.position, Quaternion.identity) as GameObject;
        drone2.transform.parent = gameObject.transform;
        drone2.transform.localPosition = new Vector3(0, 1, -55);

        drones.Add(drone1);
        drones.Add(drone2);
    }
}
