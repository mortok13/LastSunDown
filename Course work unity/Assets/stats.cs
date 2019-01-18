using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stats : MonoBehaviour
{
    public static bool isInAir;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isInAir);
    }
    void OnCollisionStay(Collision other)
    {
        isInAir = false;
    }
    void OnCollisionExit(Collision other)
    {
        isInAir = true;
    }
}
