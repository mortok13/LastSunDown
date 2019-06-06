using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleRotTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {   
        if(other.tag == "Player")
        {
            other.GetComponent<Controls>().SwitchSingleRotButtons(this.gameObject);
            this.gameObject.SetActive(false);
        }

    }
}
