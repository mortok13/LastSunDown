using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block", menuName = "Block", order = 51)]
public class Block : ScriptableObject
{
    public Vector2 sizeXZ;
    public GameObject block;
    public void RotateForY(float angle)
    {
        block.transform.Rotate(0, angle, 0);
    }
    Block()
    {
    }
    public void Delete()
    {
        Destroy(this);
    }
}