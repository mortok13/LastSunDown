using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationLeft : MonoBehaviour
{
    //private float rotationTime = 1f;

    private Quaternion qAngle;
    
    private int rotAngle;

    void Start()
    {
        rotAngle = 90;
        Debug.Log(rotAngle);
    }
    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, qAngle.normalized, Time.fixedDeltaTime);
    }

    public void rotModeControl(float angle)
    {
        this.enabled = true;
            Debug.Log(transform.rotation.y);
                qAngle = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,angle, 0));
    }
}
