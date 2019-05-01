using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endgame : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ground")
        {
        GetComponent<Moving>().gameOver();
        this.enabled = false;
        }
    }
}
