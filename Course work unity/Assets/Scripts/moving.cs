using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
 
    private Rigidbody PlayerRB;
    private GameObject[] Wheels = new GameObject[2];
    private WheelCollider[] WheelColls = new WheelCollider[2];
    public static float accelTimer;

   // private float torqueMoment;

    void Start()
    {
      //  torqueMoment = 0;
        Wheels[0] = GameObject.FindGameObjectWithTag("backWheel");
        Wheels[1] = GameObject.FindGameObjectWithTag("frontWheel");
        WheelColls[0] = GameObject.Find("bwCol").GetComponent<WheelCollider>();
        WheelColls[1] = GameObject.Find("fwCol").GetComponent<WheelCollider>();
        PlayerRB = GetComponent<Rigidbody>();
        accelTimer = 0;
    }

    void Update()
    {
        /******* TIMER  *******/
        if((controls.forward || controls.back))
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
                }
                else
                {
                    accelTimer -= 3*Time.deltaTime;
                    WheelColls[0].brakeTorque += 1f;
                    WheelColls[1].brakeTorque += 1f;
                }
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
        }
        Wheels[0].transform.Rotate(WheelColls[0].rpm *  Mathf.PI * Time.deltaTime, 0, 0);
        Wheels[1].transform.Rotate(WheelColls[1].rpm * Mathf.PI * Time.deltaTime, 0, 0);
        //Debug.Log(torqueMoment + "tm");

        if(controls.rotLeft || controls.rotRight)
        {
            if(controls.rotLeft)
            {
                // transform.Rotate(-Time.fixedDeltaTime*Mathf.PI*20, 0, 0);
                PlayerRB.AddTorque(transform.right * (-10f));
               // torqueMoment -= 5;
                //PlayerRB.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x + 1f, GetComponent<rotation>().getCurRot().y, 0), Time.fixedDeltaTime * 5));
                //GetComponent<rotation>().deltaCurRotX(1f);
                // transform.rotation *= new Quaternion(0.5f, 0, 0,1);
            }
            
            if(controls.rotRight)
            {
                PlayerRB.AddTorque(transform.right * (10f));
                //torqueMoment += 5;
                // transform.Rotate(Time.fixedDeltaTime*Mathf.PI*20, 0, 0);
                // PlayerRB.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x + 1f, GetComponent<rotation>().getCurRot().y, 0), Time.fixedDeltaTime * 5));
                // transform.rotation *= new Quaternion(-0.5f, 0, 0,1);
            }   
            GetComponent<rotation>().deltaCurRotX(PlayerRB.rotation.eulerAngles.x);
            /*
            if(transform.rotation.eulerAngles.x >= GetComponent<rotation>().getCurRot().x + 45f)
            {
                GetComponent<rotation>().deltaCurRotX(GetComponent<rotation>().getCurRot().x + 45f);
            }
            */
           /*
            if(transform.rotation.eulerAngles.x <= GetComponent<rotation>().getCurRot().x - 45f)
            {
                GetComponent<rotation>().deltaCurRotX(GetComponent<rotation>().getCurRot().x - 45f);
            }
            */
        }
        else
        {
           /* if(PlayerRB.angularVelocity.sqrMagnitude != 0)
            {
                PlayerRB.AddTorque(transform.right * Mathf.Sign(torqueMoment) * 2.5f);
                torqueMoment += Mathf.Sign(torqueMoment) * 2.5f;
            }
            */
        }
        GetComponent<rotation>().deltaCurRotX(PlayerRB.rotation.eulerAngles.x);
    }
}
