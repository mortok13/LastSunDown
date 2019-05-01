using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block", menuName = "Block", order = 51)]
public class Block : ScriptableObject
{
    //                 x = x, y = x;
    public Vector2 sizeXZ;
    public GameObject block;
    Block()
    {
    }
    // public void Delete()
    // {
    //     Destroy(this);
    // }
}