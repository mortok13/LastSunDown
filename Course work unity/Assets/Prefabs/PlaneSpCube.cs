using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpCube : MonoBehaviour
{
    int x = 0, y = 0, z = 0, limit = 0, sCount;
    public GameObject CubeCubeCube;
    void Start()
    {
    }

    void FixedUpdate()
    {
       
        if (limit != 5)
        {
            Instantiate(CubeCubeCube, new Vector3(x, y, z), Quaternion.identity);
            limit++;
            x++;
            y++;
        }
    }
}
