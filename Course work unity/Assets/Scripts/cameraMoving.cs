﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMoving : MonoBehaviour
{
    public GameObject Player;
    private Vector3 cameraOffset;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        cameraOffset = transform.position - Player.transform.position;
    }
    void Update()
    {
        transform.position = Player.transform.position + cameraOffset;
    }
}
