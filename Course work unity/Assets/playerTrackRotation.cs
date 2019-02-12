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
            switch(this.tag)
            {
                case "rotRight":
                other.GetComponent<rotationLeft>().rotModeControl(90f);
                break;
                case "rotLeft":
                other.GetComponent<rotationLeft>().rotModeControl(-90f);
                break;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        
    }

    void OnTriggerExit(Collider other)
    {
    }

}
