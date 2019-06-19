using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block", menuName = "Block", order = 51)]
public class Block : ScriptableObject
{
    //                 x = x, y = z;
    public Vector2 sizeXZ;
    public GameObject block;
    Block()
    {
    }
}