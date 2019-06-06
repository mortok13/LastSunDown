using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TRotTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {   
        if(other.tag == "Player")
        {
            other.GetComponent<Controls>().SwitchRotButtons(this.gameObject);
            this.gameObject.SetActive(false);
        }

    }
}
