using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    public static float distance;
    public static float speed;
    public float maxSpeed;
    void Start()
    {
            distance = 0f;
            maxSpeed = 100f;
    }
    void FixedUpdate()
    {
            speed = Mathf.Sin(Moving.accelTimer/2) * maxSpeed;
            distance += Mathf.Sign(speed) * GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity.magnitude * Time.fixedDeltaTime;               
    }
}
