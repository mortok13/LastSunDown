using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * control sets
 */

public class controls : MonoBehaviour
{
    public static bool forward, back, rotLeft, rotRight;

    // Update is called once per frame
    void LateUpdate()
    {
        forward = Input.GetKey(KeyCode.W);
        back = Input.GetKey(KeyCode.S);
        rotLeft = Input.GetKey(KeyCode.A);
        rotRight = Input.GetKey(KeyCode.D);
    }
}
