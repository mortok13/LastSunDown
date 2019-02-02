using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuMoving : MonoBehaviour
{
    private byte i;
    public GameObject mRoadBlock;
    private GameObject[] road = new GameObject[10];
    private GameObject[] Wheels = new GameObject[2];
    private WheelCollider[] WheelColls = new WheelCollider[2];
    void Start()
    {
        Wheels[0] = GameObject.FindGameObjectWithTag("backWheel");
        Wheels[1] = GameObject.FindGameObjectWithTag("frontWheel");
        WheelColls[0] = GameObject.Find("bwCol").GetComponent<WheelCollider>();
        WheelColls[1] = GameObject.Find("fwCol").GetComponent<WheelCollider>();
        for(i = 0; i < 10; i++)
        {
            road[i] = Instantiate(mRoadBlock, new Vector3(-1+i, 0, 0), Quaternion.Euler(0, 180, 0));
        }
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(road[i].transform.position.x + 2 < transform.position.x)
        {
            road[i].transform.position = new Vector3(road[i].transform.position.x + 10,0,0);
        }
        i++;
        if(i == 10)
        {
            i = 0;
        }
    }
    void FixedUpdate()
    {
        WheelColls[0].motorTorque = 10f;
        WheelColls[1].motorTorque = 10f;
        Wheels[0].transform.Rotate(WheelColls[0].rpm * 2 * Time.deltaTime,0,0);
        Wheels[1].transform.Rotate(WheelColls[1].rpm * 2 * Time.deltaTime,0,0);
    }
}
