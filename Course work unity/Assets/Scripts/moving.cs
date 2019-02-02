using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    // public Vector3 PlayerPosition;
    private Rigidbody PlayerRB;
    //public GameObject Cube;

    public GameObject[] Wheels = new GameObject[2];
    public WheelCollider[] WheelColls = new WheelCollider[2];
    public static float accelTimer;
    void Start()
    {
        Wheels[0] = GameObject.FindGameObjectWithTag("backWheel");
        Wheels[1] = GameObject.FindGameObjectWithTag("frontWheel");
        WheelColls[0] = GameObject.Find("bwCol").GetComponent<WheelCollider>();
        WheelColls[1] = GameObject.Find("fwCol").GetComponent<WheelCollider>();
        PlayerRB = GetComponent<Rigidbody>();
       //PlayerRB.centerOfMass = (Wheels[0].transform.position - Wheels[1].transform.position)/2;
     //  Physics.IgnoreLayerCollision(9,10);
        accelTimer = 0;
    //    for(int i = 0; i < 160; i++)
    //    {
    //        Instantiate(Cube, new Vector3(2*i, 1, 1), Quaternion.Euler(0, 180, 0));
    //    }
    }

    // Update is called once per frame
    void Update()
    {
        /******* TIMER  *******/
        if((controls.forward || controls.back)&& (WheelColls[0].isGrounded || WheelColls[1].isGrounded))
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
                accelTimer -= Time.deltaTime;
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

        if(accelTimer <= -Mathf.PI/15)
        {
            accelTimer = -Mathf.PI/15;
        }
        else if(accelTimer >= Mathf.PI)
        {
            accelTimer = Mathf.PI;
        }

        /******* TIMER  *******/
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
                foreach(WheelCollider wheelColl in WheelColls)
                {
                    if(wheelColl.isGrounded)
                    {
                    wheelColl.motorTorque = speedControl.speed;
                    }
                wheelColl.brakeTorque = 0f;
                }
            // WheelColls[0].motorTorque = speedControl.speed;
            // WheelColls[1].motorTorque = speedControl.speed;
            // WheelColls[0].brakeTorque = 0f;
            // WheelColls[1].brakeTorque = 0f;
            //Wheels[0].transform.Rotate(WheelColls[0].rpm * Time.deltaTime, 0, 0);
            //Wheels[1].transform.Rotate(WheelColls[1].rpm * Time.deltaTime, 0, 0);
            }
            else
            {
                if(speedControl.speed <= 0)
                {
                    foreach(WheelCollider wheelColl in WheelColls)
                    {
                    wheelColl.brakeTorque = 0f;
                    wheelColl.motorTorque = speedControl.speed;
                    }
                // WheelColls[0].brakeTorque = 0f;
                // WheelColls[1].brakeTorque = 0f;
                // WheelColls[0].motorTorque = speedControl.speed;
                // WheelColls[1].motorTorque = speedControl.speed;
                }
                else
                {
                    accelTimer -= 3*Time.deltaTime;
                    WheelColls[0].brakeTorque += 1f;
                    WheelColls[1].brakeTorque += 1f;
                }
            //Wheels[0].transform.Rotate(WheelColls[0].rpm * Time.deltaTime, 0, 0);
           // Wheels[1].transform.Rotate(WheelColls[1].rpm * Time.deltaTime, 0, 0);
            }
        }
        else
        {
            foreach(WheelCollider wheelColl in WheelColls)
            {
                if(wheelColl.motorTorque > 0.2f)
                {
                    wheelColl.motorTorque -= 0.4f;
                }
                else if(wheelColl.motorTorque < -0.2f)
                {
                    wheelColl.motorTorque += 0.4f;
                }
                else
                {
                        wheelColl.motorTorque = 0f;
                }
            }
         // WheelColls[0].motorTorque = 0f;
         // WheelColls[1].motorTorque = 0f;
        //   if(speedControl.speed != 0)
        //   {
        //     WheelColls[0].brakeTorque = 1f;
        //     WheelColls[1].brakeTorque = 1f;
        //   }
        }
        Wheels[0].transform.Rotate(WheelColls[0].rpm *  Mathf.PI * Time.deltaTime, 0, 0);
        Wheels[1].transform.Rotate(WheelColls[1].rpm * Mathf.PI * Time.deltaTime, 0, 0);

        if(controls.rotLeft)
        {
            PlayerRB.AddTorque(transform.right * (-20f));
        }
        if(controls.rotRight)
        {
            PlayerRB.AddTorque(transform.right * (20f));
        }
       // Wheels[0].transform.Rotate(WheelColls[0].rpm * Time.deltaTime, 0, 0);
        //Wheels[1].transform.Rotate(WheelColls[1].rpm * Time.deltaTime, 0, 0);
    }
}
