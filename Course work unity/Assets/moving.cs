using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    // public Vector3 PlayerPosition;
    public Rigidbody PlayerRB;
    private float speed,x,y,z;
    private Quaternion rotationLeft()
    void Start()
    {
        speed = 0;
        x = 0;
        y = 0;
        z = 0;
        PlayerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        Debug.Log("Body position:"+ transform.position);
        if(controls.forward)
        {
            PlayerRB.AddForce(transform.forward * 2);
        }
        if(controls.back)
        {
            PlayerRB.AddForce(transform.forward * (-2));
        }
        if(controls.rotLeft)
        {
            transform.rotation()
        }
    }
}
