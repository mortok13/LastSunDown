using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    public static float acceleration;
    public static float distance;
    public static float speed;
    public static float brake;
    public float maxSpeed;
    public static float angleSpeed;
    void Start()
    {
            distance = 0f;
            maxSpeed = 100f;
    }
    void FixedUpdate()
    {
            speed = Mathf.Sin(Moving.accelTimer/2) * maxSpeed;
           // distance += Mathf.Sign(speed) * GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity.magnitude * Time.fixedDeltaTime;
            brake = maxSpeed - speed;            
            acceleration = Mathf.Cos(Moving.accelTimer/2)/2;
            angleSpeed = speed / 0.201f;     
    }
}
