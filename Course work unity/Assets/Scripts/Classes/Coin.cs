using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Coin", menuName = "Coin", order = 52)]
public class Coin : ScriptableObject
{
    public GameObject coin;
    public int value;

    Coin()
    {
        
    }
}
