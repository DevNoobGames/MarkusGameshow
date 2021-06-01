using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markusMain : MonoBehaviour
{
    public GameObject Player;
    public Animation talkAnim;

    [Header ("Audios")]
    public AudioSource firstTimeTalking;

    private void Start()
    {
        firstTimeTalking.Play();
        StartCoroutine(talkAnimTimer(firstTimeTalking.clip.length));
    }

    void Update()
    {
        Vector3 targetPostition = new Vector3(Player.transform.position.x,
                                this.transform.position.y,
                                Player.transform.position.z);
        this.transform.LookAt(targetPostition);
    }

    IEnumerator talkAnimTimer(float talkTime)
    {
        talkAnim.Play();
        yield return new WaitForSeconds(talkTime);
        talkAnim.Stop();
    }
}
