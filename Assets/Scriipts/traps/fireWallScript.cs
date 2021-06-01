using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireWallScript : MonoBehaviour
{
    public Vector3 positionDisplacement;
    private Vector3 positionOrigin;
    private float timePassed;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        positionOrigin = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;

        transform.localPosition = Vector3.Lerp(positionOrigin, positionOrigin + positionDisplacement,
        Mathf.PingPong(timePassed * speed ,1));
    }
}
