using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    // public Vector3 PlayerPosition;
    public Rigidbody PlayerRB;
    public GameObject backwheel;
    public GameObject frontwheel;
    //private float speed,x,y,z;
    void Start()
    {
        // speed = 0;
        // x = 0;
        // y = 0;
        // z = 0;
        PlayerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        //Debug.Log("Body position:"+ transform.position);
        if(controls.forward)
        {
            backwheel.transform.Rotate(1,0,0, Space.Self);
            frontwheel.transform.Rotate(1,0,0, Space.Self);
            PlayerRB.AddForce(transform.forward * 20);
        }
        if(controls.back)
        {
            backwheel.transform.Rotate(-1,0,0, Space.Self);
            frontwheel.transform.Rotate(-1,0,0, Space.Self);
            PlayerRB.AddForce(transform.forward * (-20));
        }
        // if(controls.rotLeft)
        // {
        //     transform.rotation()
        // }
    }
}
