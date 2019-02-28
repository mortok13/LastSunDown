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
    [SerializeField]
    private float speed;


    void Start()
    {
        speed = 10f;
        Wheels[0] = GameObject.FindGameObjectWithTag("backWheel");
        Wheels[1] = GameObject.FindGameObjectWithTag("frontWheel");
        WheelColls[0] = GameObject.Find("bwCol").GetComponent<WheelCollider>();
        WheelColls[1] = GameObject.Find("fwCol").GetComponent<WheelCollider>();
        for(i = 0; i < 10; i++)
        {
            road[i] = Instantiate(mRoadBlock, new Vector3(-1+i, 0, 0), Quaternion.identity);
        }
        i = 0;
        WheelColls[0].motorTorque = speed;
        WheelColls[1].motorTorque = speed;
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
      //  WheelColls[0].motorTorque = 10f;
       // WheelColls[1].motorTorque = 10f;
        WheelColls[0].motorTorque = speed;
        WheelColls[1].motorTorque = speed;
        Wheels[0].transform.Rotate(WheelColls[0].rpm*Mathf.PI * Time.deltaTime,0,0);
        Wheels[1].transform.Rotate(WheelColls[0].rpm*Mathf.PI * Time.deltaTime,0,0);
    }
}
