using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class alphaui : MonoBehaviour
{
    public Text bodyPos;
    public Text speedText;
    public Text bodyRot;
    public Text pressTime;
    public Text isInAir;
    public Text AngleSpeed;
    public Text acceleration;
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bodyPos.text = "Body position: " + player.transform.position;
        bodyRot.text = "Body rotation: " + player.transform.rotation;
        pressTime.text = "Press time of W/S: " + moving.accelTimer;
        isInAir.text = "Is In Air: " + stats.isInAir;
        speedText.text = "Speed: " + speedControl.speed;
        AngleSpeed.text = "Angle Speed: " + speedControl.angleSpeed;
        acceleration.text = "acceleration: " + speedControl.acceleration;
    }
}
