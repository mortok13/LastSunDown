using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    public static float speed;
    public float maxSpeed;
    private static Rigidbody PlayerRB;
    void Awake()
    {
        maxSpeed = 60f;
        PlayerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        speed = Mathf.Sin(Moving.accelTimer/2) * maxSpeed;
        RunStats.Distance += Mathf.Sign(speed) * PlayerRB.velocity.magnitude * Time.fixedDeltaTime;               
    }
}
