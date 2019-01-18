using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    // public Vector3 PlayerPosition;
    private Rigidbody PlayerRB;
    public Vector3 FlipVector;
    public GameObject backwheel;
    public GameObject frontwheel;
    public static float accelTimer;
    //private float speed,x,y,z;
    void Start()
    {        
        PlayerRB = GetComponent<Rigidbody>();
        accelTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(controls.forward)
        {
            accelTimer += Time.deltaTime;
        }
        else if(controls.back)
        {
            accelTimer -= Time.deltaTime;
        }

        if(accelTimer <= 0)
        {
            accelTimer = 0;
        }
    }

    void FixedUpdate()
    {
        //Debug.Log("Body position:"+ transform.position + "\n Body rotation:" + transform.rotation);
        //Debug.Log("timer " + accelTimer);
        if(controls.forward)
        {
            backwheel.transform.Rotate(1,0,0, Space.Self);
            frontwheel.transform.Rotate(1,0,0, Space.Self);
            if(!stats.isInAir)
            {
            PlayerRB.AddForce(transform.forward * 14);
            }
        }
        if(controls.back)
        {
            backwheel.transform.Rotate(-1,0,0, Space.Self);
            frontwheel.transform.Rotate(-1,0,0, Space.Self);
            if(!stats.isInAir)
            {
            PlayerRB.AddForce(transform.forward * (-10));
            }
        }
        if(controls.rotRight)
        {
           PlayerRB.AddTorque(transform.right * 1f);
        }
        if(controls.rotLeft)
        {
            PlayerRB.AddTorque(transform.right * (-1f));
        }
    }
}
