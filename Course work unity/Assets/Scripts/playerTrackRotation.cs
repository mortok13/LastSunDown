using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrackRotation : MonoBehaviour
{
    [SerializeField]
    public BoxCollider trigger;
    void Awake()
    {
    //    Physics.IgnoreLayerCollision(9,10);
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Player")
        {
            other.transform.GetComponent<Moving>().gameOver();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // other.GetComponent<rotation>().movingMode =  !other.GetComponent<rotation>().movingMode;
          //  other.GetComponent<speedControl>().maxSpeed = 40f;
            other.GetComponent<Rotation>().inRotation = true;
            other.GetComponent<Rotation>().StopAllCoroutines();
            switch(this.tag)
            {
                case "rotRight":
                other.GetComponent<Rotation>().setRotJoint(1);
                break;
                case "rotLeft":
                other.GetComponent<Rotation>().setRotJoint(2);
                break;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {   
        if(other.tag == "Player")
        {
           // other.GetComponent<speedControl>().maxSpeed = 100f;
            other.GetComponent<Rotation>().resetRotJoint();
            other.GetComponent<Rotation>().inRotation = false;
          //  other.GetComponent<rotation>().stab = false;
            Rotation.movingMode = !Rotation.movingMode;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMoving>().StopCoroutine("ChangeCameraMode");
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMoving>().StartCoroutine("ChangeCameraMode");
            switch(this.tag)
            {
                case "rotRight":
                other.GetComponent<Rotation>().rotStabilize(90f);
                break;
                case "rotLeft":
                other.GetComponent<Rotation>().rotStabilize(-90f);
                break;
            }
            GetComponent<BoxCollider>().enabled = false;
        }
    }

}
