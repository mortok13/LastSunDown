using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    // public Vector3 PlayerPosition;
    private Rigidbody PlayerRB;
    public Vector3 FlipVector;

    public GameObject[] Wheels;
    public WheelCollider[] WheelColls;
    public static float accelTimer;
    //private float speed,x,y,z;
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody>();
       PlayerRB.centerOfMass = new Vector3(0, -0.0344f,0);
       // accelTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(controls.forward || controls.back)
        {
            if(controls.forward)
            {
                accelTimer += Time.deltaTime;
            }
            else
            {
                accelTimer -= Time.deltaTime;
            }
        }
        else
        {
            if(accelTimer > 0.03f)
            {
                accelTimer -= 2*Time.deltaTime;
            }
            else if(accelTimer < -0.03f)
            {
                accelTimer += 3*Time.deltaTime;
            }
            else
            {
                accelTimer = 0;
            }
        }

        if(accelTimer <= -Mathf.PI/2)
        {
            accelTimer = -Mathf.PI/2;
        }
        else if(accelTimer >= Mathf.PI)
        {
            accelTimer = Mathf.PI;
        }
    }

    void FixedUpdate()
    {
        if(controls.forward && controls.back)
        {
            WheelColls[0].motorTorque = 0;
            WheelColls[1].motorTorque = 0;
        }
        else if(controls.forward || controls.back)
        {
            if(controls.forward)
            {
            WheelColls[0].motorTorque = speedControl.speed;
            WheelColls[1].motorTorque = speedControl.speed;
            //Wheels[0].transform.Rotate(WheelColls[0].rpm * Time.deltaTime, 0, 0);
            //Wheels[1].transform.Rotate(WheelColls[1].rpm * Time.deltaTime, 0, 0);
            }
            else
            {
            WheelColls[0].motorTorque = speedControl.speed;
            WheelColls[1].motorTorque = speedControl.speed;
            //Wheels[0].transform.Rotate(WheelColls[0].rpm * Time.deltaTime, 0, 0);
           // Wheels[1].transform.Rotate(WheelColls[1].rpm * Time.deltaTime, 0, 0);
            }
        }
        else
        {
          WheelColls[0].motorTorque = 0f;
          WheelColls[1].motorTorque = 0f;
        }
        Wheels[0].transform.Rotate(WheelColls[0].rpm * Time.deltaTime, 0, 0);
        Wheels[1].transform.Rotate(WheelColls[1].rpm * Time.deltaTime, 0, 0);

        if(controls.rotLeft)
        {
            PlayerRB.AddTorque(transform.right * (-10f));
        }
        if(controls.rotRight)
        {
            PlayerRB.AddTorque(transform.right * (10f));
        }
       // Wheels[0].transform.Rotate(WheelColls[0].rpm * Time.deltaTime, 0, 0);
        //Wheels[1].transform.Rotate(WheelColls[1].rpm * Time.deltaTime, 0, 0);
    }
}
