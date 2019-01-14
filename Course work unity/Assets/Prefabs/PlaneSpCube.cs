using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpCube : MonoBehaviour
{
    int x = 0, y = 0, z = 0, limit;
    void Start()
    {
        limit = 0;
    }

    void FixedUpdate()
    {
        if(limit !=5)
        {
            Instantiate(new GameObject("CubeCubeCube"), new Vector3(x, y, z), Quaternion.identity);
            limit++;
            x++;
        }
    }
}
