using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingLights : MonoBehaviour
{
    public Light theLight;
    

    public float speed = 500;
    public string action;

    Quaternion wantedRotation;

    public Quaternion goldenboxRotation;
    public Quaternion goldenboxRotation2;

    Quaternion centerRotation = Quaternion.Euler(90, 0, 0);

    private void Start()
    {
        action = "Rotating";
        InvokeRepeating("pickTarget", 0, 1);
    }

    private void Update()
    {
        goldenboxRotation2 = Quaternion.Euler(goldenboxRotation.x, goldenboxRotation.y, 0);

        if (action == "Rotating")
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, wantedRotation, Time.deltaTime * 500);
        }
        if (action == "goldenBox")
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, goldenboxRotation2, Time.deltaTime * 500);
        }
        if (action == "Center")
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, centerRotation, Time.deltaTime * 500);
        }
    }

    public void pickTarget()
    {
        float randvalX = Random.Range(10, 150);
        float randvalY = Random.Range(0, 360);
        wantedRotation = Quaternion.Euler(randvalX, randvalY, 0);
    }
}
