using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTrackRotation : MonoBehaviour
{
    private bool rotated = false;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<rotationLeft>().rotModeControl(-90f);

        }
    }
    void OnTriggerStay(Collider other)
    {
        
    }

    void OnTriggerExit(Collider other)
    {
    }

}
