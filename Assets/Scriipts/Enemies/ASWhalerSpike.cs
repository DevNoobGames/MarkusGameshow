using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASWhalerSpike : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(Resources.Load("Enemies/ExplosionSystem"), transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
