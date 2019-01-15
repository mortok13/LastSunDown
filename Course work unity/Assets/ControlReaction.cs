using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlReaction : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(controls.forward)
        {
            Debug.Log("w");
        }
        if(controls.back)
        {
            Debug.Log("s");
        }
        if(controls.rotLeft)
        {
            Debug.Log("a");
        }
        if(controls.rotRight)
        {
            Debug.Log("d");
        }
    }
}
