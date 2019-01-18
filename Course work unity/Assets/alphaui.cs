using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class alphaui : MonoBehaviour
{
    public Text bodyPos;
    public Text bodyRot;
    public Text pressTime;
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
    }
}
