using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontrol : MonoBehaviour
{
    public Text showDistance;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        showDistance.text = Mathf.Round(speedControl.distance).ToString();
    }
}
