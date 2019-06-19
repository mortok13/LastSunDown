using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField]
    private int value;
    [SerializeField]
    private AudioSource sPickUp;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            sPickUp.Play();
            RunStats.Money += value;
            this.enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
            foreach(Transform ts in transform)
            {
                Destroy(ts.gameObject);
            }
            StartCoroutine("waitDestroy");
        }
    }
    
    private IEnumerator waitDestroy()
    {
        yield return new WaitWhile(() => sPickUp.isPlaying);
        Destroy(this.gameObject);
        Debug.Log("vhs destroyed");
        StopCoroutine("waitDestroy");
    }

}
