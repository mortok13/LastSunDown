using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 * control sets
 */

public class Controls : MonoBehaviour
{
    public static bool forward, back, rotLeft, rotRight;
    void Awake()
    {
       forward = back = rotLeft = rotRight = false; 
    }


    public void ForceForward()
    {
        forward = true;
    }

    public void ForceBack()
    {
        back = true;
    }

    public void ForceRotLeft()
    {
        rotLeft = true;
    }

    public void ForceRotRight()
    {
        rotRight = true;
    }


    
    public void UnforceForward()
    {
        forward = false;
    }

    public void UnforceBack()
    {
        back = false;
    }

    public void UnforceRotLeft()
    {
        rotLeft = false;
    }

    public void UnforceRotRight()
    {
        rotRight = false;
    }


}
