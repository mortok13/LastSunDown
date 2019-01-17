using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    // public Vector3 PlayerPosition;
    public Rigidbody PlayerRB;
    public Rigidbody FFVector;
    public Rigidbody BFVector;
    public GameObject backwheel;
    public GameObject frontwheel;
    //private float speed,x,y,z;
    void Start()
    {        
        PlayerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        Debug.Log("Body position:"+ transform.position + "\n Body rotation:" + transform.rotation);
        if(controls.forward)
        {
            backwheel.transform.Rotate(1,0,0, Space.Self);
            frontwheel.transform.Rotate(1,0,0, Space.Self);
            PlayerRB.AddForce(transform.forward * 14);
        }
        if(controls.back)
        {
            backwheel.transform.Rotate(-1,0,0, Space.Self);
            frontwheel.transform.Rotate(-1,0,0, Space.Self);
            PlayerRB.AddForce(transform.forward * (-10));
        }
        if(controls.rotRight)
        {
            BFVector.AddForce(transform.up * (-100))         ;
        }
    }
}
