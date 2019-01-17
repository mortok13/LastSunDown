using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMoving : MonoBehaviour
{
    public GameObject Player;
    private Vector3 cameraOffset;
    void Start()
    {
        cameraOffset = transform.position - Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + cameraOffset;
    }
}
