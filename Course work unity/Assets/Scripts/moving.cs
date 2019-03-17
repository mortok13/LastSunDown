using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
 
    private Rigidbody PlayerRB;
    private GameObject[] frontWheels;
    private GameObject[] backWheels;
    private WheelCollider[] WheelColls;
    public static float accelTimer;

   // private float torqueMoment;

    void Start()
    {
      //  torqueMoment = 0;
        frontWheels = GameObject.FindGameObjectsWithTag("frontWheel");
        backWheels = GameObject.FindGameObjectsWithTag("backWheel");
        WheelColls = FindObjectsOfType<WheelCollider>();
        foreach(WheelCollider wheelcol in WheelColls)
        {
            wheelcol.ConfigureVehicleSubsteps(10f, 7,10);
        }
        PlayerRB = GetComponent<Rigidbody>();
        //PlayerRB.centerOfMass = GetComponent<BoxCollider>().center;
        accelTimer = 0;
      //  PlayerRB.centerOfMass = new Vector3(0,-0.07f,0);
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
            if(Mathf.Abs(accelTimer) > 0.1f)
            {
                if(controls.forward)
                {
                    foreach(WheelCollider wheelColl in WheelColls)
                    {
                    wheelColl.motorTorque = speedControl.speed;
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
                        foreach(WheelCollider wheelColl in WheelColls)
                        {
                        wheelColl.brakeTorque += 1f;
                       // wheelColl.motorTorque = speedControl.speed;
                        }
                       // WheelColls[0].brakeTorque += 1f;
                        //WheelColls[1].brakeTorque += 1f;
                    }
                }
            }
            else
            {
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
        foreach(GameObject wheel in frontWheels)
        {
            wheel.transform.Rotate(WheelColls[1].rpm * Mathf.PI/2 * Time.deltaTime,0, 0 );
        }
        foreach(GameObject wheel in backWheels)
        {
            wheel.transform.Rotate(WheelColls[1].rpm * Mathf.PI/2 * Time.deltaTime,0, 0 );
        }
     //   Wheels[0].transform.Rotate(WheelColls[0].rpm * Mathf.PI * Time.deltaTime, 0, 0);
        //Wheels[1].transform.Rotate(WheelColls[1].rpm * Mathf.PI * Time.deltaTime, 0, 0);
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
    }

    public void gameOver()
    {
        this.enabled = false;
        GetComponent<rotation>().StopAllCoroutines();
        GetComponent<rotation>().enabled = false;
        GetComponent<speedControl>().enabled = false;
        GetComponent<controls>().enabled = false;
        GetComponent<MeshCollider>().enabled = true;
       // GetComponent<playerTrackRotation>().enabled = false;
        foreach(GameObject wheel in frontWheels)
        {
            wheel.transform.parent = null;
            wheel.GetComponent<MeshCollider>().enabled = true;
            if(!wheel.GetComponent<Rigidbody>())
            {
                wheel.AddComponent(typeof(Rigidbody));
            }
            wheel.GetComponent<Rigidbody>().mass = 1;            
        }
        
        foreach(GameObject wheel in backWheels)
        {
            wheel.GetComponent<MeshCollider>().enabled = true;
        }

        foreach(BoxCollider bc in GetComponents<BoxCollider>())
        {
            bc.enabled = false;
        }

        

        PlayerRB.mass = 2;
        PlayerRB.constraints = RigidbodyConstraints.None;
     //   GetComponent<MeshCollider>().enabled = true;
    }


    void OnTriggerEnter(Collider other)
    {
       // Debug.Log("triggered");
        if(other.tag == "ground")
        {
            Debug.Log("triggered");
            gameOver();
           // this.enabled = false;
        }
    }
}
