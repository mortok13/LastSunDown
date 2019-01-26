using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedControl : MonoBehaviour
{
    public static float acceleration;
    public static float speed;
    public static float brake;
    public float maxSpeed;
    public static float angleSpeed;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
            speed = Mathf.Sin(moving.accelTimer/2) * maxSpeed;
            brake = maxSpeed - speed;            
            acceleration = Mathf.Cos(moving.accelTimer/2)/2;
            angleSpeed = speed / 0.201f;
        
    }
}
