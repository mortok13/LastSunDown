using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMoving : MonoBehaviour
{
    private byte i;
    public GameObject mRoadBlock;
    private GameObject[] road = new GameObject[10];
    [SerializeField]
    private Block[] bgBlocks;
    private float backgroundX;
    private int roadX;
    private GameObject[] frontWheels;
    private GameObject[] backWheels;
    private WheelCollider[] WheelColls;  
    [SerializeField]
    private float speed;


    void Start()
    {
        backgroundX = -2;
        roadX = -2;
        speed = 10f;
        backWheels = GameObject.FindGameObjectsWithTag("backWheel");
        frontWheels = GameObject.FindGameObjectsWithTag("frontWheel");
        WheelColls = GameObject.FindObjectsOfType<WheelCollider>();
        for(i = 0; i < 10; i++)
        {
            road[i] = Instantiate(mRoadBlock, new Vector3(roadX, 0, 0), Quaternion.identity);
            int tmp = Random.Range(0, bgBlocks.Length);
            Instantiate(bgBlocks[tmp].block, new Vector3(backgroundX + bgBlocks[tmp].sizeXZ.x/2 - 0.5f, 0, 1), Quaternion.identity, road[i].transform);
            backgroundX += bgBlocks[tmp].sizeXZ.x;
            while(roadX < backgroundX - 1)
            {
                roadX++;
                Instantiate(mRoadBlock, new Vector3(roadX, 0 ,0), Quaternion.identity, road[i].transform);
            }
        }
        i = 0;
        WheelColls[0].motorTorque = speed;
        WheelColls[1].motorTorque = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(road[i].transform.position.x + 10 < transform.position.x)
        {
            Destroy(road[i]);
            road[i] = Instantiate(mRoadBlock, new Vector3(roadX, 0, 0), Quaternion.identity);
            int tmp = Random.Range(0, bgBlocks.Length);
            Instantiate(bgBlocks[tmp].block, new Vector3(backgroundX + bgBlocks[tmp].sizeXZ.x/2 - 0.5f, 0, 1), Quaternion.identity, road[i].transform);
            backgroundX += bgBlocks[tmp].sizeXZ.x;
            while(roadX < backgroundX - 1)
            {
                roadX++;
                Instantiate(mRoadBlock, new Vector3(roadX, 0 ,0), Quaternion.identity, road[i].transform);
            }
            i++;
            if(i == 10)
            {
                i = 0;
            }
        }

    }
    void FixedUpdate()
    {
      //  WheelColls[0].motorTorque = 10f;
       // WheelColls[1].motorTorque = 10f;
       foreach(WheelCollider wc in WheelColls)
        {
            wc.motorTorque = speed;
        }
       // wc.motorTorque = speed;
       // WheelColls[1].motorTorque = speed;
        foreach(GameObject wheel in frontWheels)
        {
            wheel.transform.Rotate(WheelColls[0].rpm*Mathf.PI * Time.deltaTime,0,0);
        }
        foreach(GameObject wheel in backWheels)
        {
            wheel.transform.Rotate(WheelColls[0].rpm*Mathf.PI * Time.deltaTime,0,0);
        }
    }
}
