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
        rotAngle = 0;
        Debug.Log(rotAngle);
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, qAngle, 2 * Time.fixedDeltaTime);
    }

    public void rotModeControl(float angle)
    {
        qAngle = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,angle, 0));
    }
}
