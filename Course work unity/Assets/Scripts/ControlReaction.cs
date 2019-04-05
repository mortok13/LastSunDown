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
        if(Controls.forward)
        {
            Debug.Log("w");
        }
        if(Controls.back)
        {
            Debug.Log("s");
        }
        if(Controls.rotLeft)
        {
            Debug.Log("a");
        }
        if(Controls.rotRight)
        {
            Debug.Log("d");
        }
    }
}
