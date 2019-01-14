using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    void Start()
    {
        for (int x = 1; x < 5; x++)
        {
            for (int y = 0; y <= x; y++)
            {
                Instantiate(gameObject, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

}
