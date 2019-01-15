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
    void Update()
    {
        forward = Input.GetKeyDown(KeyCode.W);
        back = Input.GetKeyDown(KeyCode.S);
        rotLeft = Input.GetKeyDown(KeyCode.A);
        rotRight = Input.GetKeyDown(KeyCode.D);
    }
}
