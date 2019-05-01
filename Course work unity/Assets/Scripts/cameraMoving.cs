using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField,Range(0, 1)]
    private float startDelay;

    private float offsetZ;

    [SerializeField, Range(0.001f,1)]
    private float lerpIntensity;
    public GameObject player;
    //private Vector3 cameraOffset;
    void Awake()
    {
        offsetZ = -4;
        player = GameObject.FindGameObjectWithTag("Player");
    //    cameraOffset = transform.position - player.transform.position; 
    }
    void Start()
    {
       transform.position = new Vector3(player.transform.position.x + 1.5f, 0.93f, -4f);
       transform.Rotate(new Vector3(-15.5f, 0, 0)); 
       StartCoroutine("CameraMainControl");
    }
    void Update()
    {
       //Debug.Log("camera player: " + player.transform.position);
    }

    private IEnumerator CameraMainControl()
    {
        yield return new WaitUntil(() => player.transform.position.x >= transform.position.x);
        Debug.Log("Camera Control has started");
        yield return new WaitForSeconds(startDelay);
        Debug.Log("Camera Delay ended, Position Lerp to player body started");
        while(true)
        {
            SetTransformPosX(Mathf.Lerp(transform.position.x, player.transform.position.x, lerpIntensity));
            SetTransformPosZ(Mathf.Lerp(transform.position.z, player.transform.position.z + offsetZ, lerpIntensity/2));
            yield return null;
        }
    }

    private void SetTransformPosX(float newPos)
    {
        transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
    }

    private void SetTransformPosZ(float newPos)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, newPos);
    }

    private void SetTransformPosY(float newPos)
    {
        transform.position = new Vector3(transform.position.x, newPos, transform.position.z);
    }

    private void SetRotationEulerX(float newRotAngle)
    {
        Vector3 resultRot = new Vector3(newRotAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        resultRot -= transform.rotation.eulerAngles;
        transform.Rotate(resultRot);

    }

    public IEnumerator ChangeCameraMode()
    {
        float curAngle;
        if(Rotation.movingMode)
        {
            curAngle = -15.5f;
            while(transform.position.y <= 1.929f || curAngle <= 29.99f || offsetZ <= -1.51f)
            {
                SetTransformPosY(Mathf.Lerp(transform.position.y, 1.93f, lerpIntensity/2));
               // Debug.Log(Mathf.Lerp(transform.rotation.eulerAngles.x, 30, lerpIntensity));
                curAngle = Mathf.Lerp(curAngle, 30, lerpIntensity/2);
                offsetZ = Mathf.Lerp(offsetZ, -1.5f, lerpIntensity);
                SetRotationEulerX(curAngle);
                yield return null;
            }
            offsetZ = -1.5f;
        }
        else
        {
            curAngle = 30;
            while(transform.position.y >= 0.929f || curAngle >= -15.49f || offsetZ >= -3.99f)
            {
                SetTransformPosY(Mathf.Lerp(transform.position.y, 0.93f, lerpIntensity/2));
                //Debug.Log(Mathf.Lerp(transform.rotation.eulerAngles.x, 30, lerpIntensity));
                curAngle = Mathf.Lerp(curAngle, -15.5f, lerpIntensity/4);
                offsetZ = Mathf.Lerp(offsetZ, -4, lerpIntensity);
                SetRotationEulerX(curAngle);
                yield return null;
            }
            offsetZ = -4;
        }
        Debug.Log("Camera moved");
        StopCoroutine("ChangeCameraMode");
    }
    

}
