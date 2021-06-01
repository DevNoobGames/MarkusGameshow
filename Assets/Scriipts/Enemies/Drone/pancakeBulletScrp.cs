using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pancakeBulletScrp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Rotate(0, 200 * Time.deltaTime, 0);
    }
}
