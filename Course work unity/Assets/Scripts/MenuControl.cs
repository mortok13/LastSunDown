using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    private Vector3 PlayerPos;
    public GameObject Road;
    private GameObject[] menuRoad = new GameObject[10];
    private byte i;
    void Start()
    {
        for(i = 0; i < 10; i++)
        {
            menuRoad[i] = Instantiate(Road, new Vector3(-1 + i,0,0), Quaternion.Euler(0, 180, 0));
        }
        i = 0;
        PlayerPos = transform.position;
    }
    void Update()
    {
        if(menuRoad[i].transform.position.x + 2 < transform.position.x)
        {
            Destroy(menuRoad[i]);
            menuRoad[i] = Instantiate(Road, new Vector3(Mathf.Floor(transform.position.x) + 7,0,0), Quaternion.Euler(0, 180, 0));
        }
        i++;
        if(i == 10)
        {
            i = 0;
        }
    }
}
