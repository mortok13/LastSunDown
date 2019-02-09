using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackControl : MonoBehaviour
{
    public GameObject road;
    public GameObject corner;
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(i == 0)
                {
                    Instantiate(road, new Vector3(1+j,0,0), Quaternion.identity);
                }
                else if(i == 1)
                {
                    Instantiate(road, new Vector3(8, 0, 1+j),Quaternion.Euler(0,-90,0));
                }
                else if(i == 2)
                {
                    Instantiate(road, new Vector3(8-j, 0, 8), Quaternion.Euler(0,-180,0));
                }
                else if(i == 3)
                {
                    Instantiate(road, new Vector3(0, 0, 8-j), Quaternion.Euler(0,90,0));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
