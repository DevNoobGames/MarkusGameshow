using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDudeLookat : MonoBehaviour
{
    public FireDudeScript fds;

    void Update()
    {
        /*bool check1 = fds.IsGrounded();
        if (!check1)
        {
            Vector3 targetPostition = new Vector3(fds.targetPos.x,
                                    this.transform.position.y,
                                    fds.targetPos.z);
            this.transform.LookAt(targetPostition);

            transform.position = Vector3.MoveTowards(transform.position, fds.targetPos, 20 * Time.deltaTime);
        }*/
    }
}
