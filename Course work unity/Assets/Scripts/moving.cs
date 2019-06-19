using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public static Rigidbody PlayerRB;
    private GameObject[] frontWheels;
    private GameObject[] backWheels;
    private WheelCollider[] WheelColls;
    public static float accelTimer;
    void Awake()
    {
                PlayerRB = GetComponent<Rigidbody>();
    }
    void Start()
    {
        frontWheels = GameObject.FindGameObjectsWithTag("frontWheel");
        backWheels = GameObject.FindGameObjectsWithTag("backWheel");
        WheelColls = FindObjectsOfType<WheelCollider>();
        foreach(WheelCollider wheelcol in WheelColls)
        {
            wheelcol.ConfigureVehicleSubsteps(15f, 7,14);
        }
        accelTimer = 0;
    }

    void Update()
    {
        if((Controls.forward || Controls.back))
        {
            if(Controls.forward)
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
    }

    void FixedUpdate()
    {
        if(Controls.forward && Controls.back)
        {
            WheelColls[0].motorTorque = 0;
            WheelColls[1].motorTorque = 0;
        }
        else if(Controls.forward || Controls.back)
        {
            if(Mathf.Abs(accelTimer) > 0.1f)
            {
                if(Controls.forward)
                {
                    foreach(WheelCollider wheelColl in WheelColls)
                    {
                    wheelColl.motorTorque = SpeedControl.speed;
                    wheelColl.brakeTorque = 0f;
                    }
                }
                else
                {
                    if(SpeedControl.speed <= 0)
                    {
                        foreach(WheelCollider wheelColl in WheelColls)
                        {
                        wheelColl.brakeTorque = 0f;
                        wheelColl.motorTorque = SpeedControl.speed;
                        }
                    }
                    else
                    {
                        accelTimer -= 3*Time.deltaTime;
                        foreach(WheelCollider wheelColl in WheelColls)
                        {
                        wheelColl.brakeTorque += 1f;
                        }
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
                    wheelColl.motorTorque -= 0.39f;
                }
                else if(wheelColl.motorTorque < -0.2f)
                {
                    wheelColl.motorTorque += 0.39f;
                }
                else
                {
                    wheelColl.motorTorque = 0f;
                }
            }
        }
        if(Mathf.Abs(PlayerRB.velocity.magnitude) >= 0.1f)
        {
            foreach(GameObject wheel in frontWheels)
            {
                wheel.transform.Rotate(WheelColls[1].rpm * Mathf.PI/2 * Time.deltaTime,0, 0 );
            }
            foreach(GameObject wheel in backWheels)
            {
                wheel.transform.Rotate(WheelColls[1].rpm * Mathf.PI/2 * Time.deltaTime,0, 0 );
            }
        }

        if(Controls.rotLeft || Controls.rotRight)
        {
            if(Controls.rotLeft)
            {
                PlayerRB.AddTorque(transform.right * (-10f));
            }
            
            if(Controls.rotRight)
            {
                PlayerRB.AddTorque(transform.right * (10f));
            }   
            GetComponent<Rotation>().deltaCurRotX(PlayerRB.rotation.eulerAngles.x);
        }
    }

    public void gameOver()
    {
        this.enabled = false;
        GetComponent<Rotation>().StopAllCoroutines();
        GetComponent<Rotation>().enabled = false;
        GetComponent<SpeedControl>().enabled = false;
        GetComponent<Controls>().enabled = false;

        foreach(GameObject col in GameObject.FindGameObjectsWithTag("wheelCol"))
        {
            col.SetActive(false);
        }
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
        GetComponent<MeshCollider>().enabled = true;
        GetComponent<Controls>().Pause(true);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ground")
        {
            Debug.Log("triggered gameOver");
            gameOver();
        }
    }
}
