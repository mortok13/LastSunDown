using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTrackRotation : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<rotation>().RBLock(true);
            other.GetComponent<speedControl>().maxSpeed = 8f;
            other.GetComponent<rotation>().StopCoroutine("rotTimer");
            switch(this.tag)
            {
                case "rotRight":
                other.GetComponent<rotation>().setRotJoint(1);
                break;
                case "rotLeft":
                other.GetComponent<rotation>().setRotJoint(0);
                break;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
                
    }

    void OnTriggerExit(Collider other)
    {   
        if(other.tag == "Player")
        {
            other.GetComponent<speedControl>().maxSpeed = 20f;
            other.GetComponent<rotation>().resetRotJoint();
            other.GetComponent<rotation>().movingMode = !other.GetComponent<rotation>().movingMode;
            switch(this.tag)
            {
                case "rotRight":
                other.GetComponent<rotation>().rotStabilize(90f);
                break;
                case "rotLeft":
                other.GetComponent<rotation>().rotStabilize(-90f);
                break;
            }

        }
    }

}
